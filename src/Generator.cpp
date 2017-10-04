
#include "S2.h"
#include <iostream>
#include <sstream>
#include <cstring>

enum Commands
{
	set_waveform_1 = 21,
	set_waveform_2 = 22,
	set_frequency_1 = 23,	// e.g. :w2336456000 = 364.450000 Hz
	set_frequency_2 = 24,	// e.g. :w2336456000 = 364.450000 Hz
	set_voltage_1 = 25,  // e.g. :w251005  Set channel 1 to 10.05 V
	set_voltage_2 = 26,  // e.g. :w251005  Set channel 1 to 10.05 V
	set_offset_1 = 27,
	set_offset_2 = 28,
	set_duty_1 = 29,	// e.g. :w29500 = 50%
	set_duty_2 = 30,
	set_phase_1 = 31,
	set_phase_2 = 32,	// e.g. :w32180 = 180degrees
	set_relay_1 = 61,		// e.g. :w621	Set output 2 on
	set_relay_2 = 62,
	set_freq_scale_1 = 63,
	set_freq_scale_2 = 64,
	set_sync = 68	// Channel 2 copies freq from Channel 1 e.g. :w681 set sync on
};

S2::Generator::Generator(int id, const char * filename) :
	Device(id, filename),
	channel0(*this),
	channel1(*this, 0),
	channel2(*this,1),
	sync(Unknown)
{
	verbose = false;
	frequencyScaling[0] = frequencyScaling[1] = FrequencyScale::Unknown;
}

S2::Generator::Generator(const Generator & src) :
	Device(src),
	channel0(*this),
	channel1(*this, 0),
	channel2(*this, 1),
	sync(Unknown)
{
	verbose = false;
	frequencyScaling[0] = frequencyScaling[1] = FrequencyScale::Unknown;
}

void S2::Generator::Send(int command, int channel, int value)
{
	if (channel < 0 || channel>1)
		throw std::logic_error("Invalid channel to send command");
	char buffer[20];
	snprintf(buffer, 20, ":w%02d%d\r\n", command+channel, value);	// ?? \r
	Send(buffer);
}

void S2::Generator::Amplitude(int channel, double value)
{
	Send(set_voltage_1, channel, int(value * 100));
	return;

	char buffer[20];
	//!! Abstract snprintf
	snprintf(buffer, 20, ":w%02d%04d\r\n", set_voltage_1+channel, int(value * 100));
	Send(buffer);
}

void S2::Generator::Output(int channel, bool value)
{
	Send(set_relay_1, channel, value);
	return;

	char buffer[20];
	snprintf(buffer, 20, ":w%02d%1d\r\n", set_relay_1+channel, int(value));
	Send(buffer);
}

void S2::Generator::Frequency(int channel, double value)
{
	if (value < 600.0 && frequencyScaling[channel] != FrequencyScale::Low)
	{
		FrequencyScaling(channel, FrequencyScale::Low);
	}
	else if (frequencyScaling[channel] != FrequencyScale::High)
	{
		FrequencyScaling(channel, FrequencyScale::High);
	}
	Send(set_frequency_1, channel, frequencyScaling[channel]==FrequencyScale::High ? int(value*100) : int(value*100000));
}

void S2::Generator::FrequencyScaling(int channel, FrequencyScale fs)
{
	Send(set_freq_scale_1, channel, fs==FrequencyScale::High ? 0 : 1);
	frequencyScaling[channel] = fs;
}

void S2::Generator::Offset(int channel, double p)
{
	Send(set_offset_1, channel, 100 + int(p*100.0));
}

void S2::Generator::Waveform(int channel, BuiltinWaveform f)
{
	Send(set_waveform_1, channel, f);
}

void S2::Generator::Waveform(int channel, const WaveData & data)
{
	std::stringstream ss;
	ss << ":a0" << (1+channel) << data[0];
	for (int i = 1; i < 1024; ++i)
		ss << "," << data[i];
	ss << "\n";  // ?? \r
	Send(ss.str().c_str());

	Send(set_waveform_1, channel, 101+channel);
}

void S2::Generator::Phase(int channel, int phase)
{
	Send(set_phase_1, channel, phase);
}

