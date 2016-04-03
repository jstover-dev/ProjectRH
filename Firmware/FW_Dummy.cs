using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {
    public class FW_Dummy : AbstractFirmware {

        public override List<AdministratorPassword> GetPasswords(byte[] data) {

            int cursor = 0;
            int currentAdmin = -1;
            var passwords = new List<AdministratorPassword>();
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
