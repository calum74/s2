#include "S2.h"
#include <cmath>
#include <iostream>

/*
Overall algorithm:
s2.config to store scanprogress once every 10s.
scanresults.csv
scanlog.csv

1. See whether to start a new scan from s2.config
2. Determine step size, limits, and sensitivity.
3. Scan forward, with automatic rescan of highlights.
4. Do not display retrys. -<X>! only.
5. Once a hit has been found, display the result

Variables that affect this:

generator
channel
voltage
waveform

step
minfrequency
maxfrequency
validation=3
steady
spike

*/

int S2::Scan(const Options & options)
{
	Devices devices(options);
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