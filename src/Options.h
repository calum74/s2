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

	class InvalidUnit : public InvalidCommandLineArgument
	{
	public:
		InvalidUnit(const char * option);
	};

	enum Verbosity
	{
		Quiet, // No output
		Short, // Just main program stages
		Normal,// All program stages and frequencies
		Verbose, // All generator outputs
		Debug	// Everything
	};


	class OptionsVisitor
	{
	public:
		void OnOption(const char * option);
		void Waveform(const char * name);
	protected:
		virtual void Duration(double seconds);
		virtual void UnrecognisedOption(const char * option);
		virtual void Generator(int generator);
		virtual void Channel(int channel);
		virtual void Pulse(int id);
		virtual void Amplitude(double amplitude);
		virtual void Frequency(double frequency);
		virtual void Output(bool on);
		virtual void Waveform(BuiltinWaveform wf);
		virtual void Waveform(const WaveData&);
		virtual void Send(const char * cmd);
		virtual void Lock(bool b);
		virtual void Offset(double p);
		virtual void Phase(int degrees);
		virtual void Sync(bool on);
		virtual void Duty(double percent);
		virtual void Preset(const char * filename);
		virtual void Simulation(bool b);
		virtual void Name(const char*);
		virtual void Program(int);
		virtual void Code(const char*);
		virtual void Verbosity(S2::Verbosity);
		virtual void Loop(bool);
		virtual void Iterations(int i);

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

		const double tissue_factor=1.4142135623731;
		const double bp_to_hz = 8.63808777288135E+17;
		const double mw_to_hz = 2.25234274582033E+23;

		const char * command;
		enum Verbosity verbosity;
		int iterations; // Loop a fixed number of iterations. 0 = infinite, 1=once etc
						// 		double stableThresh

		// RampUp
		double ramp; // The maximum rate of change of voltage, in V/s
		double frequency;
		double amplitude;
		double duration;
		bool simulation;

		int pulse; // 0=default.
		int generator; // 0=default, otherwise need to specify.
		int channel; // 0 = both channels, 1=Ch1, 2=Ch2.

		bool loop;

		std::string preset;
	    std::string dataDir;  // ~/.s2

		// Looks for a given filename in the data directory.
		std::string DataFile(const char * name) const;

		void Visit(OptionsVisitor & visitor) const;
		void VisitFile(std::istream & file, OptionsVisitor & visitor) const;
		void SaveOptions() const;
		void SaveOptions(std::ostream&) const;

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
		void Verbosity(S2::Verbosity);
		void Loop(bool);
		void Iterations(int i);

	private:
		// Gets the name of the data directory.
		// Creates the directory if it doesn't exist.
		// e.g. ~/.s2
		std::string DataDirectory() const;

	};

}
