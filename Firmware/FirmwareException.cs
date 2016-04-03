using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {

    public class FirmwareException : Exception {
        public FirmwareException(string message) : base(message) { }
    }

    public class UnrecognizedFirmwareException : FirmwareException {
        public UnrecognizedFirmwareException(String version)
            : base(String.Format("Unrecognized version string [{0}]", version)) { }
    }

}
