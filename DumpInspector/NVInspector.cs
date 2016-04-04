using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProjectRH.Firmware;

namespace ProjectRH.DumpInspector {

    public class NVRAM {

        public string Filename { get; private set; }
        public Int64 Length { get { return this.Data.Length; } }

        private byte[] Data { get; set; }

        public String FirmwareString { get; private set; }


        public NVRAM(string filename) {
            this.Filename = Path.GetFileName(filename);
            this.Data = File.ReadAllBytes(filename);
            this.FirmwareString = GetFirmwareString();
        }

        public List<AdministratorLogin> GetPasswords() {
            return FirmwareTools.GetFirmware(FirmwareString).GetPasswords(Data);
        }

        public void WriteFile(String filename) {
            File.WriteAllBytes(filename, Data);
        }

        private String GetFirmwareString() {
            StringBuilder sb = new StringBuilder();
            int cursor = Array.IndexOf(Data, (byte)0x28) + 1;
            while (Data[cursor] != 0x29) {
                sb.Append((char)Data[cursor]);
                cursor++;
            }
            return sb.ToString();
        }


    }

}
