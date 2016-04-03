using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.DumpInspector {

    interface IScanner {
        List<AdministratorPassword> Scan(byte[] data);
    }

    public class NVScanner : IScanner {

        private static readonly byte RICOH_ADMIN_START = 0xC3;

        private static readonly byte[] RICOH_ADMIN_ID = new byte[] { 0x5B, 0x5C, 0x5D, 0x5E, 0x5F };

        private static readonly byte RICOH_PASSWORD_PADDING = 0x72;


        delegate List<AdministratorPassword> ScanAction(byte[] data);

        private Dictionary<FirmwareVersion, ScanAction> ScanActions = new Dictionary<FirmwareVersion, ScanAction>();

        private FirmwareVersion firmwareVersion { get; set; }


        public int Cursor { get; private set; }
        private int currentAdmin { get; set; }
        private StringBuilder currentName { get; set; }
        private StringBuilder currentPassword { get; set; }


        public NVScanner(FirmwareVersion version) {
            firmwareVersion = version;
            ScanActions.Add(FirmwareVersion.APOLLON_C1, Scan_APOLLON_C1);
            //ScanActions.Add(FirmwareVersion.ATHENA_C1, Scan_ATHENA_C1);
            //ScanActions.Add(FirmwareVersion.GRIFFIN_C2, Scan_GRIFFIN_C2);
            //ScanActions.Add(FirmwareVersion.METIS_C1_MIPS, Scan_METIS_C1_MIPS);
            //ScanActions.Add(FirmwareVersion.VENUS_C2, Scan_VENUS_C2);
        }


        public List<AdministratorPassword> Scan(byte[] data) {
            this.Cursor = 0;
            this.currentAdmin = -1;
            this.currentName = new StringBuilder();
            this.currentPassword = new StringBuilder();
            if (!ScanActions.ContainsKey(firmwareVersion)) {
                throw new UnsupportedFirmwareException(firmwareVersion);
            }
            return ScanActions[firmwareVersion](data);
        }


        private List<AdministratorPassword> Scan_ATHENA_C1(byte[] data) {
            List<AdministratorPassword> passwords = new List<AdministratorPassword>();
            return passwords;
        }

        private List<AdministratorPassword> Scan_APOLLON_C1(byte[] data) {
            List<AdministratorPassword> passwords = new List<AdministratorPassword>();

            while (Cursor < data.Length) {
                if (data[Cursor++] == RICOH_ADMIN_START) {

                    if ((currentAdmin = Array.IndexOf(RICOH_ADMIN_ID, data[Cursor++])) < 0) {
                        continue;
                    }
                    currentName.Clear();
                    currentPassword.Clear();

                    Cursor += 4;

                    if (data[Cursor] != RICOH_PASSWORD_PADDING) {
                        while (data[Cursor] != 0) {
                            currentName.Append((char)data[Cursor]);
                            Cursor++;
                        }
                        while (data[Cursor] == 0) {
                            Cursor++;
                        }
                    }
                    while (data[Cursor] != RICOH_PASSWORD_PADDING) {
                        currentPassword.Append(data[Cursor].RicohHexConv());
                        Cursor++;
                    }
                    while (data[Cursor] == RICOH_PASSWORD_PADDING) {
                        Cursor++;
                    }
                    passwords.Add(new AdministratorPassword { ID = currentAdmin, Name = currentName.ToString(), Password = currentPassword.ToString().Replace(((char)0).ToString(), "") });
                }
            }
            return passwords;
        }



        private List<AdministratorPassword> Scan_GRIFFIN_C2(byte[] data) {
            List<AdministratorPassword> passwords = new List<AdministratorPassword>();
            return passwords;
        }

        private List<AdministratorPassword> Scan_METIS_C1_MIPS(byte[] data) {
            List<AdministratorPassword> passwords = new List<AdministratorPassword>();
            return passwords;
        }

        private List<AdministratorPassword> Scan_VENUS_C2(byte[] data) {
            List<AdministratorPassword> passwords = new List<AdministratorPassword>();
            return passwords;
        }



    }
}
