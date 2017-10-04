#include "stdafx.h"
#include "S2.h"

#include <Windows.h>
#include <iostream>
#include <memory>

const char devicePrefix[] = "";
const char hidPrefix[] = "HID#VID_0642&PID_0007";
const char generatorHwPrefix[] = "\\Device\\Silabser";

namespace S2
{
	class Win32Stream : public Stream
	{
	public:
		Win32Stream(HANDLE h);
		~Win32Stream();
		virtual int Read(char * buffer, int size);
		virtual int Write(const char * buffer, int size);
	private:
		HANDLE file;
		HANDLE event;
	};
}

std::shared_ptr<S2::Stream> S2::OpenPulse(const std::string & filename)
{
	auto handle = CreateFileA(filename.c_str(), GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);

	if (handle == INVALID_HANDLE_VALUE)
		throw DeviceNotFound();


	return std::make_unique<Win32Stream>(handle);
	// std::unique_ptr<S2::Stream>(new Win32Stream(handle));
}

int S2::Win32Stream::Read(char * buffer, int size)
{
	DWORD bytesRead = 0;
	BOOL r = ReadFile(file, buffer, size, &bytesRead, nullptr);
	if (!r)
	{
		int r = GetLastError();
		throw IOError("Error reading from device");
	}
	return bytesRead;
}

S2::Win32Stream::Win32Stream(HANDLE h) : file(h)
{

}

int S2::Win32Stream::Write(const char * buffer, int size)
{
	DWORD bytesWritten = 0;
	BOOL w = WriteFile(file, buffer, size, &bytesWritten, nullptr);
	if (!w)
		throw IOError("Failed to write to generator");
	w = FlushFileBuffers(file);
	if(!w)
		throw IOError("Failed to write to generator");
	return bytesWritten;
}


S2::Devices::Devices(const Options & options)
{
	if (options.simulation)
	{
		generators.push_back(Generator(0, ""));
		pulses.push_back(Pulse(0, ""));
	}

	char devices[40096]; // !! 
	auto r = QueryDosDeviceA(NULL, devices, sizeof(devices));
	for (char * device = devices; device<devices + r; device += strlen(device) + 1)
	{
		if (strncmp(device, "COM", 3) == 0)
		{
			int id = atoi(device + 3);
			// Look up this device
			// char filename[MAX_PATH];
			char deviceID[MAX_PATH];
			memcpy(device - 4, "\\\\.\\", 4);
			if (QueryDosDeviceA(device, deviceID, sizeof(deviceID)))
			{
				if(strncmp(deviceID, generatorHwPrefix, sizeof(generatorHwPrefix)-1)==0)
				// !! if(str
					generators.push_back(Generator(id, device-4));
				// else: Not an interesting COM port
				
			}
		}
		else if (strncmp(device, hidPrefix, sizeof(hidPrefix) - 1) == 0)
		{
			memcpy(device - 4, "\\\\.\\", 4);
			pulses.push_back(Pulse(1 + pulses.size(), device-4));
		}
	}

}


void S2::Sleep(double s)
{
	::Sleep(int(1000*s));
}

std::shared_ptr<S2::Stream> S2::OpenGenerator(const std::string & filename)
{
	auto handle = CreateFileA(filename.c_str(), GENERIC_READ|GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);

	if (handle == INVALID_HANDLE_VALUE)
		throw DeviceNotFound();

	BOOL ok = PurgeComm(handle, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);
	DCB dcbInitState;
	GetCommState((HANDLE)handle, &dcbInitState);
	DCB newState = dcbInitState;
	newState.BaudRate = 57600;
	newState.Parity = NOPARITY;
	newState.ByteSize = 8;
	newState.StopBits = ONESTOPBIT;
	ok = SetCommState((HANDLE)handle, &newState);

	Sleep(0.06);

	return std::make_shared<Win32Stream>(handle);
}



S2::Win32Stream::~Win32Stream()
{
	CloseHandle(file);
}


