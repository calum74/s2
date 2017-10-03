#include "S2.h"
#include <cmath>
#include <iostream>

int S2::Scan(const Options & options)
{
	Devices devices;
	Pulse &pulse = devices.GetPulse(options.pulse);
	pulse.Open();

	double previousValue = pulse.BPM();
	for (;;)
	{
		double currentValue = pulse.BPM();
		double delta = currentValue - previousValue, a = std::abs(delta);
		char result;
		double t0 = 4.0, t1 = 10.0;

		if (a < t0)
			result = '-';
		else if (delta > 0 && delta < t1)
			result = '<';
		else if (delta<0 && delta>-t1)
			result = '>';
		else
			result = 'X';

		previousValue = currentValue;

		std::cout << result << std::flush;
	}

	return 0;

}