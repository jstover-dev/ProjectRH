using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {
    public static class FirmwareTools {

        public static IFirmware GetFirmware(string versionString){
            
            if (versionString.StartsWith("ATHENA")) {
                return new FW_Athena();
            } else if (versionString.StartsWith("APOLLON")) {
                return new FW_Apollon();
            } else if (versionString.StartsWith("GRIFFIN")) {
                return new FW_Griffin();
            } else if (versionString.StartsWith("METIS")) {
                return new FW_Metis();
            } else if (versionString.StartsWith("VENUS")) {
                return new FW_Venus();
            } else {
                throw new UnrecognizedFirmwareException(versionString);
            }
        }

    }
}
