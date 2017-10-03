#include "S2.h"
#include <iostream>
#include <string>

void RunProgram(const std::string &desc, const std::string &p)
{
	S2::Program program(p);
	std::cout << "Running " << desc << " for " << program.TotalDuration() << "s\n"; // std::endl;

	for (int i = 0; i < program.TotalSteps(); ++i)
	{
		std::cout << "  Running step " << (i + 1) << "/" << program.TotalSteps() << " for " << program.StepDuration(i) << "s\n";
	}
}

int S2::Run(const Options & options)
{
	try
	{
		// !!
		Preset preset(options.preset);
		int n = preset.NumberOfPrograms();
		for(int i=0; i<n; ++i)
		{
			RunProgram(preset.GetProgramDescription(i), preset.GetProgram(i));
		}
	}
	catch(std::exception&)
	{
	}

	return 0;
}