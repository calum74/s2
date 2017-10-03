#include "S2.h"
#include <unistd.h>
#include <fcntl.h>
#include <errno.h>
#include <termios.h>

namespace S2
{
	class LinuxStream : public Stream
	{
	public:
		LinuxStream(int fd);
		~LinuxStream();

		int fd;
		int Read(char * data, int size);
		int Write(const char * data, int size);
	};
}

S2::LinuxStream::LinuxStream(int fd) : fd(fd)
{
}

S2::LinuxStream::~LinuxStream()
{
	close(fd);
}

int S2::LinuxStream::Write(const char * data, int size)
{
	int l = write(fd, data, size);

	tcdrain(fd);
	return l;
}

int S2::LinuxStream::Read(char * data, int size)
{
	return read(fd, data, size);
}


S2::Devices::Devices(const Options & options)
{
	generators.push_back(Generator(0, "/dev/cu.SLAB_USBtoUART"));
	pulses.push_back(Pulse(0, ""));
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

std::shared_ptr<S2::Stream> S2::OpenGenerator(const std::string &filename)
{
	int fd = open(filename.c_str(), O_EXCL|O_RDWR|O_NOCTTY|O_SYNC);

	int error = errno;

	if(fd==-1)
	{
		const char * error = strerror(errno);
		throw IOError("Could not open device");
	}

	set_interface_attribs(fd, B57600);
	// set_mincount(fd, 5);

	return std::make_shared<LinuxStream>(fd);
}

std::shared_ptr<S2::Stream> S2::OpenPulse(const std::string &)
{
	return std::make_shared<S2::DummyPulse>();
}
