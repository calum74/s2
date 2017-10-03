#pragma once

namespace S2
{
	class Step
	{
	public:
		Step(const char * str);
		double f1, f2, duration;
	};

	class Program
	{
	public:
		Program(const std::string & program);

		void Run(const Options & options, Channel &c);

		double TotalDuration() const;
		int TotalSteps() const;
		double StepDuration(int s) const;
		double StepFrequency(int s) const;
	private:
		std::string program;
		std::vector<Step> steps;
	};
}