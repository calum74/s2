#pragma once

namespace S2
{
	struct DatabaseProgram : public Code
	{
	public:
		DatabaseProgram(const Options & options, const std::string & row);

		std::string name, description, dataset, code, organ, disease;
		double durationMinutes;
		double stepSize;
		std::pair<const char*, const char*> GetCode() const;
	};

	class DatabaseVisitor
	{
	public:
		virtual void DatabaseFile(int &rowId, const std::string & filename);

		virtual void DatabaseFile(int & rowId, std::istream & file);

		// Raw access to all data files
		virtual void DatabaseRow(int rowId, const std::string & line);
	};

	class Database
	{
	public:
		Database();

		// Read in all of the columns
		void Index();

		std::vector<DatabaseProgram> programs;
		std::map<std::string, int> programsByName;
		std::map<double, int> lowerBound, upperBound;

		void VisitDataFiles(const Options &, DatabaseVisitor&) const;
	};
}
