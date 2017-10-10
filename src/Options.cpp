#include <iostream>
#include "S2.h"
#include <string>
#include <fstream>
#include <cstring>
#include <cmath>

void S2::PrintHelp()
{
	std::cout << "Command line utility for Spooky2 hardware\n"
		"Maintained by Calum Grant\n"
		"For more information see http://www.calumgrant.net/projects/s2\n"
		"\n"
		"Usage: s2 [command] [variable=value] ...\n\n"
		"Basic commands:\n"
		"  status  - Display devices\n"
		"  scan    - Start or resume scan\n"
		"  pulse   - Measure heart rate\n"
		"  run     - Run a specified program\n"
		"  set     - Store an option\n"
		"\n"
		"Basic options:\n"
		"  generator=id   - Optionally, specify a generator\n"
		"  program=...    - Specify a program file\n"
		"  spooky2=path   - Specify location of Spooky2 installation\n"
		"  user=name      - Optionally, specify a username\n"
		"\n"
		"Advanced commands:\n"
		"  control - execute a sequence of instructions\n"
		"\nAdvanced options:\n"
		"  pulse=n        - Specify pulse device (if more than one present)\n"
		"  channel=n      - Specify a channel. 0=both, 1=channel 1, 2=channel 2\n"
		"  verbose=on/off - Enable verbose mode"
		"  amplitude=nV\n"
		"  frequency=nHz\n"
		"  duration=nnn   - \n"
		"  waveform=xxx   - sine, square, triangle, saw, rsaw, hbomb\n"
		"  lock=on/off    - enable/disable user control of generator\n"
		"  output=on/off\n"
		"  step=nHz       - Step size whilst scanning\n"
		"\nExamples:\n"
		"  s2 status\n"
		"  s2 control generator=3 frequency=54kHz amplitude=5V output=on duration=1m output=off\n"
		"  s2 control @commands.txt  # Read commands from text file\n"
		"  s2 run program=\"C3 - xxx.txt\"\n"
		"  s2 scan generator=4\n";
}

S2::InvalidCommandLineArgument::InvalidCommandLineArgument(const char * option) :
	std::runtime_error("Invalid command line argument"), option(option)
{
}

S2::InvalidUnit::InvalidUnit(const char * option) : InvalidCommandLineArgument(option)
{
}

S2::Options::Options(int argc, const char *argv[])
{
	this->argc = argc;
	this->argv = argv;
	command = argv[1];
	iterations = 1;
	generator = 0;
	pulse = 0;
	channel = 0;
	simulation = false;

	frequency = std::nan("");
	amplitude = std::nan("");

	Visit(*this);
}

void S2::Options::Visit(OptionsVisitor&visitor) const
{
	// Visit the command line
	std::ifstream file1("s2.config");
	if (file1) VisitFile(file1, visitor);

	for (int i = 2; i < argc; ++i)
	{
		if (argv[i][0] == '@')
		{
			std::ifstream  file2(argv[i] + 1);
			if (file2)
				VisitFile(file2, visitor);
			else
				throw IOError("File does not exist");
		}
		visitor.OnOption(argv[i]);
	}
}

void S2::Options::VisitFile(std::istream & file, OptionsVisitor & visitor) const
{
	std::string line;
	while (std::getline(file, line))
		visitor.OnOption(line.c_str());
}

void S2::Options::SaveOptions() const
{
	std::ofstream file("s2.config");
	SaveOptions(file);
}

void S2::Options::SaveOptions(std::ostream&file) const
{
	file << "channel=" << channel << std::endl;
	file << "generator=" << generator << std::endl;
}

