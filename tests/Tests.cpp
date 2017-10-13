
#include "Tests.h"

#include <fstream>
#include <string.h>

TestStreamFactory::TestStreamFactory() : pulse(std::make_shared<TestPulse>()), generator(std::make_shared<TestGenerator>())
{
}

std::shared_ptr<S2::Stream> TestStreamFactory::Open(S2::Devices & d, S2::Pulse &p)
{
	return pulse;
}

std::shared_ptr<S2::Stream> TestStreamFactory::Open(S2::Devices & d, S2::Generator &p)
{
	return generator;
}

int TestGenerator::Read(char * buffer, int size)
{
	assert(size >= 4);
	strncpy(buffer, "ok\r\n", 4);
	return 4;
}

int TestGenerator::Write(const char * buffer, int size)
{
	input.write(buffer, size);
	return size;
}

int TestPulse::Read(char * buffer, int size)
{
	return 0;
}

int TestPulse::Write(const char * buffer, int size)
{
	return 0;
}

// class

class GeneratorTests
{
	const char **args;
	S2::Options options;
	S2::Devices devices;
	TestStreamFactory sf;
	GeneratorTests();
};

void testGenerator()
{
	const char * args[] = { "s2", "control" };
	S2::Options options(2, args);
	S2::Devices devices(options);
	TestStreamFactory sf;
	S2::Generator g(1,"");
	g.Open(devices, sf);
}

void testPulse()
{
	const char * args[] = { "s2", "control" };
	S2::Options options(2, args);
	S2::Devices devices(options);
	TestStreamFactory sf;

	S2::Pulse p(1,"");
	p.Open(devices, sf);

	assert_throws<S2::IOError>([&] { p.BPM(); });
}

void testControlCommand()
{
	const char * args[] = { "s2", "control", "simulation=on", "amplitude=5V" };
	S2::Options options(4, args);
	std::stringstream output;
	S2::DefaultProgressMonitor pm(output);
	TestStreamFactory sf;
	S2::Control(options, pm, sf);
	assert(sf.generator->input.str() == ":w681\r\n:w32180\r\n:w25250\r\n:w26250\r\n");
}

void testDatabase()
{
	S2::Database db;
	std::ifstream file("C:\\Users\\calum\\Desktop\\decoded.csv");

	if (file)
	{
		db.Read(file);
	}
	// db.Read(
}

void testProgram()
{
	S2::Program p("1000=10,10=20");
	assert(p.Duration() == 30);
	S2::OutputState state;
	p.GetState(-1.0, state);
	assert(!state.output);

	p.GetState(0.0, state);
	assert(state.output);
	assert(state.frequencyHz == 1000);
	assert(state.stepDuration == 10);

	p.GetState(10.0, state);
	assert(state.output);
	assert(state.frequencyHz == 10);
	assert(state.stepDuration == 20);

	p.GetState(30.0, state);
	assert(!state.output);
}

void testPreset()
{
	std::stringstream ss;
	ss << "\"List2=Stage 1\"" << std::endl;
	ss << "\"List2=Stage 2\"" << std::endl;
	ss << "\"List4=2000=20,1000=10\"" << std::endl;
	ss << "\"List4=5000=80\"" << std::endl;

	// std::ifstream file("C:\\Spooky2\\Preset Collections\\Morgellons and Lyme\\Morgellons and Lyme v3.0\\Contact\\C04 Blood and Lymph - DB.txt");
	S2::Preset p(ss);

	assert(p.Duration() == 110);
	S2::OutputState state;
	p.GetState(0.0, state);
	assert(state.output);
}

void testRunnable()
{
	// Run multiple generators?
	/*
		RunSequence sequence;

		Runnable is awkward

		Pool p;
		p.add(runnable, channel1, 0.0);
		p.add(program, channel2, 5.0);

	*/



}

int main()
{
	testOptions();
	testGenerator();
	testPulse();
	testControlCommand();
	testProgram();
	// testDatabase();
	testPreset();
	std::cout << "Tests passed\n";
	return 0;
}
