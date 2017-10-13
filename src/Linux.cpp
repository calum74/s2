#include "S2.h"

#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <errno.h>

namespace S2
{
	class HidStream : public Stream
	{
	public:
		HidStream(int);
		~HidStream();
		int Read(char * buffer, int len);
		int Write(const char * buffer, int len);
	private:
		int fd;
	};
}

S2::Devices::Devices(const Options & options)
{
	if (options.simulation)
	{
		generators.push_back(Generator(0, ""));
		pulses.push_back(Pulse(0, ""));
	}

	// Look for them now ...
	if(access("/dev/ttyUSB0", F_OK)!= -1)
		generators.push_back(Generator(1, "/dev/ttyUSB0"));
	if(access("/dev/ttyUSB1", F_OK)!=-1)
		generators.push_back(Generator(2, "/dev/ttyUSB1"));
	if(access("/dev/hidraw0", F_OK)!=-1)
		pulses.push_back(Pulse(1,"/dev/hidraw0"));
}

std::shared_ptr<S2::Stream> S2::DefaultStreamFactory::Open(Devices & devices, Pulse &pulse)
{
	if(pulse.id==0)
		return std::make_shared<DummyPulse>();

	int fd = open(pulse.filename.c_str(), O_RDONLY);

	if(fd==-1)
	{
		if(errno == EPERM)
			throw IOError("Permission denied");	// !! Exception class
		// Look at the error code...
		throw IOError("Could not open pulse device."); 
	}

	return std::make_shared<HidStream>(fd);
}

S2::HidStream::HidStream(int fd) : fd(fd)
{
}

S2::HidStream::~HidStream()
{
	close(fd);
}

int S2::HidStream::Read(char * buffer, int len)
{
	return 1 + read(fd, buffer+1, len-1);
}

int S2::HidStream::Write(const char * buffer, int len)
{
	throw std::logic_error("Writing to a HID device");
}
