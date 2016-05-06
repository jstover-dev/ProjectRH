using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            return Directory.GetFiles(path, "*.nv", SearchOption.TopDirectoryOnly).Select(f => new DataFile(f));
        }

        static void Main() {

            Console.WriteLine("*** DumpInspector.Test ***");
            foreach (var f in GetFirmwareDumps(@"C:\Users\josh\Desktop\NV")) {
                Console.WriteLine(f);
                //FirmwareScanner scanner = new FirmwareScanner(f.Data);
                /*
                foreach (IFirmwareDefinition fw in Firmware.GetFirmwareDefinitions()) {
                    if (fw.ValidFirmwareStrings.Contains(scanner.GetFirmwareString())) {

                        Debug.Assert(scanner.GetUADVersion() == fw.UADVersion);

                        Console.WriteLine("\n================================================================================");
                        Console.WriteLine("{0} ({1})", f.Filename, f.Data.Length);
                        Console.WriteLine("UAD Version: {0}", scanner.GetUADVersion());
                        Console.WriteLine("FW Type:     {0}", scanner.GetFirmwareString());
                        Console.WriteLine("NameLength:  {0}", fw.UsernameLength);

                        foreach (AdministratorLogin a in scanner.GetPasswords(fw)) {
                            Console.WriteLine(a);
                        }

                    }
                }
                */
            }

        }
    }
}
