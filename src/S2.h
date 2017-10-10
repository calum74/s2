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
#include "ProgressMonitor.h"

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

	int PulseCommand(const Options&, ProgressMonitor &pm, StreamFactory &sf);
	int StatusCommand(const Options&, ProgressMonitor &pm, StreamFactory &sf);
	int Main(int argc, const char*argv[]);
	void PrintHelp();
	void Diagnose();
	int Scan(const Options & options, ProgressMonitor &pm, StreamFactory &sf);
	int Control(const Options & options, ProgressMonitor &pm, StreamFactory &sf);
	void Sleep(double seconds);
	int Send(const char * sendString);
	int Set(const Options & options);
	int Run(const Options & options);
}
