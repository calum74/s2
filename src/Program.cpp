#include "S2.h"
#include <cstring>

S2::Program::Program(const std::string & text)
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

double S2::Program::TotalDuration() const
{
	double s = 0.0;
	for (int i = 0; i < steps.size(); ++i)
		s += StepDuration(i);
	return s;
}

int S2::Program::TotalSteps() const
{
	return steps.size();
}

double S2::Program::StepDuration(int n) const
{
	return steps[n].duration;
}

S2::Step::Step(const char *s)
{
	auto eq = strchr(s, '=');
	auto dash = strchr(s, '-');
	
	f1 = atof(s);
	f2 = dash&&dash < eq ? atof(dash + 1) : f1;
	duration = eq ? atof(eq + 1) : 0.0;
}