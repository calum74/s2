#pragma once
#include <string>

namespace S2
{
	class InvalidCommandLineArgument : std::runtime_error
	{
	public:
		InvalidCommandLineArgument(const char * option);
		const char * option;
	};

	class ValueOutOfRange : public InvalidCommandLineArgument
	{
	public:
		ValueOutOfRange(const char * option);
	};

	enum BuiltinWaveform
	{
		sine, square, sawtooth
	};

	class OptionsVisitor
	{
	public:
		void OnOption(const char * option);
		void Waveform(const char * name);
	protected:
		virtual void Verbose(bool value)=0;
		virtual void Duration(double seconds)=0;
		virtual void UnrecognisedOption(const char * option)=0;
		virtual void Generator(int generator)=0;
		virtual void Channel(int channel)=0;
		virtual void Pulse(int id) = 0;
		virtual void Amplitude(double amplitude)=0;
		virtual void Frequency(double frequency)=0;
		virtual void Output(bool on)=0;
		virtual void Waveform(BuiltinWaveform wf)=0;
		virtual void Waveform(const WaveData&) = 0;
		virtual void Send(const char * cmd) = 0;
		virtual void Lock(bool b) = 0;
		virtual void Offset(double p) = 0;
		virtual void Phase(int degrees) = 0;
		virtual void Sync(bool on)=0;
		virtual void Duty(double percent) = 0;
		virtual void Preset(const char * filename) = 0;
		virtual void Simulation(bool b) = 0;
	private:
		bool parseInt(const char * option, const char * variable, int min, int max, int & output);
		bool parseSuffix(const char * option, const char * variable, const char * unitOptional, double min, double max, double & output);
		bool parseDuration(const char * option, const char * variable, double & output);
		bool parseBool(const char * option, const char * variable, bool &output);
		bool parseString(const char * option, const char * variable, const char*&output);
		bool parsePercent(const char * option, const char * variable, double min, double max, double &output);

		bool splitOption(const char * option, const char * variable, const char *&value, const char*&unit);
	};

	struct Options : public OptionsVisitor
	{
		Options(int argc, const char *argv[]);
		int argc;
		const char **argv;

		const char * command;
		bool verbose;
		int iterations; // Loop a fixed number of iterations. 0 = infinite, 1=once etc
						// 		double stableThresh

		double frequency;
		double amplitude;
		double duration;
		bool simulation;

		int pulse; // 0=default.
		int generator; // 0=default, otherwise need to specify.
		int channel; // 0 = both channels, 1=Ch1, 2=Ch2.

		std::string preset;

		void Visit(OptionsVisitor & visitor) const;
		void VisitFile(std::istream & file, OptionsVisitor & visitor) const;
		void SaveOptions() const;
		void SaveOptions(std::ostream&) const;

		virtual void Verbose(bool value);
		virtual void Duration(double seconds);
		virtual void UnrecognisedOption(const char * option);
		virtual void Generator(int generator);
		virtual void Channel(int channel);
		void Pulse(int id);
		virtual void Amplitude(double amplitude);
		virtual void Frequency(double frequency);
		virtual void Output(bool on);
		virtual void Waveform(BuiltinWaveform wf);
		virtual void Waveform(const WaveData &);
		virtual void Send(const char * cmd);
		virtual void Lock(bool on);
		virtual void Offset(double d);
		virtual void Phase(int degrees);
		virtual void Sync(bool on);
		virtual void Duty(double percent);
		void Preset(const char * filename);
		void Simulation(bool b);
	};

}
