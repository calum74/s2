#include "S2.h"

#include <IOKit/hid/IOHIDManager.h>

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

		void StartRead(char * data, int length);
		void StopRead() const;

		virtual void OnInputReport(
                IOReturn        inResult,       // completion result for the input report operation
                IOHIDReportType inType,         // the report type
                uint32_t        inReportID,     // the report ID
                uint8_t *       buffer,         // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp);

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

	class PulseData : public S2::Stream
	{
	public:
		PulseData(HidDevice d);
		PulseData(const PulseData&)=delete;
		~PulseData();
	
		int Read(char * buffer, int size);
		int Write(const char * buffer, int size);
	public:
		HidDevice device;
		char buffer[64];
	};
}

class S2::Devices::Impl
{
public:
	MacOS::HidManager hm;
};

MacOS::PulseData::PulseData(HidDevice d) : device(d)
{
	device.StartRead(buffer, sizeof(buffer));
}

MacOS::PulseData::~PulseData()
{
	device.StopRead();
}

int MacOS::PulseData::Read(char * output, int size)
{
	CFRunLoopRun();
	if(size>5)
	{
		output[3] = buffer[2];
		output[4] = buffer[3];
		output[5] = buffer[4];
	}
	return std::min(size, 65);
}

int MacOS::PulseData::Write(const char *output, int size)
{
	throw std::logic_error("Writing to a pulse device");
}

S2::Devices::Devices(const Options & options)
{
	if(options.simulation)
	{
		generators.push_back(Generator(0,"Simulator"));
		pulses.push_back(Pulse(0, "Simulator"));
	}

	// !! More than one ...
	generators.push_back(Generator(1, "/dev/cu.SLAB_USBtoUART"));

	impl = std::make_shared<Impl>();

	int count=0;
	for(auto &device : impl->hm.devices)
	{
		count++;
		if(device.VendorId()==1602 && device.ProductId()==7)
		{
			pulses.push_back(Pulse(count, "pulse"));
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

	// !! Exceptions
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

static void Handle_IOHIDDeviceIOHIDReportCallback(
                void *          inContext,          // context from IOHIDDeviceRegisterInputReportCallback
                IOReturn        inResult,           // completion result for the input report operation
                void *          inSender,           // IOHIDDeviceRef of the device this report is from
                IOHIDReportType inType,             // the report type
                uint32_t        inReportID,         // the report ID
                uint8_t *       buffer,           // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp)
{
	auto hd = (MacOS::HidDevice*)inContext;
	hd->OnInputReport(inResult, inType, inReportID, buffer, inReportLength, timeStamp);
}

void MacOS::HidDevice::OnInputReport(
                IOReturn        inResult,           // completion result for the input report operation
                IOHIDReportType inType,             // the report type
                uint32_t        inReportID,         // the report ID
                uint8_t *       buffer,           // pointer to the report data
                CFIndex         inReportLength, // the actual size of the input report
                uint64_t 		timeStamp)
{
	CFRunLoopStop(CFRunLoopGetCurrent());
}

void MacOS::HidDevice::StartRead(char * buffer, int length)
{
	IOHIDDeviceScheduleWithRunLoop(deviceRef, CFRunLoopGetCurrent(), kCFRunLoopDefaultMode);

	IOHIDDeviceRegisterInputReportWithTimeStampCallback
	(
		 deviceRef,
		 (uint8_t*)buffer,
		 length,
		 Handle_IOHIDDeviceIOHIDReportCallback,
		 this);
}

void MacOS::HidDevice::StopRead() const
{
	IOHIDDeviceUnscheduleFromRunLoop(deviceRef, CFRunLoopGetCurrent(), kCFRunLoopDefaultMode);
}

void S2::Pulse::Open(Devices&devices)
{
	if(id==0)
		stream = std::make_shared<DummyPulse>();
	else
		stream = std::shared_ptr<MacOS::PulseData>(new MacOS::PulseData(devices.impl->hm.devices[id-1]));
}

