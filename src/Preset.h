#pragma once
#include <map>

namespace S2
{
	class Preset : public Runnable
	{
	public:
		Preset(const std::string & filename);

		int NumberOfPrograms() const;
		const std::string & GetProgram(int p) const;
		const std::string & GetProgramDescription(int p) const;

		double Duration() const;
		std::string Description() const;
		void GetStep(double time, OutputStep &step) const;
	private:
		std::multimap<std::string, std::string> entries;
	};
}