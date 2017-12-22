#pragma once
#include <map>

namespace S2
{
	class Preset : public Sequence
	{
	public:
		Preset(const Options & options, std::istream &contents);

		int NumberOfPrograms() const;
		const std::string & GetProgram(int p) const;
		const std::string & GetProgramDescription(int p) const;

		double Duration() const;
		void GetState(double time, ChannelState & state) const;

		std::string Description() const;
	private:
		std::multimap<std::string, std::string> entries;
		std::vector<Program> programs;
	};
}
