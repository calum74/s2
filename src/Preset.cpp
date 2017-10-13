#include "S2.h"
#include <fstream>
#include <iostream>
#include <string>

S2::Preset::Preset(std::istream & contents)
{
	if (!contents)
		throw IOError("Cannot load preset");

	std::string line;
	while (std::getline(contents, line))
	{
		bool startsWithQuote = line[0] == '\"';
		bool endsWithQuote = line.back() == '\"';
		while (startsWithQuote && !endsWithQuote)
		{
			std::string nextLine;
			if (!std::getline(contents, nextLine))
				throw IOError("Underterminated quote");
			line += "\n";
			line += nextLine;
			endsWithQuote = line.back() == '\"';
		}
		int eq = line.find('=');
		std::string key = line.substr(1, eq - 1);
		std::string value = line.substr(eq+1, line.length() - eq - 2);
		entries.insert(std::make_pair(key, value));
	}

	// Construct programs.
	for(auto code = entries.lower_bound("List4"), description = entries.lower_bound("List2");
		code != entries.upper_bound("List4") && description != entries.upper_bound("List2");
			++code, ++description)
	{
		programs.push_back(Program(description->second, code->second));
	}
}

int S2::Preset::NumberOfPrograms() const
{
	return entries.count("List4");
}

const std::string & S2::Preset::GetProgram(int n) const
{
	auto i = entries.lower_bound("List4");
	for (; n > 0; n--, i++)
		;
	return i->second;
}

const std::string & S2::Preset::GetProgramDescription(int n) const
{
	auto i = entries.lower_bound("List2");
	for (; n > 0; n--, i++)
		;
	return i->second;
}

double S2::Preset::Duration() const
{
	double d = 0.0;
	for(auto & p : programs)
		d += p.Duration();
	return d;
}

std::string S2::Preset::Description() const
{
	return "!!";
}

void S2::Preset::GetState(double d, OutputState &s) const
{
}