#pragma once

namespace S2
{
	class Devices;
	std::shared_ptr<Stream> OpenPulse(Devices&devices, const std::string &filename);

	class Pulse : public Device
	{
	public:
		Pulse(int id, const std::string &filename);
		// Throws: IOError if device disconnected or fails.
		// This is a blocking call.
		double BPM();
		void Open(Devices&);
	};

	class DummyPulse : public Stream
	{
	public:
		virtual int Read(char * buffer, int size);
		virtual int Write(const char * buffer, int size);
		virtual double BPM();
	};

}
