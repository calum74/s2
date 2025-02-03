# s2 (Document in progress)

s2 is a cross-platform command-line utility for controlling the Spooky2 device. It serves as a lightweight, alternative replacement for the standard Spooky2 software and is ideal for scenarios where you may not have access to a Windows PC. Whether you're running on Linux, macOS, or a low-power device like a Raspberry Pi, s2 offers flexibility and ease of use.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
  - [Common Features](#common-features)
  - [Additional Features](#additional-features)
  - [Missing Features](#missing-features)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Running a Program](#running-a-program)
  - [Scanning](#scanning)
- [Building from Source](#building-from-source)
  - [Linux](#building-and-installing-on-linux)
  - [macOS](#building-and-installing-on-macos)
  - [Windows](#building-and-installing-on-windows)
- [Quick Start Guide](#quick-start-guide)
- [Commands](#commands)
- [Variables](#variables)
  - [General Comments](#general-comments)
  - [List of Variables](#list-of-variables)
- [Files](#files)

## Overview
s2 is designed for users who need a flexible, command-line based solution to run Spooky2 presets and programs without relying on a full Windows installation. Its cross-platform nature makes it a great choice for embedded systems and low-power devices.

## Features
### Common Features
- **Run Spooky2 Presets**: Execute preset presentations from the Spooky2 collection.
- **Execute Saved Programs**: Launch and control saved Spooky2 programs.
- **Biofeedback Scan**: Initiate biofeedback scans directly from the command line.

### Additional Features
- **Dual Program Execution**: Run two different programs simultaneously on the same generator.
- **Advanced Biofeedback Scans**: Utilize a new algorithm to perform scans and export results back to Spooky2.
- **Interruptible Scans**: Scans can be paused (Ctrl+C) and resumed.
- **Low-Level Control**: Set specific generator parameters for fine-tuned operation.
- **Pulse Rate Functionality**: Manage and adjust pulse rates.
- **Activity Diary**: Automatically logs all activities.
- **Time Compression/Extension**: Compress or extend a program to match a desired duration.

### Missing Features
- **Reverse Lookup**: Not available due to the encrypted nature of the Spooky2 database.
- **Some Advanced Options**: Certain advanced features are not yet implemented.
- **Graphical User Interface**: Currently, there is no installer or GUI.

**Note**: This software is not officially supported by Spooky2.

## Getting Started
### Installation
There are two primary methods to get s2 running:

1. **Pre-built Binaries:** Download the binaries and copy them to your machine.
2. **Building from Source:** Follow the platform-specific instructions below to build s2 yourself.

**Important:** Ensure your generators are connected and powered on.

- **Windows**: You may need to run the device driver setup program (usually found in `C:\Spooky2\CP210x_VCP_Windows`).
- **Linux**: Drivers should install automatically.
- **macOS**: Check for any necessary driver installations.

Verify your connected hardware by running:
```sh
s2 status
```

### Running a Program
To run a Spooky2 program, copy the desired file from your Spooky2 installation (via USB, network share, etc.) and run:
```sh
cd "C:\Spooky2\Preset Collections\Detox\Contact"
s2 run generator=3 preset="Intestinal Parasites (C) - BY.txt"
```

### Scanning
Start a biofeedback scan with:
```sh
s2 scan
```
- **Interrupt & Resume**: Press `Ctrl+C` to interrupt. Running `s2 scan` again will resume from where it left off.
- **Specify Generator**: Use `generator=N` to target a specific generator.

## Building from Source
### Building and Installing on Linux
```sh
sudo apt-get install git cmake g++
git clone https://github.com/calum74/s2.git
cd s2
cmake -DCMAKE_BUILD_TYPE=Release
make
sudo make install
s2 status
```
**Upgrading:**
```sh
git pull && make && sudo make install
```

### Building and Installing on macOS
1. Install Xcode Command Line Tools and CMake:
```sh
xcode-select --install
```
2. Clone and build:
```sh
git clone https://github.com/calum74/s2.git
cd s2
cmake -DCMAKE_BUILD_TYPE=Release -GXcode
xcodebuild
```

### Building and Installing on Windows
1. Install Prerequisites:
   - Git for Windows
   - CMake
   - Visual Studio 2019 or 2022 (with C++ development tools)

2. Clone the Repository:
```sh
git clone https://github.com/calum74/s2.git
cd s2
```

3. Generate Visual Studio Solution:
   - For Visual Studio 2022:
```sh
mkdir build
cd build
cmake -G "Visual Studio 17 2022" -A x64 ..
```
   - For Visual Studio 2019:
```sh
mkdir build
cd build
cmake -G "Visual Studio 16 2019" -A x64 ..
```

4. Build the Project:
```sh
msbuild s2.sln /p:Configuration=Release /p:Platform=x64
```
Alternatively, open the generated `.sln` file in Visual Studio, select Release mode, and build (Ctrl + Shift + B).

5. Verify your installation with:
```sh
s2 status
```

## Quick Start Guide
(Expand with concise examples and common use cases.)

## Commands
(Detailed command explanations and usage examples will be added here.)

## Variables
### General Comments
s2 uses variables to control program behavior. These settings are stored in:
- **Windows**: `%AppData%\.s2\`
- **Linux/macOS**: `~/.s2/`

Modify values with:
```sh
s2 set variable=value
```
Variables can also be passed on the command line:
```sh
s2 run generator=5 preset="My scan.txt" duration=1h
```
Load from a config file:
```sh
s2 scan @my_settings.config
```

### List of Variables
| Variable | Commands | Units | Values | Default | Description |
|----------|---------|-------|--------|---------|-------------|
| amplitude | control, run | V, mV | 0–20 / 40V | | Sets peak-to-peak voltage. |
| channel | scan, run, control | (none) | 0, 1, 2 | channel=1 | Selects generator channel. |
| duration | control, run | s, m, h | | | Defines total duration. |
| frequency | control | Hz, kHz | | | Adjusts generator frequency. |
| generator | scan, run, control | (none) | | | Selects generator. |
| pulse | pulse, scan | (none) | | | Selects pulse unit. |
| simulation | scan, run, control | (none) | on/off | simulation=on | Simulated hardware. |
| waveform | run, scan | (none) | | square | Sets waveform type. |
| compress | run | (none) | disabled | | Compresses/extends program. |
| loop | run, pulse | (none) | off | | Loops program execution. |
| iterations | run, pulse | (none) | 1 | | Execution count. |

## Files
(Additional file-related documentation can be added here.)

