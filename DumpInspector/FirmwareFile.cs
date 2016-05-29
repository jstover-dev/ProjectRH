using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ProjectRH.DumpInspector {

    public class FirmwareFile {

        // private fields
        private FirmwareScanner scanner;
        private byte[] content;

        // set during constructor
        public string FirmwareString    { get; private set; }

        // derived from file content
        public long Length { get { return content.Length; } }

        // provides firmware version specifics
        private IFirmwareDefinition _firmwareDefinition;
        public IFirmwareDefinition FirmwareDefinition {
            get {
                if (_firmwareDefinition == null) {
                    _firmwareDefinition = new DefaultFirmwareDefinition();
                }
                return _firmwareDefinition;
            }
            private set { _firmwareDefinition = value; }
        }

        
        private IEnumerable<AdministratorLogin> _administratorLogins;
        public IEnumerable<AdministratorLogin> AdministratorLogins { 
            get {
                if (_administratorLogins == null) {
                    _administratorLogins = Rescan();
                }
                return _administratorLogins ;
            }
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
            Debug.WriteLine("Opening "+filename);
        }

        public IEnumerable<AdministratorLogin> Rescan(IFirmwareDefinition fw=null) {
            FirmwareDefinition = (fw ?? FirmwareDefinition);
            var passes = scanner.GetLogins(FirmwareDefinition.ApplyRules(FirmwareString));
            return passes;
        }

        public void Write(string filename) {
            var data = new List<byte>();

            var fwd = FirmwareDefinition;
            int cursor = scanner.GetUadPosition() + 1;

            data.AddRange(content.Take(cursor));
            foreach(var admin in AdministratorLogins) {

                data.AddRange(content.Skip(cursor).Take(8));
                cursor += 8;

                data.AddRange(fwd.EncryptedUsername ? FirmwareEncoding.Encode(admin.Username) : admin.Username.Cast<byte>().ToArray());
                data.AddRange(Enumerable.Repeat(fwd.UsernamePadByte, fwd.UsernameLength - admin.Username.Length));
                cursor += fwd.UsernameLength;

                int pwLength = admin.UserId == 0 ? fwd.SupervisorPasswordLength : fwd.AdministratorPasswordLength;
                data.AddRange(fwd.EncryptedPassword ? FirmwareEncoding.Encode(admin.Password) : admin.Password.Cast<byte>().ToArray());
                data.AddRange(Enumerable.Repeat(fwd.PasswordPadByte, pwLength - admin.Password.Length));
                cursor += pwLength;
            }
            data.AddRange(content.Skip(cursor));

            File.WriteAllBytes(filename, data.ToArray());
        }

    }

}
