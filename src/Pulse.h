#pragma once

namespace S2
{
	class Devices;

	class Pulse : public Device
	{
	public:
		Pulse(int id, const std::string &filename);
		// Throws: IOError if device disconnected or fails.
		// This is a blocking call.
		double BPM();
		void Open(Devices&, StreamFactory&sf);
	};

	class DummyPulse : public Stream
	{
	public:
		virtual int Read(char * buffer, int size);
		virtual int Write(const char * buffer, int size);
		virtual double BPM();
	};

}
