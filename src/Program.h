#pragma once

namespace S2
{
	// The desired state
	struct OutputState
	{
		bool output;	// Whether the output is on or off

		// NaN means unknown
		double amplitudeV, frequencyHz;

		BuiltinWaveform waveform;
		WaveData wavedata; // Only if waveform==custom

		double stepDuration;
	};


	// A sequence of commands
	class Sequence
	{
		// What is the total duration of this sequence
		virtual double Duration() const = 0;

		// Gets the state at a particular timepoint.
		virtual void GetState(double time, OutputState & state) const = 0;
	};

	class RunSequence
	{
	public:
		RunSequence(Sequence & seq, Channel & channel);

		OutputState current;

	private:
		const Sequence & sequence;
		Channel & channel;
	};

	class ProgressMonitor;

	class Scheduler
	{
	public:
		void Add(const Sequence & seq, Channel & channel, bool loop, double offset);
		void Run(const Options & options, ProgressMonitor &pm);
	private:
		std::vector<RunSequence> sequences;
	};

	class ProgramStep : public Sequence
	{
	public:
		ProgramStep(const char * str);
		double f1, f2, duration;

		double Duration() const;
		void GetState(double time, OutputState & state) const;
	};

	class Program : public Sequence
	{
	public:
		Program(const std::string & name, const std::string & code);
		Program(const std::string & program);

		double Duration() const;
		void GetState(double time, OutputState & state) const;
	private:
		void Initialize(const std::string&);
		std::string name, program;
		std::vector<ProgramStep> steps;
	};
}
