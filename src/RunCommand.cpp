#include "S2.h"
#include <iostream>
#include <string>
#include <fstream>

void RunProgram(const std::string &desc, const std::string &p)
{
	S2::Program program(p);
	std::cout << "Running " << desc << " for " << program.Duration() << "s\n"; // std::endl;
}

int S2::Run(const Options & options)
{
	try
	{
		// !!
		std::ifstream file(options.preset);
		Preset preset(file);
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