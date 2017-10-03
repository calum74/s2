# s2 (Document in progress)

Cross-platform command line interface for Spooky2.

## Overview

s2 is a command-line utility for controlling the Spooky2, and is designed as a simple replacement for the standard Spooky2 software. This is useful in scenarios where you don't have access to a Windows PC, and can be used for example on a Mac or Linux device such as a Raspberry Pi. A Raspberry Pi is a very cheap and low power device and is a great solution for people who don't want to buy a Windows PC or leave a PC switched on overnight.

Features in common with the Spooky2:
* Run presents from the Spooky2 collection.
* Run saved Spooky2 programs.

Additional features (over Spooky2):
* Run 2 different programs at the same time on the same generator.
* Perform biofeedback scans using a new algorithm, and exports the results.
* Biofeedback scans can be interrupted and resumed.
* Low level control, setting specific generator parameters
* Pulse rate measurement.

Missing features:
* Reverse lookup. This is because the Spooky2 database is encrypted, which I fully respect.
* Some advanced options.
* Convenient installer and a graphical user interface.

Note that this software is not officially supported by Spooky2.


## Getting started

### Installation

There are two ways to get the software. Pre-build binaries are available, which can be copied to your computer. Otherwise follow the build instructions for your platform below. `s2` consists of a single file, so installation should not be too difficult.

Next, connect your generators and pulse and switch them on. On Windows, you may need to run the device driver setup program, located in `C:\Spooky2\CP210x_VCP_Windows`, and on MacOS, ... The drivers should automatically install on Linux.

https://www.silabs.com/products/development-tools/software/usb-to-uart-bridge-vcp-drivers

To see the attached devices, run

```
$ s2 status
```

This should list your attached hardware.

### Running a program

Copy the program from the Spooky2 software installation. Maybe use a memory stick. For example,

```
cd C:\Spooky2\Preset Collections\Detox\Contact
s2 run generator=3 preset="Intestinal Parasites (C) - BY.txt"
```

### Scanning

Scanning is performed using the `s2 scan` command as follows.

```
s2 scan
```

The generator can be specified using the `generator=N` argument. The scan command is designed to be simple to use and can be interrupted at any time by pressing Ctrl+C. Running `s2 scan` will resume from where the scan left off.

### Building

#### Building and installing on Linux

You can tailor these steps to suit your environment. For example, you may already have the prerequisites installed (git, cmake, g++), in which case you can simply skip that step.

```
$ sudo apt-get install git cmake g++
$ git clone https://github.com/calum74/s2.git
$ cd s2
$ cmake -DCMAKE_BUILD_TYPE=Release
$ make
$ sudo make install
$ s2 status
```

To upgrade, go to the `s2` directory containing the source code, and run `git pull && make && sudo make install` to install the latest version.

#### Building and installing on MacOS

#### Building and installing on Windows

## Quick start guide



## Commands

## Variables



| Variable name | Commands | Default value | Description | Example |
|---------------|----------|---------------|-------------|---------|
| amplitude |
| channel       | scan, run, control | 0 | Specifies which channel of the generator to use. Options are `0`, `1` or `2`. Channel `0` means "invert and sync" using both channels. | channel=1 |
| frequency |
| generator | scan, run, control | 0 | Specify which generator to use. By default, generator 0 uses the next available generator. | `generator=3` |
| pulse | | 0 | Specifies which pulse unit to use. `0` means use the first available pulse unit. Usually there is only one attached. | `pulse=2` |
| simulation | scan, run, control | off | Specifies whether to use simulated hardware. | `simulation=on` |
| waveform |


# Files
