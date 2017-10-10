#pragma once
#include <stdexcept>
#include <iostream>
#include <string>

#include "Waveforms.h"
#include "Options.h"
#include "Device.h"
#include "Pulse.h"
#include "Generator.h"
#include "Devices.h"
#include "Program.h"
#include "Preset.h"
#include "Database.h"

namespace S2
{
	class IOError : public std::runtime_error
	{
	public:
		IOError(const char * message);
	};
		
	class DeviceNotFound : IOError
	{
	public:
		DeviceNotFound();
	};

	int PulseCommand(const Options&);
	int StatusCommand(const Options&);
	int Main(int argc, const char*argv[]);
	void PrintHelp();
	void Diagnose();
	int Scan(const Options & options);
	int Control(const Options & options);
	void Sleep(double seconds);
	int Send(const char * sendString);
	int Set(const Options & options);
	int Run(const Options & options);
}
