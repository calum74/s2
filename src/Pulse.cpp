
#include "S2.h"
#include <algorithm>

S2::Pulse::Pulse(int id, const std::string & filename) : Device(id, filename)
{
}

double S2::Pulse::BPM()
{
	unsigned char buffer[65];
	if (stream->Read((char*)buffer, sizeof(buffer)) == 65)
	{
		unsigned value = (buffer[3] << 16) | (buffer[4] << 8) | buffer[5];
		return 60.0 / (double(value) / 250000.0);
	}
	else
	{
		throw IOError("Failed to read pulse");
	}
}

void S2::Pulse::Open(Devices&devices, StreamFactory & sf)
{
	stream = sf.Open(devices, *this);
}

int S2::DummyPulse::Read(char * buffer, int size)
{
	if (size > 5)
	{
		unsigned value = unsigned((60.0 / BPM()) * 250000.0);
		buffer[3] = (unsigned char)(value >> 16);
		buffer[4] = (unsigned char)(value >> 8);
		buffer[5] = (unsigned char)value;
	}
	S2::Sleep(0.1);
	return std::min(size, 65);
}

int S2::DummyPulse::Write(const char * buffer, int size)
{
	throw IOError("Do not write to the pulse");
}

double S2::DummyPulse::BPM()
{
	return 72.0;
}
