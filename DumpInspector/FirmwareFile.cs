using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProjectRH;

namespace ProjectRH.DumpInspector {

    public class FirmwareFile {

        public string Filename { get; private set; }
        public Int64 Length { get { return this.Data.Length; } }

        private byte[] Data { get; set; }

        public String FirmwareString { get; private set; }

        public FirmwareScanner Scanner { get; private set; }

        public FirmwareFile(string filename) {
            this.Filename = Path.GetFileName(filename);
            this.Data = File.ReadAllBytes(filename);
            this.Scanner = new FirmwareScanner(Data);
            this.FirmwareString = Scanner.GetFirmwareString();
        }

        public List<AdministratorLogin> GetPasswords() {
            foreach (var fwd in Firmware.GetFirmwareDefinitions()) {
                if (fwd.ValidFirmwareStrings.Contains(FirmwareString)) {
                    return Scanner.GetPasswords(fwd);
                }
            }
            throw new Exception(string.Format("Matching firmware definition not found [{0}]", FirmwareString));
        }

        public void WriteFile(String filename) {
            File.WriteAllBytes(filename, Data);
        }

    }

}
