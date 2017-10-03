#include "S2.h"
#include <unistd.h>

S2::Devices::Devices()
{
	generators.push_back(Generator(-1, ""));
	pulses.push_back(Pulse(-1, ""));
}

void S2::Sleep(double s)
{
	usleep(s*1000000);
}

std::shared_ptr<S2::Stream> S2::OpenGenerator(const std::string &)
{
	return std::make_shared<S2::DummyGenerator>(true);
}

std::shared_ptr<S2::Stream> S2::OpenPulse(const std::string &)
{
	return std::make_shared<S2::DummyPulse>();
}
