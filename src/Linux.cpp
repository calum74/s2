#include "S2.h"

S2::Devices::Devices(const Options & options)
{
	if (options.simulation)
	{
		generators.push_back(Generator(0, ""));
		pulses.push_back(Pulse(0, ""));
	}

	// Look for them now ...
}

std::shared_ptr<S2::Stream> S2::DefaultStreamFactory::Open(Devices & devices, Pulse &pulse)
{
	return std::make_shared<DummyPulse>();
}