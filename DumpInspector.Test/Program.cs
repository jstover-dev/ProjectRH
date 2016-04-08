using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProjectRH;
using System.Diagnostics;

namespace DumpInspector.Test {

    class DataFile {
        public string Filename { get; private set; }
        public byte[] Data { get; private set; }
        public DataFile(string filename) {
            Filename = filename;
            Data = File.ReadAllBytes(filename);
        }
    }

    class Program {

        private static IEnumerable<DataFile> GetFirmwareDumps(string path) {
            foreach( string f in Directory.GetFiles(path, "*.nv", SearchOption.TopDirectoryOnly)) {
                yield return new DataFile(f);
            }
        }
        
        static void Main(string[] args) {

            Console.WriteLine("*** DumpInspector.Test ***");
            foreach (var f in GetFirmwareDumps(@"C:\Users\josh\Desktop\NV")) {
           
                FirmwareScanner scanner = new FirmwareScanner(f.Data);

                IFirmwareDefinition fw = new BasicFirmwareDefinition(
                    ValidFirmwareStrings: new[] { "APOLLON_C1" },
                    UADVersion: 7,
                    ReverseLoginByte: false,
                    PrePadCount: 4,
                    PostPadCount: 2
                );

                if (!fw.ValidFirmwareStrings.Contains(scanner.GetFirmwareString())){
                    continue;
                }

                Debug.Assert(scanner.GetUADVersion() == 7 || scanner.GetUADVersion() == 9);

                Console.WriteLine("\n================================================================================");
                Console.WriteLine("{0} ({1})", f.Filename, f.Data.Length);
                Console.WriteLine("UAD Version: {0}", scanner.GetUADVersion());
                Console.WriteLine("FW Type:     {0}", scanner.GetFirmwareString());
                Console.WriteLine("NameLength:  {0}", fw.UsernameLength);

                scanner.GetPasswords(fw);

            }


        }
    }
}
