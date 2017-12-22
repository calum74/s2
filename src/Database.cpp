#include "S2.h"
#include <fstream>

S2::Database::Database()
{
}

void S2::Database::VisitDataFiles(const Options & options, DatabaseVisitor&visitor) const
{
	int id=1;
	
	// TODO: Locate other datafiles as well
	visitor.DatabaseFile(id, options.DataFile("programs.csv"));
}

void S2::DatabaseVisitor::DatabaseFile(int & row, const std::string & filename)
{
	std::ifstream file(filename.c_str());
	DatabaseFile(row, file);
}

void S2::DatabaseVisitor::DatabaseFile(int & row, std::istream & file)
{
	std::string line;
	while(std::getline(file, line))
	{
		DatabaseRow(row++, line);
	}
}

void S2::DatabaseVisitor::DatabaseRow(int id, const std::string & line)
{
}

template<typename It>
std::pair<It,It> readColumn(It &i, It end)
{
	It start = i;
	if (*start == '\"')
	{
		++start;
		for (i++; i < end && *i!='\"'; ++i)
		{
			if (*i == '\\') ++i;
		}
		It end2 = i++;
		if (i < end) i++;
		return std::make_pair(start, end2);
	}
	else
	{
		for (; i < end && *i != ','; ++i)
			;
		return std::make_pair(start, i<end ? i++ : i);
	}
}

std::string readStringColumn(std::string::const_iterator &i, std::string::const_iterator end)
{
	auto p = readColumn(i, end);
	return std::string(p.first, p.second);
}

S2::DatabaseProgram::DatabaseProgram(const Options & options, const std::string & row) : Code(options)
{
	std::string::const_iterator i = row.begin(), j = row.end();
	name = readStringColumn(i, j);
	dataset = readStringColumn(i, j);
	durationMinutes = atoi(readStringColumn(i, j).c_str());
	description = readStringColumn(i, j);
	code = readStringColumn(i, j);
	organ = readStringColumn(i, j);
	disease = readStringColumn(i, j);
	stepSize = atof(readStringColumn(i, j).c_str());
}

std::pair<const char*, const char*> S2::DatabaseProgram::GetCode() const
{
	return std::make_pair(code.c_str(), code.c_str()+code.length());
}

S2::Code::Code(const Options & options) : options(options)
{
}

