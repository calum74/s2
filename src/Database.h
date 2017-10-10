#pragma once

namespace S2
{
	class Code : public Runnable
	{
		std::string code;
	};

	struct DatabaseProgram : Code
	{
		std::string name;
		std::string description;
		double stepSize;
	};

	struct Frequency
	{
		double lowerBound, upperBound;
		int entry;
	};

	class Database
	{
	public:
		Database();

		// Read in all of the columns
		void Read(const std::string & filename);
		void Index();

		std::vector<DatabaseProgram> programs;
		std::vector<Frequency> lowerBound, upperBound;
		std::map<std::string, int> programsByName;

		Runnable & GetProgram(const std::string & programName);
	};
}