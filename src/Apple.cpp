#include "S2.h"
#include <iostream>
#include <iomanip>

#include <IOKit/hid/IOHIDManager.h>
#include <CoreFoundation/CFString.h>

namespace MacOS
{
	class HidManager;

	class HidDevice
	{
	public:
		HidDevice(IOHIDDeviceRef ref);

		IOHIDDeviceRef deviceRef;

		long ProductId() const;
		long VendorId() const;
		long LongProperty(CFStringRef) const;

		void ScheduleWithRunLoop() const;
		int Read(char * data, int length);

		virtual void OnInputReport(
                IOReturn        inResult,           // completion result for the input report operation
                IOHIDReportType inType,             // the report type
                uint32_t        inReportID,         // the report ID
                uint8_t *       buffer,           // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp);

		char buffer[64];
		int readLength;
	};

	class HidManager
	{
	public:
		HidManager();
		HidManager(const HidManager&)=delete;
		HidManager & operator=(const HidManager&)=delete;
		~HidManager();

		std::vector<HidDevice> devices;
		IOHIDManagerRef hidMgr;
	};
}


S2::Devices::Devices(const Options & options)
{
	std::cout << "Probing devices\n";
	generators.push_back(Generator(0, "/dev/cu.SLAB_USBtoUART"));

	pulses.push_back(Pulse(0, ""));

	MacOS::HidManager hm;
	for(auto &device : hm.devices)
	{
		if(device.VendorId()==1602 && device.ProductId()==7)
		{
			std::cout << "Pulse found'\n";
			device.ScheduleWithRunLoop();

			char buffer[64];
			device.Read(buffer,64);
		}
	}
}

MacOS::HidManager::HidManager()
{
	hidMgr = IOHIDManagerCreate(kCFAllocatorDefault, kIOHIDOptionsTypeNone);

	// !! Set matching devices here:
	IOHIDManagerSetDeviceMatching(hidMgr, nullptr);

	auto success = IOHIDManagerOpen(hidMgr, kIOHIDOptionsTypeNone);

   if(success != kIOReturnSuccess)
	   throw S2::IOError("Could not open the HID manager");

   	CFSetRef deviceSet = IOHIDManagerCopyDevices(hidMgr);

	int count = CFSetGetCount(deviceSet);
	std::vector<IOHIDDeviceRef> deviceIds(count);
	CFSetGetValues(deviceSet, (const void**)&deviceIds[0]);

	devices.reserve(count);
	for(auto id : deviceIds)
		devices.push_back(HidDevice(id));
}

MacOS::HidManager::~HidManager()
{
	IOHIDManagerClose(hidMgr, kIOHIDOptionsTypeNone);
	CFRelease(hidMgr);
}

MacOS::HidDevice::HidDevice(IOHIDDeviceRef ref) : deviceRef(ref)
{
}

long MacOS::HidDevice::VendorId() const
{
	return LongProperty(CFSTR(kIOHIDVendorIDKey));
}

long MacOS::HidDevice::ProductId() const
{
	return LongProperty(CFSTR(kIOHIDProductIDKey));
}

long MacOS::HidDevice::LongProperty(CFStringRef name) const
{
    Boolean result = FALSE;
    int outValue=0;

    CFTypeRef tCFTypeRef = IOHIDDeviceGetProperty(deviceRef, name);
    if (tCFTypeRef) {
        // if this is a number
        if (CFNumberGetTypeID() == CFGetTypeID(tCFTypeRef)) {
            // get its value
            result = CFNumberGetValue((CFNumberRef) tCFTypeRef, kCFNumberSInt32Type, &outValue);
        }
    }
    return outValue;
}

void MacOS::HidDevice::ScheduleWithRunLoop() const
{
	IOHIDDeviceScheduleWithRunLoop(deviceRef, CFRunLoopGetCurrent(), kCFRunLoopDefaultMode);
}

static void Handle_IOHIDDeviceIOHIDReportCallback2(
                void *          inContext,          // context from IOHIDDeviceRegisterInputReportCallback
                IOReturn        inResult,           // completion result for the input report operation
                void *          inSender,           // IOHIDDeviceRef of the device this report is from
                IOHIDReportType inType,             // the report type
                uint32_t        inReportID,         // the report ID
                uint8_t *       buffer,           // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp)
{
	((MacOS::HidDevice*)inContext)->OnInputReport(inResult, inType, inReportID, buffer, inReportLength, timeStamp);
}

void MacOS::HidDevice::OnInputReport(
                IOReturn        inResult,           // completion result for the input report operation
                IOHIDReportType inType,             // the report type
                uint32_t        inReportID,         // the report ID
                uint8_t *       buffer,           // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp)
{
	const uint8_t * base = buffer+2;
	unsigned value = (base[0] << 16) | (base[1] << 8) | base[2];

	static uint64_t previousTimestamp;
	auto tsDiff = timeStamp - previousTimestamp;
	previousTimestamp = timeStamp;
	double secondsDiff = tsDiff/1000000000.0;

	double bpm = 60.0 / (double(value) / 250000.0);

	std::cout << "Time stamp Bpm = " << 60.0/secondsDiff
		<< ", device bpm = " << bpm << " bpm\n";

}

int MacOS::HidDevice::Read(char * buffer, int length)
{
	IOHIDDeviceRegisterInputReportWithTimeStampCallback
	(
		 deviceRef,
		 (uint8_t*)buffer,
		 length,
		 Handle_IOHIDDeviceIOHIDReportCallback2,
		 this);

	CFRunLoopRun();

	 return 0;	// !!
}

