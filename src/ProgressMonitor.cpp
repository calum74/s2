
#include "S2.h"

S2::DefaultProgressMonitor::DefaultProgressMonitor(std::ostream & os): output(os)
{
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

void S2::DefaultProgressMonitor::OutputState(int generator, int channel, double amplitude, double frequency, BuiltinWaveform wf)
{
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

