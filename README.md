s2 (Document in progress)

Cross-platform command line interface for Spooky2.
Overview

s2 is a command-line utility for controlling the Spooky2 and is designed as a simple replacement for the standard Spooky2 software. This is useful in scenarios where you don't have access to a Windows PC, and it can be run on systems like a Mac, Linux, or even a Raspberry Pi—a very inexpensive, low-power device ideal for those who don’t want to keep a PC running 24/7.
Features in common with the Spooky2:

    Run presents from the Spooky2 collection.
    Run saved Spooky2 programs.
    Biofeedback scan.

Additional features (over Spooky2):

    Run two different programs simultaneously on the same generator.
    Perform biofeedback scans using a new algorithm and export the results to Spooky2.
    Interruptible and resumable biofeedback scans.
    Low-level control by setting specific generator parameters.
    Pulse rate function.
    Maintains a comprehensive activity diary.
    Ability to compress or extend a program to a given time period.

Missing features:

    Reverse lookup (due to the encrypted nature of the Spooky2 database).
    Some advanced options.
    A convenient installer and a graphical user interface.

Note: This software is not officially supported by Spooky2.
Getting Started
Installation

There are two main methods to get s2 running on your device:

    Pre-Built Binaries:
    Pre-built binaries are available and can be copied directly to your computer.

    Building from Source:
    Follow the platform-specific build instructions below. Since s2 is contained in a single file (source-wise), the installation is straightforward.

Before proceeding, ensure that your generators are connected and powered on.

    On Windows, you might need to run the device driver setup program (typically found in C:\Spooky2\CP210x_VCP_Windows).
    On Linux, the drivers should install automatically.
    For MacOS, additional configuration may be required (check your system’s driver support).

To verify your attached hardware, run:

s2 status

Running a Program

To run a Spooky2 program, copy the desired program file from your Spooky2 software installation (using a memory stick or network share, for example), then execute:

cd C:\Spooky2\Preset Collections\Detox\Contact
s2 run generator=3 preset="Intestinal Parasites (C) - BY.txt"

Scanning

The scan command is simple to use and designed to be interruptible with Ctrl+C. When you run:

s2 scan

the scan will resume from where it was interrupted. You can specify a particular generator by adding the generator=N argument.
Building
Building and Installing on Linux

You can tailor these instructions to fit your environment. If you already have the prerequisites (git, cmake, g++), you can skip installing them.

sudo apt-get install git cmake g++
git clone https://github.com/calum74/s2.git
cd s2
cmake -DCMAKE_BUILD_TYPE=Release
make
sudo make install
s2 status

To upgrade, navigate to the s2 source directory and run:

git pull && make && sudo make install

Building and Installing on MacOS

    Install Xcode Command Line Tools and CMake:
    Download CMake from cmake.org and install the Xcode command line tools:

xcode-select --install

Clone and Build:

    git clone https://github.com/calum74/s2.git
    cd s2
    cmake -DCMAKE_BUILD_TYPE=Release -GXcode
    xcodebuild

Building and Installing on Windows

For Windows users, follow these detailed steps:

    Install Prerequisites

    Before building s2, install the required tools:
        Git for Windows: For cloning the repository.
        CMake: For generating Visual Studio project files.
        Visual Studio 2019 or 2022: Ensure you install the C++ development tools.

    Clone the Repository

    Open PowerShell or Command Prompt and run:

git clone https://github.com/calum74/s2.git
cd s2

This command downloads the source code into a folder named s2.

Generate Visual Studio Solution with CMake

Create a separate build directory and run CMake to configure the build system.

For Visual Studio 2022, use:

mkdir build
cd build
cmake -G "Visual Studio 17 2022" -A x64 ..

For Visual Studio 2019, modify the command as follows:

mkdir build
cd build
cmake -G "Visual Studio 16 2019" -A x64 ..

CMake will generate a Visual Studio solution file (.sln) within the build directory.

Build the Project

Once CMake has finished, build the project using msbuild:

    msbuild s2.sln /p:Configuration=Release /p:Platform=x64

    Alternatively, open the generated .sln file in Visual Studio, select the Release mode, and build the solution (you can use the shortcut Ctrl + Shift + B).

After building, you can run s2 from the output directory. Verify your installation by running:

s2 status

Quick Start Guide

(This section can be expanded with further details on how to quickly get started with common tasks.)
Commands

(A list and detailed explanations of available commands would be added here.)
Variables
General Comments

Variables in s2 control the behavior of the current program. They are stored in %AppData%\.s2\ on Windows or in ~/.s2 on Linux and MacOS. Use the s2 set command to view or change default values.

Variables can be specified on the command line in the form A=B with no spaces. For example:

s2 run generator=5 preset="My scan.txt" duration=1h

Commands that include variables with units require both a numeric value and the unit (without spaces):

    Correct: 
    s2 control amplitude=5V
    s2 control frequency=500kHz

Incorrect (due to spaces or missing units):

    s2 control amplitude = 5V
    s2 control amplitude=5

You can also store variables in a file and pass them to s2 using the @ symbol:

s2 scan @my_settings.config

List of Variables
Variable name	Commands	Units	Values	Default value	Description
amplitude	control, run	V, mV	0-20/40V		Sets the peak-to-peak voltage. For dual generator use, the value is combined.
channel	scan, run, control	(none)	0, 1, 2	channel=1	Specifies which generator channel to use. (Channel 0 means "invert and sync" using both channels.)
duration	control, run	s, m, h			Sets the total duration for a program or preset.
frequency	control	uHz, mHz, Hz, kHz, MHz		frequency=123.45kHz	Changes the generator frequency.
generator	scan, run, control	(none)		generator=3	Specifies which generator to use; by default, generator 0 uses the next available generator.
pulse	pulse, scan	(none)		pulse=2	Specifies which pulse unit to use. (0 usually means the first available pulse unit.)
simulation	scan, run, control	(none)	on, off	simulation=on	Specifies whether to use simulated hardware.
waveform	run, scan	(none)		square	Sets the waveform type.
compress	run	(none)	disabled		Compresses or extends a program to a given time period.
loop	run, pulse	(none)	off		Specifies if the program should run in a loop.
iterations	run, pulse	(none)	1		Specifies the number of times to execute a command.
Files
