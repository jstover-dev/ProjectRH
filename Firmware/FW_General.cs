using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {
    public class FW_General : AbstractFirmware {

        public override List<AdministratorLogin> GetPasswords(byte[] data) {
            var passwords = new List<AdministratorLogin>();
            return passwords;
        }
    }
}
