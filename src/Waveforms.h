#pragma once

namespace S2
{
	// These are built into the generator
	enum class BuiltinWaveform
	{
		sine, square, sawtooth, custom, unknown
	};

	// These are baked into the program
	// Anything else must be loaded from disk.
	enum class StandardWaveforms
	{
		sine, square, sawtooth, custom, hbomb, sbomb, ascendingsawtooth, descendingsawtooth,
	};

	typedef int WaveData[1024];

	void LoadFromFile(const char * filename, WaveData & out);

	void LoadFromFile(std::istream & is, WaveData & out);

	extern WaveData BuiltInWaveformData[16];
	extern WaveData h_bomb;  // etc
}
