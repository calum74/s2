#pragma once

namespace S2
{
	class Code : public Sequence
	{
		std::string code;
		double Duration() const;
		void GetState(double time, OutputState & state) const;
	};

	struct DatabaseProgram : Code
	{
	public:
		DatabaseProgram(const std::string & row);
		std::string name;
		std::string description;
		double stepSize;

		std::string Description() const;
	};

	class Database
	{
	public:
		Database();

		// Read in all of the columns
		void Read(std::istream &file);
		void Index();

		std::vector<DatabaseProgram> programs;
		std::map<std::string, int> programsByName;
		std::map<double, int> lowerBound, upperBound;

		Program & GetProgram(const std::string & programName);

		// Query
		void AddRow(const std::string & row);
	};
}