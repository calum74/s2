#include "S2.h"
#include <iostream>

int S2::Set(const Options & options)
{
	if (options.argc > 2)
	{
		// Options are set
		options.SaveOptions();
	}
	else
	{
		// Display current options
		options.SaveOptions(std::cout);
		std::cout << "generator=" << options.generator << std::endl;
		std::cout << "channel=" << options.channel << std::endl;
	}
	return 0;
}