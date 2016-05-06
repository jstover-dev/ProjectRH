using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectRH.DumpInspector {

    public class FirmwareFile {

        public string Filename          { get; private set; }
        public String FirmwareString    { get; private set; }

        public Int64 Length { get { return Data.Length; } }

        private IFirmwareDefinition _firmwareDefinition;
        public IFirmwareDefinition FirmwareDefinition {
            get { return _firmwareDefinition ?? new DefaultFirmwareDefinition(); }
            set { _firmwareDefinition = value; }
        }
        
        private FirmwareScanner Scanner { get; set; }
        private byte[] Data { get; set; }

        public FirmwareFile(string filename = null) {
            if (!String.IsNullOrEmpty(filename)) {
                Open(filename);
            }
        }

        public void Open(string filename) {
            Filename = Path.GetFileName(filename);
            Data = File.ReadAllBytes(filename);
            Scanner = new FirmwareScanner(Data);
            FirmwareString = Scanner.GetFirmwareString();
            FirmwareDefinition = new DefaultFirmwareDefinition() {
                UadVersion = Scanner.GetUadVersion()
            };
        }

        public List<AdministratorLogin> GetPasswords(IFirmwareDefinition fw = null) {
            if (String.IsNullOrEmpty(Filename)) {
                return new List<AdministratorLogin>();
            } 
            return Scanner.GetPasswords(
                (fw ?? FirmwareDefinition).ApplyRules(FirmwareString)
            );
        }

        public void WriteFile(String filename) {
            File.WriteAllBytes(filename, Data);
        }

    }

}
