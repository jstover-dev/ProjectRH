using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProjectRH.DumpInspector {

    public class FirmwareFile {

        // private fields
        private FirmwareScanner scanner;
        private byte[] content;


        // set during constructor
        public string FirmwareString    { get; private set; }

        // derived from file content
        public long Length { get { return content.Length; } }

        // 
        private IFirmwareDefinition _firmwareDefinition;
        public IFirmwareDefinition FirmwareDefinition {
            get { return _firmwareDefinition ?? new DefaultFirmwareDefinition(); }
            private set { _firmwareDefinition = value; }
        }

        private IEnumerable<AdministratorLogin> _administratorLogins;
        public IEnumerable<AdministratorLogin> AdministratorLogins { 
            get { return _administratorLogins ?? Rescan(); }
            set { _administratorLogins = value; }
        }


        

        /// <summary>
        /// Abstraction of a firmware dump file.
        /// </summary>
        /// <param name="filename">Filename to open</param>
        public FirmwareFile(string filename) {
            Open(filename);
        }

        private void Open(string filename) {
            content = File.ReadAllBytes(filename);
            scanner = new FirmwareScanner(content);
            FirmwareString = scanner.GetFirmwareString();
            FirmwareDefinition = new DefaultFirmwareDefinition {
                UadVersion = scanner.GetUadVersion()
            };
            Debug.WriteLine("Open: {0}", filename);
        }

        public IEnumerable<AdministratorLogin> Rescan(IFirmwareDefinition fw=null) {
            FirmwareDefinition = (fw ?? FirmwareDefinition);
            var passes = scanner.GetPasswords(FirmwareDefinition.ApplyRules(FirmwareString));
            return passes;
        }

        public void WriteFile(string filename) {
            File.WriteAllBytes(filename, content);
        }

    }

}
