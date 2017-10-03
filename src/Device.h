#pragma once

#include <memory>

namespace S2
{
	class Stream
	{
	public:
		virtual int Read(char * buffer, int size) = 0;
		virtual int Write(const char * buffer, int size) = 0;
		virtual ~Stream();
	};

	class Device
	{
	public:
		int id;
		std::string filename;

		Device();
		Device(int id, const char*);
		~Device();
	protected:
		std::shared_ptr<Stream> stream;
	};
}