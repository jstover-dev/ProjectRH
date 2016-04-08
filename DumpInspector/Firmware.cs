using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {
    public class Firmware {

        public static List<IFirmwareDefinition> GetFirmwareDefinitions() {

            var firmwares = new List<IFirmwareDefinition>();

            firmwares.Add(new BasicFirmwareDefinition(
                ValidFirmwareStrings: new []{ "APOLLON_C1", "ATHENA_C1" },
                UADVersion: 7,
                ReverseIDByteOrder: false
            ));

            firmwares.Add(new BasicFirmwareDefinition(
                ValidFirmwareStrings: new[] { "BELLINI_C3", "MARTINI_C3" },
                UADVersion: 7,
                ReverseIDByteOrder: true
            ));

            firmwares.Add(new BasicFirmwareDefinition(
                ValidFirmwareStrings: new[] { "APOLLON_C2", "APOLLON_C25", "APOLLON_C3", "ATHENA_C25", "ATHENA_C3", "DIANA_C15", "METIS_C1_X86", "VENUS_C2" },
                UADVersion: 9,
                ReverseIDByteOrder: true
            ));

            firmwares.Add(new BasicFirmwareDefinition(
                ValidFirmwareStrings: new[] { "UNKNOWN_MACHINE", "ALEX_C1", "DIANA_C1", "GRIFFIN_C2", "METIS_C1_MIPS", "ORVAL_C1", "RUSSIAN_C5" },
                UADVersion: 9,
                ReverseIDByteOrder: false
            ));

            return firmwares;
        }
    }
}
