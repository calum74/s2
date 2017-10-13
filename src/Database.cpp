#include "S2.h"

S2::Database::Database()
{
}

void S2::Database::Read(std::istream & is)
{
	std::string line;
	while (std::getline(is, line))
	{
		AddRow(line);
	}
}

void S2::Database::AddRow(const std::string &row)
{
	programs.push_back(DatabaseProgram(row));
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

S2::DatabaseProgram::DatabaseProgram(const std::string & row)
{
	std::string::const_iterator i = row.begin(), j = row.end();
	auto name = readStringColumn(i, j);
	auto dataset = readStringColumn(i, j);
	auto durationMinutes = readStringColumn(i, j);
	auto description = readStringColumn(i, j);
	auto code = readStringColumn(i, j);
	auto organOptional = readStringColumn(i, j);
	auto diseaseOptional = readStringColumn(i, j);
	auto stepSizeSeconds = readStringColumn(i, j);
}

double S2::Code::Duration() const
{
	return 0.0;
}

void S2::Code::GetState(double d, OutputState &s) const
{
}


std::string S2::DatabaseProgram::Description() const
{
	return "";
}

