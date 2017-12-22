#include "S2.h"
#include <iostream>
#include <string>
#include <fstream>
#include <sys/time.h>
#include <cmath>

// !! Into class
class Runner : public S2::OptionsVisitor
{
	int generator, channel;
	S2::MultiChannelSequence scheduler;
	S2::Database database;
	const S2::Options & options;
	S2::Devices & devices;
	S2::StreamFactory &streamFactory;

public:
	Runner(const S2::Options & options, S2::Devices & devices, S2::StreamFactory &sf) : options(options), devices(devices), streamFactory(sf)
	{
	}

	void Run(S2::ProgressMonitor&pm)
	{
		pm.TotalRunTime(scheduler.Duration());

		scheduler.Begin();

		S2::ChannelState state;
		timeval t0;
		gettimeofday(&t0, nullptr);
		bool loop = options.loop;
		state.amplitude = options.amplitude;
		while(scheduler.Next(state, 1.0, loop))
		{
			timeval t1;
			gettimeofday(&t1, nullptr);
			double d = (t1.tv_sec - t0.tv_sec)+0.000001*(t1.tv_usec-t0.tv_usec);

			double wait = state.time - d;

			if(options.simulation)
				wait = (state.time/100.0)-d;

			if(wait>0)
			{
				// if(!options.simulation)
				// std::cout << "Sleeping for " << wait << "s\n";
				S2::Sleep(wait);
			}

			// Channel & c =
			auto &channel = devices.GetGenerator(state.channelId.first).GetChannel(state.channelId.second);

			if(!std::isnan(state.amplitude))
				channel.Amplitude(state.amplitude);
			if(!std::isnan(state.frequency))
				channel.Frequency(state.frequency);

			pm.GeneratorState(state.time, state.channelId, state.amplitude, state.frequency, state.waveform);

			//std::cout << state.channelId.first << "." << state.channelId.second << " " << state.time << "s " // << state.frequencyHz << "Hz\n";
		}
	}

	void Generator(int id)
	{
		generator=id;
	}

	void Channel(int id)
	{
		channel=id;
	}

	void Add(const std::shared_ptr<S2::Sequence> & seq)
	{
		devices.GetGenerator(generator).Open(devices, streamFactory);
		scheduler.Add(S2::ChannelId(generator,channel), seq);
	}

	void Preset(const char * filename)
	{
		std::ifstream preset(filename);
		if(!preset)
			throw S2::IOError("Failed to load preset");
		Add(std::make_shared<S2::Preset>(options, preset));
	}

	class ProgramId : public S2::DatabaseVisitor
	{
	public:
		ProgramId(int id) : id(id) { }
		void DatabaseRow(int row, const std::string & line)
		{
			if(row==id)
			{
				this->line = line;
			}
		}
		std::string line;
	private:
		int id;
	};

	void Program(int id)
	{
		std::cout << "Queuing program " << id << std::endl;

		ProgramId getId(id);
		database.VisitDataFiles(options, getId);
		if(getId.line.empty())
			throw S2::ValueOutOfRange("Program not found");
		std::cout << S2::DatabaseProgram(options, getId.line).code << std::endl;
		Add(std::make_shared<S2::DatabaseProgram>(options, getId.line));
	}

	void Code(const char * code)
	{
		scheduler.Add(S2::ChannelId(generator,channel), std::make_shared<S2::Program>(options, code));
	}

	void Amplitude(double v)
	{

	}

	void Waveform(S2::BuiltinWaveform wf)
	{
	}

};

int S2::Run(const Options & options, ProgressMonitor &pm, StreamFactory &sf)
{
	try
	{
		Devices devices(options);
		Runner run(options, devices, sf);
		options.Visit(run);
		run.Run(pm);
	}
	catch(...)
	{
		pm.HandleException();
		return 1;
	}

	return 0;
}
