
#include "Tests.h"
#include <cassert>
#include <functional>


void testOptions()
{
	{
		const char * args[] = { "s2", "pulse" };
		S2::Options options(2, args);
		assert(std::string(options.command) == "pulse");
	}

	{
		const char * args[] = { "s2", "control", "amplitude=4V" };
		S2::Options options(3, args);
		assert(options.amplitude == 4.0);
	}

	{
		const char * args[] = { "s2", "control", "amplitude=40mV" };
		S2::Options options(3, args);
		assert(options.amplitude == 0.04);
	}

	{
		const char * args[] = { "s2", "control", "amplitude=40mV" };
		S2::Options options(3, args);
		assert(options.amplitude == 0.04);
	}

	assert_throws<S2::InvalidUnit>([] {
		const char * args[] = { "s2", "control", "amplitude=10" };
		S2::Options options(3, args); });

	assert_throws<S2::InvalidUnit>([] {
		const char * args[] = { "s2", "control", "amplitude=10v" };
		S2::Options options(3, args); });

	assert_throws<S2::ValueOutOfRange>([] {
		const char * args[] = { "s2", "control", "amplitude=500V" };
		S2::Options options(3, args); });

	assert_throws<S2::ValueOutOfRange>([] {
		const char * args[] = { "s2", "control", "amplitude=41V" };
		S2::Options options(3, args); });

	assert_throws<S2::InvalidCommandLineArgument>([] {
		const char * args[] = { "s2", "control", "amplitude=xV" };
		S2::Options options(3, args); });

	assert_throws<S2::InvalidCommandLineArgument>([] {
		const char * args[] = { "s2", "control", "amplitude=kV" };
		S2::Options options(3, args); });

	assert_throws<S2::InvalidCommandLineArgument>([] {
		const char * args[] = { "s2", "control", "amplitude=" };
		S2::Options options(3, args); });

	{
		const char * args[] = { "s2", "control", "frequency=1Hz" };
		S2::Options options(3, args);
		assert(options.frequency == 1.0);
	}

	{
		const char * args[] = { "s2", "control", "frequency=1kHz" };
		S2::Options options(3, args);
		assert(options.frequency == 1000.0);
	}

	{
		const char * args[] = { "s2", "control", "frequency=1MHz" };
		S2::Options options(3, args);
		assert(options.frequency == 1000000.0);
	}

	{
		const char * args[] = { "s2", "control", "frequency=1mHz" };
		S2::Options options(3, args);
		assert(options.frequency == 0.001);
	}

	{
		const char * args[] = { "s2", "control", "frequency=0Hz" };
		S2::Options options(3, args);
		assert(options.frequency == 0.0);
	}

	{
		const char * args[] = { "s2", "control", "frequency=1uHz" };
		S2::Options options(3, args);
		assert(options.frequency == 0.000001);
	}

	assert_throws<S2::InvalidUnit>([]
	{
		const char * args[] = { "s2", "control", "frequency=1" };
		S2::Options options(3, args);
	});

	{
		const char * args[] = { "s2", "control", "channel=1" };
		S2::Options options(2, args);
		assert(options.channel == 0);
	}
	{
		const char * args[] = { "s2", "control", "channel=1" };
		S2::Options options(3, args);
		assert(options.channel == 1);
	}

	assert_throws<S2::ValueOutOfRange>([]
	{
		const char * args[] = { "s2", "control", "channel=3" };
		S2::Options options(3, args);
	});

	assert_throws<S2::ValueOutOfRange>([]
	{
		const char * args[] = { "s2", "control", "channel=-1" };
		S2::Options options(3, args);
	});

	assert_throws<S2::InvalidUnit>([]
	{
		const char * args[] = { "s2", "control", "channel=0x" };
		S2::Options options(3, args);
	});

	assert_throws<S2::InvalidCommandLineArgument>([]
	{
		const char * args[] = { "s2", "control", "channel=" };
		S2::Options options(3, args);
	});

	assert_throws<S2::InvalidCommandLineArgument>([]
	{
		const char * args[] = { "s2", "control", "channel=x" };
		S2::Options options(3, args);
	});

	{
		const char * args[] = { "s2", "control", "generator=3" };
		S2::Options options(3, args);
		assert(options.generator == 3);
	}

	assert_throws<S2::ValueOutOfRange>([]
	{
		const char * args[] = { "s2", "control", "generator=-1" };
		S2::Options options(3, args);
	});

	assert_throws<S2::InvalidUnit>([]
	{
		const char * args[] = { "s2", "control", "generator=1d" };
		S2::Options options(3, args);
	});

	{
		const char * args[] = { "s2", "control", "generator=3" };
		S2::Options options(3, args);
		assert(options.generator == 3);
	}


	// !! Waveform tests
}
