#pragma once

namespace S2
{
	std::shared_ptr<Stream> OpenPulse(const std::string &filename);

	class Pulse : public Device
	{
	public:
	Pulse(int id, const char * filename);
	// Throws: IOError if device disconnected or fails.
	// This is a blocking call.
	double BPM();
	void Open();
	};

	class DummyPulse : public Stream
	{
	public:
		virtual int Read(char * buffer, int size);
		virtual int Write(const char * buffer, int size);
		virtual double BPM();
	};

}