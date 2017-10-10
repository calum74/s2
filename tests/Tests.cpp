
#include "S2.h"
#include <cassert>

void testOptions();

class TestGenerator : public S2::Stream
{
	int Read(char * buffer, int size);
	int Write(const char * buffer, int size);
};

class TestProgress : S2::ProgressMonitor
{
};

int TestGenerator::Read(char * buffer, int size)
{

}

int TestGenerator::Write(const char * buffer, int size)
{

}

// class

void testGenerator()
{
	auto s = std::make_shared<TestGenerator>();

	S2::Generator g(1,"");
	// g.Open(sf);
	// g.SetStream(s);
	// Test communication errors
}

void testPulse()
{
	S2::Pulse p(1,"");
	auto s = std::make_shared<TestGenerator>();
	// p.SetStream(s);
}

void testControlCommand()
{
	const char * args[] = { "s2", "control", "amplitude=5V" };
	S2::Options options(3, args);

}

int main()
{
	testOptions();
	testGenerator();
	testPulse();
	std::cout << "Tests passed\n";
	return 0;
}