bool S2::OptionsVisitor::parseInt(const char * option, const char * variable, int min, int max, int & output)
{
	int len = strlen(variable);
	if (strncmp(option, variable, len) == 0 && option[len]=='=')
	{
		char *e;
		output = std::strtol(option + len + 1, &e, 10);
		if (e == option + len + 1)
			throw S2::InvalidCommandLineArgument(option);
		if (*e != 0)
			throw S2::InvalidUnit(option);
		if (output<min || output>max)
			throw ValueOutOfRange(option);
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::parseSuffix(const char * option, const char * variable, const char * suffix, double min, double max, double & output)
{
	int len = strlen(variable);
	int suffixLen = strlen(suffix);

	if(strncmp(option, variable, len)==0 && option[len]=='=')
	{
		output = atof(option + len + 1);
		char * e;
		output = std::strtod(option + len + 1, &e);

		if (e == option + len + 1)
			throw InvalidCommandLineArgument(option);

		switch (*e)
		{
		case 'k':
			output *= 1000.0;
			e++;
			break;
		case 'M':
			output *= 1000000.0;
			e++;
			break;
		case 'm':
			output *= 0.001;
			e++;
			break;
		case 'u':
			output *= 0.000001;
			e++;
			break;
		}

		if (strcmp(e, suffix) != 0)
			throw InvalidUnit(option);
		
		if (output<min || output>max)
			throw ValueOutOfRange(option);
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::parseDuration(const char * option, const char * variable, double & duration)
{
	int len = strlen(variable);
	if (strncmp(option, variable, len) == 0 && option[len] == '=')
	{
		duration = atof(option + len + 1);
		const char * end = option + strlen(option);
		if (isalpha(end[-1]))
		{
			switch (end[-1])
			{
			case 's':
				break;
			case 'm':
				duration *= 60.0;
				break;
			default:
				throw InvalidCommandLineArgument(option);
				;
			}
		}
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::parseBool(const char * option, const char * variable, bool & output)
{
	int len = strlen(variable);
	if (strncmp(option, variable, len) == 0 && option[len] == '=')
	{
		const char * value = option + len + 1;
		if (strcmp(value, "yes") == 0 || strcmp(value, "on") == 0 || strcmp(value, "1") == 0)
			output = true;
		else if (strcmp(value, "no") == 0 || strcmp(value, "off") == 0 || strcmp(value, "0") == 0)
			output = false;
		else
			throw InvalidCommandLineArgument(option);
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::parseString(const char * option, const char * variable, const char*& output)
{
	int len = strlen(variable);
	if (strncmp(option, variable, len) == 0 && option[len] == '=')
	{
		output = option + len + 1;
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::splitOption(const char * option, const char * variable, const char*&value, const char *&unit)
{
	int len = strlen(variable);
	if (strncmp(option, variable, len) == 0 && option[len] == '=')
	{
		value = option + len + 1;
		unit = value + strlen(value);
		while (isalpha(unit[-1])|| unit[-1]=='%')
			--unit;
		return true;
	}
	return false;
}

bool S2::OptionsVisitor::parsePercent(const char * option, const char * variable, double min, double max, double & output)
{
	const char * value, *unit;
	if (splitOption(option, variable, value, unit))
	{
		output = atof(value);
		switch (*unit)
		{
		case 0:
			break;
		case '%':
			output /= 100.0;
			break;
		default:
			throw InvalidCommandLineArgument(option);
		}
		if (output<min || output>max)
			throw ValueOutOfRange(option);
		return true;
	}
	return false;
}

void S2::OptionsVisitor::Waveform(const char * str)
{
	WaveData d;
	LoadFromFile(str, d);
	Waveform(d);
}

S2::ValueOutOfRange::ValueOutOfRange(const char * option) : InvalidCommandLineArgument(option)
{
}

void S2::OptionsVisitor::OnOption(const char * option)
{
	double d;
	int i;
	bool b;
	const char * str, *unit;
	if (parseInt(option, "generator", 0, 255, i))
		Generator(i);
	else if (parseInt(option, "channel", 0, 2, i))
		Channel(i);
	else if (parseSuffix(option, "frequency", "Hz", 0.0, 5e6, d))
		Frequency(d);
	else if (parseSuffix(option, "amplitude", "V", 0.0, 40.0, d))
		Amplitude(d);
	else if (parseDuration(option, "duration", d))
		Duration(d);
	else if (parseBool(option, "output", b))
		Output(b);
	else if (parseString(option, "send", str))
		Send(str);
	else if (parsePercent(option, "offset", -1.0, 1.0, d))
		Offset(d);
	else if (splitOption(option, "waveform", str, unit))
	{
		if (strcmp(str, "sine") == 0) Waveform(BuiltinWaveform::sine);
		else if (strcmp(str, "square") == 0) Waveform(BuiltinWaveform::square);
		else if (strcmp(str, "sawtooth") == 0) Waveform(BuiltinWaveform::sawtooth);
		else if (strcmp(str, "hbomb") == 0) Waveform(h_bomb);
		else Waveform(str);
	}
	else if (parsePercent(option, "duty", 0.0, 1.0, d))
		Duty(d);
	else if (parseInt(option, "phase", 0, 359, i))
		Phase(i);
	else if (parseBool(option, "sync", b))
		Sync(b);
	else if (parseInt(option, "pulse", 0, 16, i))
		Pulse(i);
	else if (parseString(option, "preset", str))
		Preset(str);
	else if (parseBool(option, "simulation", b))
		Simulation(b);
	else
		UnrecognisedOption(option);
}

void S2::Options::Waveform(BuiltinWaveform)
{
}

void S2::Options::Waveform(const WaveData&)
{
}

void S2::Options::UnrecognisedOption(const char*option)
{
	throw InvalidCommandLineArgument(option);
}

void S2::Options::Verbose(bool v)
{
	verbose = v;
}

void S2::Options::Output(bool b)
{
}

void S2::Options::Frequency(double f)
{
	frequency = f;
}

void S2::Options::Duration(double d)
{
	duration = d;
}

void S2::Options::Amplitude(double a)
{
	amplitude = a;
}

void S2::Options::Channel(int c)
{
	channel = c;
}

void S2::Options::Generator(int g)
{
	generator = g;
}

void S2::Options::Send(const char*)
{
}

void S2::Options::Lock(bool b)
{
}

void S2::Options::Phase(int p)
{
}

void S2::Options::Offset(double p)
{
}

void S2::Options::Sync(bool b)
{
}

void S2::Options::Duty(double percent)
{
}

void S2::Options::Pulse(int id)
{
	pulse = id;
}

void S2::Options::Preset(const char * filename)
{
	preset = filename;
}

void S2::Options::Simulation(bool b)
{
	simulation = b;
}
