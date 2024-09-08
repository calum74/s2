
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
        S2::DefaultProgressMonitor pm(S2::Quiet, output);
        TestStreamFactory sf;
        S2::Control(options, pm, sf);
        assert(sf.generator->input.str() ==
               ":w681\r\n:w32180\r\n:w25250\r\n:w26250\r\n");
}

void testProgram()
{
  S2::Options options;
  S2::Program p(options, "1000=10,10=20");
  assert(p.Duration() == 30);
  S2::ChannelState state;
  p.GetState(-1.0, state);
  assert(!state.output);

  p.GetState(0.0, state);
  assert(state.output);
  assert(state.frequency == 1000);
  assert(state.stepDuration == 10);

  p.GetState(10.0, state);
  assert(state.output);
  assert(state.frequency == 10);
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
        S2::Options options;
        S2::Preset p(options, ss);

        assert(p.Duration() == 110);
        S2::ChannelState state;
        p.GetState(0.0, state);
        assert(state.output);

        p.GetState(20, state);
        assert(state.frequency == 1000.0);
        p.GetState(30, state);
        assert(state.frequency == 5000.0);
}

void checkSequence(S2::MultiChannelSequence & seq, double stepSize, bool loop, bool success,
				   int generator, int channel, double timestamp, bool output, double frequency)
{
	S2::ChannelState state;
	assert(seq.Next(state, stepSize,loop) == success);

	if(success)
	{
		assert(state.channelId == S2::ChannelId(generator, channel));
		assert(output == state.output);
                assert(frequency == state.frequency);
        }
}

void testMultiChannelSequence()
{
  S2::Options options;
  S2::MultiChannelSequence sequence;
  sequence.Add(S2::ChannelId(0, 0),
               std::make_shared<S2::Program>(options, "1000,2000"));
  sequence.Add(
      S2::ChannelId(1, 0),
      std::make_shared<S2::Program>(options, "3000=20,4000=280,1000-2000=5"));

  sequence.Begin();
  checkSequence(sequence, 1.0, true, true, 0, 0, 0, true, 1000);
  checkSequence(sequence, 1.0, true, true, 1, 0, 0, true, 3000);
  checkSequence(sequence, 1.0, true, true, 1, 0, 20, true, 4000);
  checkSequence(sequence, 1.0, true, true, 0, 0, 180, true, 2000);
  checkSequence(sequence, 1.0, true, true, 1, 0, 300, true, 1000);
  checkSequence(sequence, 1.0, true, true, 1, 0, 301, true, 1200);
  checkSequence(sequence, 1.0, true, true, 1, 0, 302, true, 1400);
  checkSequence(sequence, 1.0, true, true, 1, 0, 303, true, 1600);
  checkSequence(sequence, 1.0, true, true, 1, 0, 304, true, 1800);
  checkSequence(sequence, 1.0, true, true, 1, 0, 305, true, 3000);
  checkSequence(sequence, 1.0, true, true, 1, 0, 325, true, 4000);
  checkSequence(sequence, 1.0, true, true, 0, 0, 360, true, 1000);

  sequence.Begin();
  checkSequence(sequence, 1.0, false, true, 0, 0, 0, true, 1000);
  checkSequence(sequence, 1.0, false, true, 1, 0, 0, true, 3000);
  checkSequence(sequence, 1.0, false, true, 1, 0, 20, true, 4000);
  checkSequence(sequence, 1.0, false, true, 0, 0, 180, false, 0);

  checkSequence(sequence, 1.0, false, true, 1, 0, 300, true, 1000);
  checkSequence(sequence, 1.0, false, true, 1, 0, 301, true, 1200);
  checkSequence(sequence, 1.0, false, true, 1, 0, 302, true, 1400);
  checkSequence(sequence, 1.0, false, true, 1, 0, 303, true, 1600);
  checkSequence(sequence, 1.0, false, true, 1, 0, 304, false, 0);

  S2::ChannelState state;
  while (sequence.Next(state, 1.0, false)) {
    std::cout << state.channelId.first << "." << state.channelId.second << ": "
              << state.time << ": " << state.frequency
              << "Hz, output=" << state.output << "\n";
  }

  // Now test sequencing
  S2::MultiChannelSequence seq2;
  seq2.Add(S2::ChannelId(0, 0),
           std::make_shared<S2::Program>(options, "1000,2000"));
  seq2.Add(S2::ChannelId(0, 0), std::make_shared<S2::Program>(options, "3000"));
  seq2.Begin();
  assert(seq2.Duration() == 3 * 180);
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
	testMultiChannelSequence();
	std::cout << "Tests passed\n";
	return 0;
}
