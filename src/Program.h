#pragma once

namespace S2
{
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

	class ProgressMonitor
	{
	public:
		// Running:
		virtual void RunPreset(const std::string & presetName, double duration) = 0;
		virtual void RunProgram(const std::string & progName, double duration) = 0;
		virtual void RunStep(const std::string & stepDescription, double duration) = 0;
		virtual void RunCompleted() = 0;

		// Output options:
		virtual void OutputState(int generator, int channel, double amplitude, double frequency, BuiltinWaveform wf) = 0;

		// Scanning:
		virtual void HeartRateValue(double h) = 0;
		virtual void HeartRateGood() = 0;
		virtual void HeartRateBad() = 0;
		virtual void ScanProgress() = 0;
		virtual void ScanCompleted() = 0;

		// Status:
		virtual void PulseFound(int id, const std::string & name)=0;
		virtual void GeneratorFound(int id, const std::string & name)=0;
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