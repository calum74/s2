#include "S2.h"
#include <iostream>

// !! Move to header file
class Controller : public S2::OptionsVisitor
{
	S2::ProgressMonitor &pm;
	S2::StreamFactory & sf;
public:
	Controller(const S2::Options &o, S2::ProgressMonitor &pm, S2::StreamFactory &sf) :
		devices(o), pm(pm), sf(sf)
	{
		generator = o.generator;
		channel = o.channel;
		devices.GetGenerator(generator).Open(devices, sf);
	}

	void Verbose(bool b)
	{
	}

	void Duration(double d)
	{
		std::cout << "Pausing for " << d << " seconds\n";
		S2::Sleep(d);
	}

	void UnrecognisedOption(const char*)
	{
	}

	void Generator(int g)
	{
		generator = g;
	}

	void Pulse(int id)
	{
	}

	void Channel(int c)
	{
		channel = c;
	}

	S2::Channel &GetChannel()
	{
		return devices.GetGenerator(generator).GetChannel(channel);
	}

	void Waveform(S2::BuiltinWaveform f)
	{
		const char * wavetext[] = { "sine", "square", "sawtooth", "hbomb" };
		std::cout << "Setting generator " << generator << " channel " << channel << " to " << wavetext[(int)f] << "\n";
		GetChannel().Waveform(f);
	}

	void Waveform(const S2::WaveData &data)
	{
		std::cout << "Setting waveform data\n";
		GetChannel().Waveform(data);
	}

	void Amplitude(double a)
	{
		std::cout << "Setting generator " << generator << " channel " << channel << " to " << a << "V\n";
		GetChannel().Amplitude(a);
	}

	void Frequency(double f)
	{
		std::cout << "Setting generator " << generator << " channel " << channel << " to " << f << "Hz\n";
		GetChannel().Frequency(f);
	}

	void Output(bool b)
	{
		std::cout << "Setting generator " << generator << " channel " << channel << " to " << (b?"on":"off") << "\n";
		GetChannel().Output(b);
	}

	void Send(const char * cmd)
	{
		auto &g = devices.GetGenerator(generator);
		char buffer[50];
		snprintf(buffer, sizeof(buffer), "%s\n", cmd);
		std::cout << "Sending generator " << g.id << " " << cmd << "\n";
		g.Send(buffer);
	}

	void Lock(bool b)
	{
		auto &g = devices.GetGenerator(generator);
		std::cout << "Setting generator " << g.id << " channel " << channel << " lock " << (b ? "on" : "off") << std::endl;
	}

	void Offset(double p)
	{
		std::cout << "Setting generator " << generator << " channel " << channel << " offset " << p << std::endl;
		GetChannel().Offset(p);
	}

	void Phase(int degrees)
	{
		// !!
	}

	virtual void Sync(bool on)
	{
		// !!
	}

	void Simulation(bool) { }

	void Duty(double duty)
	{
		std::cout << "Setting generator " << generator << " channel " << channel << " duty " << duty << "\n";
		GetChannel().Duty(duty);
	}

	void Preset(const char * filename)
	{
	}

private:
	int generator, channel;
	S2::Devices devices;
};

int S2::Control(const Options & options, ProgressMonitor &pm, StreamFactory &sf)
{
	try
	{
		Controller controller(options, pm, sf);
		options.Visit(controller);
		return 0;
	}
	catch (const DeviceNotFound &)
	{
		std::cout << "Could not open generator " << options.generator << std::endl;
		return 1;
	}
	catch (const IOError &e)
	{
		std::cout << "Error writing to generator: " << e.what() << std::endl;
		return 1;
	}
}
