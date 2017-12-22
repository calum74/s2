#pragma once

namespace S2
{
	class ProgressMonitor
	{
	public:
		// Running:
		virtual void TotalRunTime(double seconds)=0;
		virtual void RunPreset(const std::string & presetName, double duration) = 0;
		virtual void RunProgram(const std::string & progName, double duration) = 0;
		virtual void RunStep(const std::string & stepDescription, double duration) = 0;
		virtual void RunCompleted() = 0;

		// Output options:
		virtual void GeneratorState(double time, ChannelId channel, double amplitude, double frequency, BuiltinWaveform wf) = 0;

		// Scanning:
		virtual void HeartRateValue(double h) = 0;
		virtual void HeartRateGood() = 0;
		virtual void HeartRateBad() = 0;
		virtual void ScanProgress() = 0;
		virtual void ScanCompleted() = 0;

		// Status:
		virtual void PulseFound(int id, const std::string & name)=0;
		virtual void GeneratorFound(int id, const std::string & name)=0;

		// Errors:
		virtual void Error(const std::string & msg)=0;

		void HandleException();
	};

	class DefaultProgressMonitor : public ProgressMonitor
	{
	public:
		DefaultProgressMonitor(Verbosity verbosity, std::ostream&os);
		void TotalRunTime(double seconds);

		void RunPreset(const std::string & presetName, double duration);
		void RunProgram(const std::string & progName, double duration);
		void RunStep(const std::string & stepDescription, double duration);
		void RunCompleted();

		// Output options:
		void GeneratorState(double time, ChannelId channel, double amplitude, double frequency, BuiltinWaveform wf);

		// Scanning:
		void HeartRateValue(double h);
		void HeartRateGood();
		void HeartRateBad();
		void ScanProgress();
		void ScanCompleted();

		// Status:
		void PulseFound(int id, const std::string & name);
		void GeneratorFound(int id, const std::string & name);

		void Error(const std::string & msg);
	private:
		Verbosity verbosity;
		std::ostream &output;
	};
}
