#pragma once
#include <map>

namespace S2
{
	class Preset
	{
	public:
		Preset(const std::string & filename);

		int NumberOfPrograms() const;
		const std::string & GetProgram(int p) const;
		const std::string & GetProgramDescription(int p) const;
	private:
		std::multimap<std::string, std::string> entries;
	};
}