#pragma once

namespace S2
{
	typedef int WaveData[1024];

	void LoadFromFile(const char * filename, WaveData & out);

	extern WaveData BuiltInWaveformData[16];
	extern WaveData h_bomb;  // etc
}
