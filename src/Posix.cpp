#include "S2.h"
#include <unistd.h>
#include <fcntl.h>
#include <errno.h>
#include <termios.h>
#include <string.h>
#include <dirent.h>

namespace S2
{
	class PosixStream : public Stream
	{
	public:
		PosixStream(int fd);
		~PosixStream();

		int fd;
		int Read(char * data, int size);
		int Write(const char * data, int size);
	};
}

S2::PosixStream::PosixStream(int fd) : fd(fd)
{
}

S2::PosixStream::~PosixStream()
{
	close(fd);
}

int S2::PosixStream::Write(const char * data, int size)
{
	int l = write(fd, data, size);

	tcdrain(fd);
	return l;
}

int S2::PosixStream::Read(char * data, int size)
{
	return read(fd, data, size);
}


void S2::Sleep(double s)
{
	usleep(s*1000000);
}

int set_interface_attribs(int fd, int speed)
{
    struct termios tty;

    if (tcgetattr(fd, &tty) < 0) {
        printf("Error from tcgetattr: %s\n", strerror(errno));
        return -1;
    }

    cfsetospeed(&tty, (speed_t)speed);
    cfsetispeed(&tty, (speed_t)speed);

    tty.c_cflag |= (CLOCAL | CREAD);    /* ignore modem controls */
    tty.c_cflag &= ~CSIZE;
    tty.c_cflag |= CS8;         /* 8-bit characters */
    tty.c_cflag &= ~PARENB;     /* no parity bit */
    tty.c_cflag &= ~CSTOPB;     /* only need 1 stop bit */
    tty.c_cflag &= ~CRTSCTS;    /* no hardware flowcontrol */

    /* setup for non-canonical mode */
    tty.c_iflag &= ~(IGNBRK | BRKINT | PARMRK | ISTRIP | INLCR | IGNCR | ICRNL | IXON);
    tty.c_lflag &= ~(ECHO | ECHONL | ICANON | ISIG | IEXTEN);
    tty.c_oflag &= ~OPOST;

    /* fetch bytes as they become available */
    tty.c_cc[VMIN] = 1;
    tty.c_cc[VTIME] = 1;

    if (tcsetattr(fd, TCSANOW, &tty) != 0) {
        printf("Error from tcsetattr: %s\n", strerror(errno));
        return -1;
    }
    return 0;
}

void set_mincount(int fd, int mcount)
{
    struct termios tty;

    if (tcgetattr(fd, &tty) < 0) {
        printf("Error tcgetattr: %s\n", strerror(errno));
        return;
    }

    tty.c_cc[VMIN] = mcount ? 1 : 0;
    tty.c_cc[VTIME] = 5;        /* half second timer */

    if (tcsetattr(fd, TCSANOW, &tty) < 0)
        printf("Error tcsetattr: %s\n", strerror(errno));
}

std::shared_ptr<S2::Stream> S2::DefaultStreamFactory::Open(Devices &devices, Generator & generator)
{
	if(generator.id==0)
		return std::make_shared<DummyGenerator>(true);

	auto filename = generator.filename;

	int fd = open(filename.c_str(), O_EXCL|O_RDWR|O_NOCTTY|O_SYNC);

	int error = errno;

	if(fd==-1)
	{
		const char * error = strerror(errno);
		throw IOError("Could not open device");
	}

	set_interface_attribs(fd, B57600);
	// set_mincount(fd, 5);

	return std::make_shared<PosixStream>(fd);
}

namespace Posix
{
	class Dir
	{
	public:
		Dir(const char * filename);
		~Dir();
		bool Next();
		const char * Filename() const;
		bool StartsWith(const char * pattern) const;
	private:
		DIR * dir;
		dirent * current;
	};
}

Posix::Dir::Dir(const char * filename)
{
	dir = opendir(filename);
}

Posix::Dir::~Dir()
{
	closedir(dir);
}

bool Posix::Dir::Next()
{
	current = readdir(dir);
	return current != nullptr;
}

const char * Posix::Dir::Filename() const
{
	return current->d_name;
}

bool Posix::Dir::StartsWith(const char * pattern) const
{
	return strncmp(Filename(),pattern,strlen(pattern))==0;
}

void S2::Devices::FindDevices()
{
	int generator=1, pulse=1;
	for(Posix::Dir dir("/dev"); dir.Next();)
	{
		if(dir.StartsWith("cu.SLAB_USBtoUART")||dir.StartsWith("ttyUSB"))
			generators.push_back(S2::Generator(generator++, std::string("/dev/") + dir.Filename()));

		if(dir.StartsWith("hidraw"))
			pulses.push_back(S2::Pulse(pulse++, std::string("/dev/") + dir.Filename()));
	}
}