void S2::Generator::Duty(int channel, double duty)
{
	Send(set_duty_1, channel, int(1000.0*duty));
}

void S2::Generator::Sync(bool sync)
{
	Send(set_sync, 0, (int)sync);
}

void S2::Generator::Send(const char * buffer)
{
	// verbose = true;
	if(verbose)
		std::cout << buffer;
	int len = strlen(buffer) + 1;
	int written = stream->Write(buffer, len);
	char returnBuffer[20];
	int bytes; //  = Read(returnBuffer, 4);
	do
	{
		Sleep(0.01);	// !! Should not be needed any more
		bytes = stream->Read(returnBuffer, 4);
	} while (bytes == 0);
	returnBuffer[4] = 0;
	if (bytes != 4 || returnBuffer[0] != 'o' || returnBuffer[1] != 'k')
	{
		throw IOError("Unexpected response from the generator");
	}
	if(verbose)
		std::cout << returnBuffer;

}

void S2::Generator::Open()
{
	stream = filename.empty() ? std::make_shared<DummyGenerator>(true) : OpenGenerator(filename);
}

S2::Stream::~Stream()
{
}

S2::DummyGenerator::DummyGenerator(bool v) : verbose(v)
{
}

int S2::DummyGenerator::Read(char * buffer, int size)
{
	if (size >= 4)
	{
		buffer[0] = 'o';
		buffer[1] = 'k';
		buffer[2] = '\r';
		buffer[3] = '\n';
	}
	return 4;
}

int S2::DummyGenerator::Write(const char * buffer, int size)
{
	if (verbose)
		std::cout << buffer;
	return size;
}

S2::Channel & S2::Generator::GetChannel(int channel)
{
	switch (channel)
	{
	case 0:
		if (sync != On)
		{
			Sync(true);
			Phase(1, 180);
			sync = On;
		}
		return channel0;
	case 1:
		if (sync != Off)
		{
			Sync(false);
			sync = Off;
		}
		return channel1;
	case 2:
		if (sync != Off)
		{
			Sync(false);
			sync = Off;
		}
		return channel2;
	default:
		throw std::logic_error("Invalid channel specified");
	}
}

S2::SingleChannel::SingleChannel(Generator &g, int id) :
	generator(g), channel(id)
{
}

void S2::SingleChannel::Amplitude(double value)
{
	generator.Amplitude(channel, value);
}

void S2::SingleChannel::Frequency(double value)
{
	generator.Frequency(channel, value);
}

void S2::SingleChannel::Waveform(BuiltinWaveform value)
{
	generator.Waveform(channel, value);
}

void S2::SingleChannel::Waveform(const S2::WaveData & data)
{
	generator.Waveform(channel, data);
}

void S2::SingleChannel::Duty(double p)
{
	generator.Duty(channel, p);
}

void S2::SingleChannel::Output(bool b)
{
	generator.Output(channel, b);
}

void S2::SingleChannel::Offset(double p)
{
	generator.Offset(channel, p);
}

S2::InvertAndSync::InvertAndSync(Generator &g) :
	generator(g)
{
}

void S2::InvertAndSync::Amplitude(double value)
{
	generator.Amplitude(0, value / 2.0);
	generator.Amplitude(1, value / 2.0);
}

void S2::InvertAndSync::Frequency(double value)
{
	generator.Frequency(0, value);
}

void S2::InvertAndSync::Waveform(BuiltinWaveform value)
{
	generator.Waveform(0, value);
	generator.Waveform(1, value);
}

void S2::InvertAndSync::Waveform(const S2::WaveData & data)
{
	generator.Waveform(0, data);
	WaveData inverse;
	for (int i = 0; i < 1024; ++i)
		inverse[i] = 1023 - data[i];
	generator.Waveform(1, inverse);
}

void S2::InvertAndSync::Duty(double p)
{
	generator.Duty(0, p);
	generator.Duty(1, 1.0 - p);
}

void S2::InvertAndSync::Output(bool b)
{
	generator.Output(0, b);
	generator.Output(1, b);
}

void S2::InvertAndSync::Offset(double p)
{
	generator.Offset(0, p);
	generator.Output(1, 1.0-p);
}
