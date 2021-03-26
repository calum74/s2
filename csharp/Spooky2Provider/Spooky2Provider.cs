using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using GeneratorLib;

namespace Spooky2
{
    public class Spooky2Programs : IProgramFolder
    {
        public string Name => "Spooky² programs";

        public IEnumerable<IProgram> Programs => Enumerable.Empty<IProgram>();

        string folder = "C:\\Spooky2\\Whatever\\Presets Collection";
        string tempFile = "C:\\Spooky2\\Downloaded-Presets-Collection.zip";
        string source = "https://calumgrant.github.io/spooky2/Presets%Collection.zip";

        void DownloadData()
        {
            if(!Directory.Exists(source))
            {
                while(!File.Exists(tempFile))
                {
                    var wc = new WebClient();
                    wc.DownloadFile(source, tempFile);
                    ZipFile.ExtractToDirectory(tempFile, folder);
                    File.Delete(tempFile);

                }
            }

        }

        public IEnumerable<IProgramFolder> Folders
        {
            get
            {
                DownloadData();
                // yield return new StoredProgramFolder() { Stream = presetsCollection };
                yield return new Spooky2PresetFolder(Path.Combine(RootDirectory, "Preset Collections"));
                yield return new Spooky2DatabaseFile(Path.Combine(RootDirectory, "Custom.csv"));
            }
        }

        string RootDirectory = "C:\\Spooky2";

        MemoryStream presetsCollection = new MemoryStream(Spooky2Provider.Resource1.presets2);


    }

    // Wave-forms as defined by Spooky2
    public enum Waveforms
    {
        Unspecified = 0,
        Sine = 1,
        Square,
        Sawtooth,
        InvertedSawtooth,
        Triangle,
        DampedSinusoidal,
        DampedSquare,
        HBombSinusoidal,
        HBombSquare,
        UserDefined1,
        UserDefined2
    }

    public class Provider : IProvider, IDisposable
    {

        public override string ToString() => "Spooky²";

        string RootDirectory { get; }
        public Provider(string rootDirectory)
        {
            RootDirectory = rootDirectory;
            hardwareList = new HardwareList();
            hardwareList.GeneratorAdded += x => OnHardwareChanged();
            hardwareList.GeneratorRemoved += x => OnHardwareChanged();
            hardwareList.MonitorAdded += x => OnHardwareChanged();
            hardwareList.MonitorRemoved += x => OnHardwareChanged();
            // var db = Path.Combine(rootDirectory, "openspooky.db");

            // Attempt to load file from resources.
            using (var stream = new MemoryStream(Spooky2Provider.Resource1.openspooky))
                ReverseLookup = GeneratorLib.ReverseLookup.Load(stream);

            presetsCollection = new MemoryStream(Spooky2Provider.Resource1.presets2); 

//              ReverseLookup = File.Exists(db) ? GeneratorLib.ReverseLookup.Load(db) : new EmptyLookup(); 
        }

        MemoryStream presetsCollection;

        public IProgramFolder Custom => new Spooky2DatabaseFile(Path.Combine(RootDirectory, "Custom.csv"));

        public IProgramFolder RootPresetCollection { get; } = new Spooky2Programs();
        // => new StoredProgramFolder() { Stream = presetsCollection };
          // new Spooky2PresetFolder(Path.Combine(RootDirectory, "Preset Collections"));

        public IEnumerable<IHeartRateMonitor> HeartRateMonitors => hardwareList.HeartRateMonitors;

        public IEnumerable<ISignalGenerator> Generators => hardwareList.Generators;

        public IReverseLookup ReverseLookup { get; private set; }

        private HardwareList hardwareList;

        public void Dispose() { hardwareList.Dispose(); }

        public delegate void GeneratorChangedDel(ISignalGenerator g);

        public event GeneratorChangedDel GeneratorAdded;
        public event GeneratorChangedDel GeneratorRemoved;

        public event HardwareChangedDel OnHardwareChanged;
    }

}
