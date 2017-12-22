#pragma once
#include <map>

namespace S2
{
	typedef std::pair<int,int> ChannelId;

	// The desired state
	struct ChannelState
	{
		ChannelId channelId;
		double time;	// Seconds from begining

		bool output;	// Whether the output is on or off

		// NaN means unknown
		double amplitude, frequency;

		BuiltinWaveform waveform;
		WaveData wavedata; // Only if waveform==custom

		// In/out.
		// As an out value, reports the duration until the next step
		// As an input, requests the new timestamp, given time + stepDuration
		// If the output is continuous (e.g. a ramp), then the caller
		// needs to specify the stepDuration manually.
		double stepDuration;
		bool operator<(const ChannelState & other) const;
	};


	// A sequence of commands
	class Sequence
	{
	public:
		// What is the total duration of this sequence
		virtual double Duration() const = 0;

		// Gets the state at a particular timepoint.
		virtual void GetState(double time, ChannelState & state) const = 0;
	};

	class ChannelInUse : public std::runtime_error
	{
	public:
		ChannelInUse(ChannelId channel);
		ChannelId channel;
	};

	class ChainedSequence : public Sequence
	{
	public:
		ChainedSequence(const std::shared_ptr<Sequence> s1, const std::shared_ptr<Sequence> &s2);
		double Duration() const;
		void GetState(double time, ChannelState & state) const;
	private:
		std::shared_ptr<Sequence> s1, s2;
	};

	std::shared_ptr<Sequence> Chain(const std::shared_ptr<Sequence>&s1,const std::shared_ptr<Sequence>&s2);

	// Merges multiple channels
	class MultiChannelSequence
	{
	public:
		void Add(ChannelId, const std::shared_ptr<Sequence> &seq);
		double Duration() const;
		void Begin();
		bool Next(ChannelState &state, double stepSize, bool loop);
	private:
		std::map<ChannelId, std::shared_ptr<Sequence>> sequences;
		std::vector<ChannelState> heap;
	};

	class ProgressMonitor;

	class SyntaxError : public std::runtime_error
	{
	public:
		SyntaxError(const char * position);
	};

	// Flyweight - decodes a program text
	class ProgramStep : public Sequence
	{
	public:
		ProgramStep(const Options & options, const char *& str, const char * end);
		double f1, f2, duration, amplitude, offset, fmultiplier, foffset, l;
		int waveform;

		double Duration() const;
		void GetState(double time, ChannelState & state) const;

	private:
		double ReadFrequency(const Options & options, const char *&str, const char *end);
		double ReadNumber(const char *&str, const char *end);
	};

	class Code : public Sequence
	{
	public:
		Code(const Options & options);
		double Duration() const;
		void GetState(double time, ChannelState & state) const;
	protected:
		virtual std::pair<const char*, const char*> GetCode() const=0;
		const Options & options;
	};

	class Program : public Code
	{
	public:
		Program(const Options & options, const std::string &name, const std::string & program);
		Program(const Options & options, const std::string & program);
	protected:
		std::pair<const char*, const char*> GetCode() const;
		std::string name, code;
	};
}
