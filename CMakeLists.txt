cmake_minimum_required(VERSION 2.6)
project(s2)

set(CMAKE_CXX_STANDARD 11) # C++11...
set(CMAKE_CXX_STANDARD_REQUIRED ON) #...is required...
set(CMAKE_CXX_EXTENSIONS OFF) #...without compiler extensions like gnu++11

if(WIN32)
	set(PLATFORM_CPP src/Win32.cpp)
elseif(APPLE)
	find_library(IOKit IOKit)
	find_library(CoreFoundation CoreFoundation)
	set(PLATFORM_CPP src/Apple.cpp src/Posix.cpp)
	link_libraries(${IOKit} ${CoreFoundation})
else()
	set(PLATFORM_CPP src/Linux.cpp src/Posix.cpp)
endif()

add_library(s2core STATIC
	src/Control.cpp
	src/Devices.h
	src/Devices.cpp
	src/Device.h
	src/Generator.cpp
	src/Generator.h
	src/Options.h
	src/Options.cpp
	src/Preset.cpp
	src/Preset.h
	src/Program.h
	src/Program.cpp
	src/Pulse.cpp
	src/Pulse.h
	src/RunCommand.cpp
	src/S2.h
	src/Scan.cpp
	src/SetCommand.cpp
	src/Waveforms.cpp
	src/Waveforms.h
	${PLATFORM_CPP}
	)

include_directories(src)

add_executable(s2 cmdline/Main.cpp)
target_link_libraries(s2 s2core)

install (TARGETS s2 DESTINATION bin)
