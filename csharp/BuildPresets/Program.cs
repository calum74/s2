using System;
using System.IO;
using GeneratorLib;
using Spooky2;

namespace BuildPresets
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length !=2 )
            {
                Console.WriteLine("Usage: BuildPresets C:\\Spooky2 presets.db");
                return;
            }
            string spooky2dir = args[0];
            string outfile = args[1];
            var provider = new Spooky2.Provider(spooky2dir);
            var presets = ProgramFolder.Clone(provider.RootPresetCollection);

            presets.SaveDatabase(outfile);

            using (var file = File.OpenRead(outfile + ".idx"))
            {
                var root = new StoredProgramFolder();
                root.Stream = file;
                root.Offset = 0;
            }

            Console.WriteLine("Successfully generated database");
        }
    }
}
