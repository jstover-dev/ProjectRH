using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.DumpInspector {

    public enum FirmwareVersion {
        ATHENA_C1,
        APOLLON_C1,
        GRIFFIN_C2,
        METIS_C1_MIPS,
        VENUS_C2
    }

    public static class FirmwareVersionExtensions {
        public static FirmwareVersion ToFirmwareVersion(this string s) {
            switch (s) {
                case "ATHENA_C1":
                    return FirmwareVersion.ATHENA_C1;
                case "APOLLON_C1":
                    return FirmwareVersion.APOLLON_C1;
                case "GRIFFIN_C2":
                    return FirmwareVersion.GRIFFIN_C2;
                case "METIS_C1_MIPS":
                    return FirmwareVersion.METIS_C1_MIPS;
                case "VENUS_C2":
                    return FirmwareVersion.VENUS_C2;
                default:
                    throw new UnrecognizedFirmwareException(s);
            }
        }

    }

    public class FirmwareException : Exception {
        public FirmwareException(string message) : base(message) { }
    }

    public class UnsupportedFirmwareException : FirmwareException {
        public UnsupportedFirmwareException(FirmwareVersion fw)
            : base(String.Format("Firmware version not supported [{0}]", fw)) { }
    }

    public class UnrecognizedFirmwareException : FirmwareException {
        public UnrecognizedFirmwareException(String version)
            : base(String.Format("Unrecognized version string [{0}]", version)) { }
    }

}
