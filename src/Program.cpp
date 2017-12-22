#include "S2.h"
#include <cstring>
#include <cmath>
#include <cassert>

S2::Program::Program(const Options & options, const std::string & c) : Code(options), code(c)
{
}

S2::Program::Program(const Options & options, const std::string & name, const std::string & c) : Code(options), name(name), code(c)
{
}

std::pair<const char*, const char*> S2::Program::GetCode() const
{
	return std::make_pair(code.c_str(), code.c_str()+code.length());
}

void S2::Code::GetState(double time, ChannelState & state) const
{
	if(time>=0.0)
	{
		auto code = GetCode();
		for(auto i = code.first; i!=code.second;)
		{
			ProgramStep p(options, i, code.second);
			if(time < p.Duration())
			{
				p.GetState(time, state);
				return;
			}
			time -= p.Duration();
		}
	}
	state.output = false;
}

double S2::Code::Duration() const
{
	double s=0.0;
	auto code = GetCode();
	for(const char * p=code.first; p<code.second; )
	{
		ProgramStep step(options, p, code.second);
		s+=step.Duration();
	}

	return s;
}

const double c=299792458.0;

double scaleF(double f, double limit)
{
	while(f>limit)
		f*=0.5;
	return f;
}

S2::ProgramStep::ProgramStep(const Options & options, const char *&s, const char * end)
{
	amplitude = std::nan(nullptr);

	duration = 180;	// !! Is this configurable ??
	// !! Also frequency modulations etc.
	f1=f2 = std::nan(nullptr);
	bool dash=false;
	char * next;
	double n, f;
	while(s<end)
	{
		switch(*s)
		{
			case '=':
				s++;
				duration = ReadNumber(s, end);
				break;
			case ',':
				++s;
				return;
			case 0:
				return;
			case '-':
				dash=true;
				++s;
				break;
			case 'A':
				amplitude = ReadNumber(s,end);
				break;
			case 'W':
				++s;
				waveform = ReadNumber(s, end);
				break;
			case 'L':
				++s;
				l = ReadNumber(s, end);
				break;
			case 'O':
				++s;
				offset = ReadFrequency(options, s, end);
				break;
			case 'F':
				++s;
				fmultiplier = ReadNumber(s,end);
				break;
			case 'C':
				++s;
				foffset = ReadNumber(s,end);
				break;
			case ' ':
				++s;
				break;
			default:
				(dash ? f2:f1) = ReadFrequency(options, s, end);
				if(!dash) f2=f1;
				break;
		}
	}
}

double S2::ProgramStep::ReadNumber(const char*&s, const char *end)
{
	char * next;
	double n = std::strtod(s, &next);
	if(next==s) throw SyntaxError(s);
	s = next;
	return n;
}

double S2::ProgramStep::ReadFrequency(const Options & options, const char *&str, const char *end)
{
	switch(*str)
	{
	case 'M':
		++str;
		return ReadNumber(str,end);	// !! This is wrong
		break;
	case 'B':
		++str;
		return scaleF(options.bp_to_hz/ReadNumber(str,end), 5e6);
		break;
	default:
		return ReadNumber(str,end);
	}
}

void S2::ProgramStep::GetState(double time, ChannelState&output) const
{
	output.output = true;
	output.frequency = f1 + (f2-f1) * (time/duration);
	output.stepDuration = f1==f2 ? duration : 0.0;
}

double S2::ProgramStep::Duration() const
{
	return duration;
}

void S2::MultiChannelSequence::Add(ChannelId channel, const std::shared_ptr<Sequence> &sequence)
{
	bool has0 = sequences.find(ChannelId(channel.first,0))!=sequences.end();
	bool has1 = sequences.find(ChannelId(channel.first,1))!=sequences.end();
	bool has2 = sequences.find(ChannelId(channel.first,2))!=sequences.end();

	if((channel.second==0 && (has1||has2))|| (has0 && channel.second!=0))
		throw ChannelInUse(channel);

	const auto & existing = sequences.find(channel);
	if(existing == sequences.end())
		sequences.insert(std::make_pair(channel, sequence));
	else
		existing->second = Chain(existing->second, sequence);
}

void S2::MultiChannelSequence::Begin()
{
	heap.clear();
	heap.reserve(sequences.size());
	for(auto & p : sequences)
	{
		ChannelState state;
		state.channelId = p.first;
		state.time = 0;
		p.second->GetState(0, state);
		heap.push_back(state);
	}
	std::make_heap(heap.begin(), heap.end());
}

bool S2::ChannelState::operator<(const ChannelState & other) const
{
	return time > other.time || (time == other.time && channelId > other.channelId);
}

bool S2::MultiChannelSequence::Next(ChannelState & state, double stepSize, bool loop)
{
	if(heap.empty()) return false;
	state = heap.front();
	std::pop_heap(heap.begin(), heap.end());
	ChannelState newState;
	newState.channelId = state.channelId;
	newState.time = state.time + (state.stepDuration==0.0 ? stepSize : state.stepDuration);
	auto & sequence = sequences[state.channelId];
	auto duration = sequence->Duration();
	double timestamp = newState.time;
	if(loop)
	{
		// Wrap newState.time
		double intPart;
		timestamp = duration * std::modf(timestamp/duration, &intPart);
	}

	if(timestamp < duration)
	{
		sequence->GetState(timestamp, newState);
		heap.back() = newState;
		std::push_heap(heap.begin(), heap.end());
	}
	else
	{
		heap.pop_back();
		state.output=false;
		state.amplitude=0;
		state.frequency=0;
	}
	return true;
}

double S2::MultiChannelSequence::Duration() const
{
	double m = 0.0;
	for(const auto & i : sequences)
		m = std::max(m, i.second->Duration());
	return m;
}

S2::ChannelInUse::ChannelInUse(ChannelId channel) : std::runtime_error("The specified channel is already used"), channel(channel)
{
}

std::shared_ptr<S2::Sequence> S2::Chain(const std::shared_ptr<Sequence> &s1, const std::shared_ptr<Sequence> &s2)
{
	return std::make_shared<ChainedSequence>(s1, s2);
}

S2::ChainedSequence::ChainedSequence(const std::shared_ptr<Sequence> s1, const std::shared_ptr<Sequence> &s2) : s1(s1), s2(s2)
{
}

double S2::ChainedSequence::Duration() const
{
	return s1->Duration() + s2->Duration();
}

void S2::ChainedSequence::GetState(double time, ChannelState & state) const
{
	double t1 = s1->Duration();
	return time < t1 ? s1->GetState(time, state) : s2->GetState(time-t1, state);
}

class Validator : public S2::DatabaseVisitor
{
public:
	Validator(const S2::Options &options) : options(options)
	{
	}

	const S2::Options & options;

	void DatabaseRow(int rowId, const std::string & line)
	{
		S2::DatabaseProgram p(options, line);

		try
		{
			p.Duration();
		}
		catch(...)
		{
			std::cout << "Failed to read program: " << line << ", code=" << p.code << std::endl;
		}
	}
};

int S2::ValidateDatabase(const Options &options)
{
	Database db;
	Validator validator(options);
	db.VisitDataFiles(options, validator);
	return 0;
}

S2::SyntaxError::SyntaxError(const char * pos) : std::runtime_error("Invalid program format")
{
}

