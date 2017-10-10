#include "S2.h"

S2::Device::~Device()
{
}


S2::Device::Device(int id, const std::string& filename) : id(id), filename(filename)
{
}

S2::Pulse & S2::Devices::GetPulse(int id)
{
	if (id == 0 && !pulses.empty())
		return pulses.front();
	for (auto & p : pulses)
		if (p.id == id) return p;
	throw DeviceNotFound();
}


S2::Generator & S2::Devices::GetGenerator(int id)
{
	if (id == 0 && !generators.empty())
		return generators.front();
	for (auto & g : generators)
		if (g.id == id) return g;
	throw DeviceNotFound();
}

S2::IOError::IOError(const char * message) : std::runtime_error(message)
{
}

S2::DeviceNotFound::DeviceNotFound() : IOError("Device not found")
{
}

S2::Device::Device()
{
}
