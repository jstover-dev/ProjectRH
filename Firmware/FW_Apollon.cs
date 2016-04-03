using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {
    public class FW_Apollon : AbstractFirmware {

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

                    cursor += 4;

                    if (data[cursor] != RICOH_PASSWORD_PADDING) {
                        while (data[cursor] != 0) {
                            currentName.Append((char)data[cursor]);
                            cursor++;
                        }
                        while (data[cursor] == 0) {
                            cursor++;
                        }
                    }
                    while (data[cursor] != RICOH_PASSWORD_PADDING) {
                        currentPassword.Append(HexConv(data[cursor]));
                        cursor++;
                    }
                    while (data[cursor] == RICOH_PASSWORD_PADDING) {
                        cursor++;
                    }
                    passwords.Add(new AdministratorPassword { ID = currentAdmin, Name = currentName.ToString(), Password = currentPassword.ToString().Replace(((char)0).ToString(), "") });
                }
            }
            return passwords;
        }

    }
}
