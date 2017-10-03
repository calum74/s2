#include "stdafx.h"
#include "Spooky2.h"

#include <Windows.h>
#include "Hidsdi.h"
#include "SetupAPI.h"
//#include <initguid.h>
//#include "Hidclass.h"

#include <iostream>
#include <fstream>
#include <string>
#include <iomanip>

// void readFromFile

// Problem: Need to enumerate HIDs


const char hidPrefix[] = "\\\\?\\hid#vid_0642&pid_0007";

const char * bidDevice = "\\\\?\\hid#vid_0642&pid_0007#6&3328c60&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
const char * hidDevice = "\\\\?\\hid#vid_0642&pid_0007#6&374364e6&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
void Spooky2::ListHidDevices()
{
}

std::vector<std::string> EnumeratePulses()
{
	std::vector<std::string> result;

	GUID hidGuid;
	HidD_GetHidGuid(&hidGuid);

	auto deviceInfoSet = SetupDiGetClassDevs(
		&hidGuid,
		NULL,
		NULL,
		DIGCF_PRESENT | 
		DIGCF_DEVICEINTERFACE);

	if (!deviceInfoSet)
	{
		return result;
	}

	int e = GetLastError();

	SP_DEVICE_INTERFACE_DATA interfaceData;
	ZeroMemory(&interfaceData, sizeof(interfaceData));
	interfaceData.cbSize = sizeof(interfaceData);

	for (int index = 0; 
		SetupDiEnumDeviceInterfaces(
			deviceInfoSet,
			NULL,

			&hidGuid,
			index,
			&interfaceData);
			++index)
	{
		union
		{
			SP_DEVICE_INTERFACE_DETAIL_DATA detailData;
			char data[1024];
		} detailData;
		detailData.detailData.cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA);

		SP_DEVINFO_DATA devInfo;
		devInfo.cbSize = sizeof(devInfo);

		DWORD requiredSize;
		if (SetupDiGetDeviceInterfaceDetail(deviceInfoSet, &interfaceData, &detailData.detailData, sizeof(detailData), &requiredSize, &devInfo))
		{
			std::string narrowFile(detailData.detailData.DevicePath, detailData.detailData.DevicePath + wcslen(detailData.detailData.DevicePath));

			if (strncmp(narrowFile.c_str(), hidPrefix, sizeof(hidPrefix)-1) == 0)
			{
				result.push_back(narrowFile);
			}
			//std::wcout << "Device path: " << detailData.detailData.DevicePath << std::endl;
		}
		else
		{
			// std::cout << "Failed..." << requiredSize << std::endl;
		}
	}

	e = GetLastError();

	// !! Exceptions
	SetupDiDestroyDeviceInfoList(deviceInfoSet);
	return result;
}




void DisplayHR()
{
	auto handle = CreateFileA(hidDevice, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, nullptr);

	if (handle != INVALID_HANDLE_VALUE)
	{
		unsigned char buffer[1024];
		// std::cout << handle;
		DWORD bytesRead;
		LARGE_INTEGER timestamp, frequency;
		QueryPerformanceCounter(&timestamp);
		QueryPerformanceFrequency(&frequency);
		for (;;)
		{
			LARGE_INTEGER currentTime;
			ReadFile(handle, buffer, sizeof(buffer), &bytesRead, nullptr);
			QueryPerformanceCounter(&currentTime);

			// std::cout << "Read " << std::dec << bytesRead << " bytes: ";
			// std::cout << std::hex;
			//for (int i = 0; i < bytesRead; ++i)
			//	std::cout << std::setw(3) << unsigned(buffer[i]);// << " ";

																 // Data seems to live in bytes 3,4,5
			unsigned char extractedData[3] = { buffer[3], buffer[4], buffer[5] };

			//std::cout << int(extractedData[0]) << " " << int(extractedData[1]) << " " << int(extractedData[2]) << " ";
			//std::cout << (extractedData[0] << 8 | extractedData[1]) << " " << (extractedData[1] << 8 | extractedData[2]) << " ";

			unsigned value = (extractedData[0] << 16) | (extractedData[1] << 8) | extractedData[2];
			//std::cout << std::endl;
			//std::cout << std::dec << value << " " << value/250000.0 << std::endl;
			double HrInvertalFromPulse = value / 250000.0;


			double HrInterval = double(currentTime.QuadPart - timestamp.QuadPart) / double(frequency.QuadPart);
			timestamp = currentTime;
			//std::cout << "From timestamps, HR interval=" << HrInterval << "s, " << (60.0 / HrInterval) << " bpm\n";
			std::cout << "From pulse " << (60.0 / HrInvertalFromPulse) << " bpm, from timestamp " << (60.0 / HrInterval) << " bpm\n";
		}

		CloseHandle(handle);
	}
}

void findPulse()
{
	// ListHidDevices();
	DisplayHR();
	//std::ifstream pulse(hidDevice);
	//std::string line;
	//pulse >> line;
	//std::cout << line;
	// std::cout << std::r
}
