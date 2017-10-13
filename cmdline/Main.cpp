// S2.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include "S2.h"
#include <string>
#include <cstring>


int S2::StatusCommand(const Options&options, ProgressMonitor &pm, StreamFactory &sf)
{
	Devices devices(options);

	if (devices.pulses.size()==0)
		std::cout << "Pulse not connected\n";
	else
		std::cout << devices.pulses.size() << " pulse connected:\n";
	for(const auto & device : devices.pulses)
		// if(device.filename!="")
		std::cout << "  " << device.id << ": " << device.filename << std::endl;

	if (devices.generators.size()==0)
	{
		std::cout << "No generators connected\n";

	}
	else
		std::cout << devices.generators.size() << " generators connected:\n";
	for (const auto & g : devices.generators)
		//if(g.id!=-1)
		std::cout << "  " << g.id << ": " << g.filename << std::endl;
	return 0;
}

int S2::PulseCommand(const Options & options, ProgressMonitor &pm, StreamFactory &sf)
{
// options.iterations=0;
	bool loop = false; // options.iterations == 0; //argc > 0 && strcmp(argv[0], "-l") == 0;
	if (loop)
	{
		std::cout << "Reading pulse. Press Ctrl+C to stop.\n";
	}
	try
	{
		Devices devices(options);
		Pulse &pulse = devices.GetPulse(options.pulse);
		pulse.Open(devices, sf);

		for(int i=0; options.iterations==0 || i<options.iterations; ++i)
		{
			std::cout << pulse.BPM() << " bpm\n";
		}
		return 0;
	}
	catch (const DeviceNotFound&)
	{
		std::cout << "Pulse not found. Ensure that the device is connected.\n";
		return 1;
	}
}

int main(int argc, const char *argv[])
{
	return S2::Main(argc, argv);
}

void S2::Diagnose()
{
}

int S2::Main(int argc, const char *argv[])
{
	if (argc < 2)
	{
		PrintHelp();
		return 0;
	}

	try
	{
		Options options(argc, argv);
		std::string command = options.command;
		DefaultStreamFactory sf;
		DefaultProgressMonitor pm(std::cout);

		if (command=="status")
			return StatusCommand(options, pm, sf);
		else if (command=="pulse")
			return PulseCommand(options, pm, sf);
		else if (command=="diagnose")
			Diagnose();
		else if (command== "scan")
			return Scan(options, pm, sf);
		else if (command== "control")
			return Control(options, pm, sf);
		else if (command== "help")
			PrintHelp();
		else if (command== "set")
			Set(options);
		else if (command=="run")
			Run(options);
		else
			throw InvalidCommandLineArgument(options.command);

		return 0;
	}
	catch (ValueOutOfRange & ex)
	{
		std::cout << "The supplied argument is out of range: " << ex.option << std::endl;
		return 1;
	}
	catch (const InvalidCommandLineArgument & ex)
	{
		std::cout << "Unrecognized option: " << ex.option << std::endl;
		return 1;
	}
	std::cout << "Run \"s2 help\" to display help\n";
	return 1;
}

