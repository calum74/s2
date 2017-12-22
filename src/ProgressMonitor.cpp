
#include "S2.h"

S2::DefaultProgressMonitor::DefaultProgressMonitor(Verbosity v, std::ostream & os): verbosity(v), output(os)
{
}

void S2::DefaultProgressMonitor::TotalRunTime(double seconds)
{
	output << "Total run time = " << seconds << "s\n";
}

void S2::DefaultProgressMonitor::RunPreset(const std::string & presetName, double duration)
{
}

void S2::DefaultProgressMonitor::RunProgram(const std::string & progName, double duration)
{
}

void S2::DefaultProgressMonitor::RunStep(const std::string & stepDescription, double duration)
{
}

void S2::DefaultProgressMonitor::RunCompleted()
{
}

void S2::DefaultProgressMonitor::GeneratorState(double time, ChannelId channel, double amplitude, double frequency, BuiltinWaveform wf)
{
	if(verbosity>=Verbose)
	{
		output << "[" << time << " " << channel.first << "." << channel.second << " " << amplitude << "V " << frequency << "Hz]          \r" << std::flush;
	}
}

void S2::DefaultProgressMonitor::HeartRateValue(double h)
{
}

void S2::DefaultProgressMonitor::HeartRateGood()
{
}

void S2::DefaultProgressMonitor::HeartRateBad()
{
}

void S2::DefaultProgressMonitor::ScanProgress()
{
}

void S2::DefaultProgressMonitor::ScanCompleted()
{
}

void S2::DefaultProgressMonitor::PulseFound(int id, const std::string & name)
{
}

void S2::DefaultProgressMonitor::GeneratorFound(int id, const std::string & name)
{

}

void S2::DefaultProgressMonitor::Error(const std::string & msg)
{
	output << msg << std::endl;
}

void S2::ProgressMonitor::HandleException()
{
	try
	{
		throw;
	}
	catch(std::exception &ex)
	{
		Error(ex.what());
	}
}
