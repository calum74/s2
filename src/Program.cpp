#include "S2.h"
#include <cstring>

S2::Program::Program(const std::string & text)
{
	Initialize(text);
}

S2::Program::Program(const std::string & name, const std::string & code)
{
	Initialize(code);
}

void S2::Program::Initialize(const std::string & text)
{
	int current = 0;
	int end = text.find(',', current);
	while (end != -1)
	{
		steps.push_back(text.substr(current, end - current).c_str());
		current = end + 1;
		end = text.find(',', current);
	}
	steps.push_back(text.substr(current).c_str());
}

double S2::Program::Duration() const
{
	double s = 0.0;
	for (unsigned i = 0; i < steps.size(); ++i)
		s += steps[i].Duration();
	return s;
}

S2::ProgramStep::ProgramStep(const char *s)
{
	auto eq = strchr(s, '=');
	auto dash = strchr(s, '-');
	
	f1 = atof(s);
	f2 = dash&&dash < eq ? atof(dash + 1) : f1;
	duration = eq ? atof(eq + 1) : 0.0;
}

void S2::ProgramStep::GetState(double d, S2::OutputState&output) const
{
	output.output = true;
	if (f1 == f2)
	{
		output.frequencyHz = f1;
		output.stepDuration = duration;
	}
	else
	{
		output.frequencyHz = f1 + (f2 - f1)*(d / duration);
		output.stepDuration = 0.0;  // Continuous
	}
}

double S2::ProgramStep::Duration() const
{
	return duration;
}


void S2::Program::GetState(double d, S2::OutputState&output) const
{
	if (d >= 0)
	{
		// Locate the sub-program
		for (const auto & step : steps)
		{
			if (d < step.Duration())
			{
				step.GetState(d, output);
				return;
			}
			else d -= step.Duration();
		}
	}
	output.output = false;
}
