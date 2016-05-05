using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProjectRH;

namespace ProjectRH.DumpInspector {

    public class FirmwareFile {

        public string Filename          { get; private set; }
        public String FirmwareString    { get; private set; }

        public Int64 Length { get { return this.Data.Length; } }

        private IFirmwareDefinition _FirmwareDefinition;
        public IFirmwareDefinition FirmwareDefinition {
            get { return _FirmwareDefinition ?? new DefaultFirmwareDefinition(); }
            set { _FirmwareDefinition = value; }
        }
        
        private FirmwareScanner Scanner { get; set; }
        private byte[] Data { get; set; }

        public FirmwareFile(string filename = null) {
            if (!String.IsNullOrEmpty(filename)) {
                this.Open(filename);
            }
        }

        public void Open(string filename) {
            this.Filename = Path.GetFileName(filename);
            this.Data = File.ReadAllBytes(filename);
            this.Scanner = new FirmwareScanner(Data);
            this.FirmwareString = Scanner.GetFirmwareString();
            this.FirmwareDefinition = new DefaultFirmwareDefinition() {
                UADVersion = Scanner.GetUADVersion()
            };
        }

        public List<AdministratorLogin> GetPasswords(IFirmwareDefinition fw = null) {
            if (String.IsNullOrEmpty(this.Filename)) {
                return new List<AdministratorLogin>();
            } 
            return Scanner.GetPasswords(
                (fw ?? this.FirmwareDefinition).ApplyRules(this.FirmwareString)
            );
        }

        public void WriteFile(String filename) {
            File.WriteAllBytes(filename, Data);
        }

    }

}
