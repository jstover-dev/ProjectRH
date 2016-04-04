using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {
    public class FW_Venus : AbstractFirmware {

        public override List<AdministratorLogin> GetPasswords(byte[] data) {

            int cursor = 0;
            int currentAdmin = -1;
            var passwords = new List<AdministratorLogin>();
            var currentName = new StringBuilder();
            var currentPassword = new StringBuilder();

            while (cursor < data.Length) {
                if (data[cursor++] == RICOH_ADMIN_START) {
                    if ((currentAdmin = Array.IndexOf(RICOH_ADMIN_ID, data[cursor++])) < 0) {
                        continue;
                    }
                    currentName.Clear();
                    currentPassword.Clear();

                    //

                }
            } // end scan loop

            return passwords;
        }

    }
}
