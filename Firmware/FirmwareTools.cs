using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {
    public static class FirmwareTools {

        public static IFirmware GetFirmware(string versionString){
            switch (versionString) {
                case "ATHENA_C1":
                    return new FW_Athena();
                case "APOLLON_C1":
                    return new FW_Apollon();
                case "GRIFFIN_C2":
                    return new FW_Griffin();
                case "METIS_C1_MIPS":
                    return new FW_Metis();
                case "VENUS_C2":
                    return new FW_Venus();
                default:
                    throw new UnrecognizedFirmwareException(versionString);
            }
        }


        public static String GetFirmwareString(byte[] data) {
            StringBuilder sb = new StringBuilder();
            int cursor = Array.IndexOf(data, (byte)0x28) + 1;
            int maxLength = 32;
            while (data[cursor] != 0x29 && (--maxLength>0)) {
                sb.Append((char)data[cursor]);
                cursor++;
            }
            return sb.ToString();
        }


    }
}
