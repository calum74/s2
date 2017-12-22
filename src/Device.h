#pragma once

#include <memory>

namespace S2
{
	class Devices;
	class Pulse;
	class Generator;

	extern const char * simulation;

	enum class DeviceStatus
	{
		Available,
		InUse,
		Error
	};

	class Stream
	{
	public:
		virtual int Read(char * buffer, int size) = 0;
		virtual int Write(const char * buffer, int size) = 0;
		virtual ~Stream();
	};

	class StreamFactory
	{
	public:
		virtual std::shared_ptr<Stream> Open(Devices & d, Pulse &p)=0;
		virtual std::shared_ptr<Stream> Open(Devices &d, Generator &g)=0;
	};

	// Stream factory producing hardware streams.
	class DefaultStreamFactory : public StreamFactory
	{
	public:
		std::shared_ptr<Stream> Open(Devices & d, Pulse &p);
		std::shared_ptr<Stream> Open(Devices &d, Generator &g);
	};

	class Device
	{
	public:
		int id;
		std::string filename;

		Device();
		Device(int id, const std::string&);
		~Device();

		bool Simulation() const;
	protected:
		std::shared_ptr<Stream> stream;
	};
}
