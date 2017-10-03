#pragma once

#include <vector>

namespace S2
{
	class Devices
	{
	public:
		// Enumerators all devices but does not try to open them.
		Devices(const Options & options);

		std::vector<Pulse> pulses;
		std::vector<Generator> generators;

		// Throws Device not FOund
		Pulse & GetPulse(int id);
		Generator & GetGenerator(int id);
	};
}

