#pragma once

namespace S2
{
	class ProgressMonitor;

	struct OutputState
	{
		// NaN means unknown
		double amplitudeV, frequencyHz;

		BuiltinWaveform waveform;
		WaveData wavedata; // Only if waveform==custom
	};

	struct OutputStep
	{
		std::string programName, itemName;

		OutputState state;

		double stepDuration;
	};


	// Any preset or program.
	class Runnable
	{
		virtual double Duration() const = 0;
		virtual std::string Description() const = 0;

		virtual void GetStep(double time, OutputStep &step) const=0;

		void Run(Channel & channel, ProgressMonitor & pm, double startAt = 0.0) const;
	};

	class ProgramStep
	{
	public:
		ProgramStep(const char * str);
		double f1, f2, duration;
	};

	class Program
	{
	public:
		Program(const std::string & program);

		void Run(const Options & options, Channel &c);

		double TotalDuration() const;
		int TotalSteps() const;
		double StepDuration(int s) const;
		double StepFrequency(int s) const;
	private:
		std::string program;
		std::vector<ProgramStep> steps;
	};
}
