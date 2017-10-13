#include "S2.h"

#ifdef NDEBUG
#undef NDEBUG
#endif

#include <cassert>
#include <functional>
#include <sstream>


void testOptions();

template<typename T>
void assert_throws(std::function<void()> t)
{
	try
	{
		t();
		assert(!"Failed to throw exception");
	}
	catch (T&)
	{
	}
	catch (std::exception&)
	{
		assert(!"Threw wrong exception");
	}
}

template<typename T>
void assert_nothrow(std::function<void()> t)
{
	try
	{
		t();
	}
	catch(...)
	{
		assert(!"Test threw an exception");
	}
}

class TestGenerator : public S2::Stream
{
public:
	int Read(char * buffer, int size);
	int Write(const char * buffer, int size);

	std::stringstream input;
	// S2::GeneratorStatus channel1, channel2;
};

class TestPulse : public S2::Stream
{
public:
	int Read(char * buffer, int size);
	int Write(const char * buffer, int size);
	double frequency;
};

class TestStreamFactory : public S2::StreamFactory
{
public:
	TestStreamFactory();
	std::shared_ptr<TestPulse> pulse;
	std::shared_ptr<TestGenerator> generator;
	std::shared_ptr<S2::Stream> Open(S2::Devices & d, S2::Pulse &p);
	std::shared_ptr<S2::Stream> Open(S2::Devices &d, S2::Generator &g);
};
