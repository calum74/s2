using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorLib
{
    public class Report
    {
        string ClientId;
        DateTime ScanTime;
        string SoftwareId;
        Waveform waveform;
        double MinFreq, MaxFreq;
        string ReponseType;
        double Threshold;
        string Algorithm;

        Dictionary<double, double> Hits;
    }

    interface ICloudServer
    {
        void SubmitReport(Report report);
    }

    class Server
    {
    }
}
