#include "S2.h"
#include <fstream>
#include <cctype>

bool contains(const std::string & search, const char * pattern, const char * patternend)
{
	return std::search(search.begin(), search.end(), pattern, patternend,
       [](char c1, char c2) { return toupper(c1)==toupper(c2); }) != search.end();
}

bool contains(const std::string & search, double target)
{
	char * end;
	// Look for all digits
	for(const char * p = search.c_str(), *q=p+search.length(); p<q; ++p)
	{
		if(std::isdigit(*p))
		{
			double value = std::strtod(p, &end);
			p=end;
			if(value>target*0.99 && value < target*1.01)
				return true;
		}
	}
	return false;
}

template<typename Fn>
void VisitPrograms(const S2::Options & options, Fn fn)
{
	std::string programsCsv = options.DataFile("programs.csv");
	std::ifstream file(programsCsv);
	std::string line;
	int lineNo=0;
	while(std::getline(file, line))
	{
		++lineNo;
		fn(lineNo, line);
	}
}

class FindOptions : public S2::OptionsVisitor
{
public:
	FindOptions(const S2::Options & options) : options(options) {}

	void Name(const char * name)
	{
		const char * nameEnd = name + strlen(name);
		VisitPrograms(options, [&](int id, const std::string&line) {
			if(contains(line, name, nameEnd))
			{
				S2::DatabaseProgram p(options, line);
				std::cout << id << ": " << p.name << " (" << p.dataset << ") " << p.durationMinutes << " mins" << std::endl;
			}
		});
	}

	void Frequency(double f)
	{
		std::string programsCsv = options.DataFile("programs.csv");
		std::ifstream file(programsCsv);
		std::string line;
		int id=0;
		while(std::getline(file, line))
		{
			++id;
			if(contains(line, f))
			{
				S2::DatabaseProgram p(options, line);
				std::cout << id << ": " << p.name << " (" << p.dataset << ") " << p.durationMinutes << " mins" << std::endl;
				std::cout << p.code << std::endl;
			}
		}
	}

private:
	const S2::Options & options;
};


int S2::FindCommand(const Options & options, ProgressMonitor &pm)
{
	//Database db;
	//std::string programsCsv = options.DataFile("programs.csv");
	//std::ifstream file(programsCsv);

	FindOptions find(options);
	options.Visit(find);

	return 0;
}
