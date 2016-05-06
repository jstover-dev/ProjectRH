using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectRH.DumpInspector {

    public enum AdminIdLookupDirection { Behind=-1, Ahead=1 }

    public class FirmwareScanner {

        private byte[] Data { get; set; }

        public FirmwareScanner(byte[] data) {
            Data = data;
        }

        public string GetFirmwareString() {
            StringBuilder sb = new StringBuilder();
            int cursor = Array.IndexOf(Data, (byte)'(') + 1;
            int maxLength = 32;
            while (cursor<Data.Length && Data[cursor] != (byte)')' && (--maxLength > 0)) {
                sb.Append((char)Data[cursor]);
                cursor++;
            }
            return sb.ToString();
        }

        public UadVersion GetUadVersion() {
            for (int i = 0; i < Data.Length - 3; i++) {
                if (Data[i] == (byte)'U' && Data[i + 1] == (byte)'A' && Data[i + 2] == (byte)'D') {
                    return UadVersion.Get(int.Parse(((char)Data[i + 3]).ToString()));
                }
            }
            return UadVersion.Default;
        }


        private string ReadString(int start, int count, byte ignoreByte, bool decode = false) {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < start+count; i++) {
                if (Data[i] != ignoreByte) {
                    sb.Append((char)Data[i]);
                }
            }
            return decode? FirmwareTools.HexConv(sb.ToString()) : sb.ToString() ;
        }


        public List<AdministratorLogin> GetPasswords(IFirmwareDefinition fw) {
            List<AdministratorLogin> results = new List<AdministratorLogin>();

            for (int i = 1; i < Data.Length; i++) {
                if (Data[i] == fw.LoginMajorByte) {
                    byte adminByte;
                    if (fw.ReverseLoginByte) {
                        adminByte = Data[i - 1];
                    } else {
                        adminByte = Data[i + 1];
                        i++;
                    }

                    int currentAdmin;
                    if ((currentAdmin = Array.IndexOf(fw.LoginMinorBytes, adminByte)) < 0) {
                        continue;
                    }

                    i += fw.PrePadCount;
                    i++;

                    var currentUsername = ReadString(i, fw.UsernameLength, fw.UsernamePadByte, fw.EncryptedUsername);
                    i += fw.UsernameLength;

                    string currentPassword;
                    if (currentAdmin == 0) {
                        currentPassword = ReadString(i, fw.SupervisorPasswordLength, fw.PasswordPadByte, true);
                        i += fw.SupervisorPasswordLength;
                    } else {
                        currentPassword = ReadString(i, fw.AdministratorPasswordLength, fw.PasswordPadByte, true);
                        i += fw.AdministratorPasswordLength;
                    }

                    results.Add(new AdministratorLogin() {
                        UserId = currentAdmin,
                        Username = currentUsername,
                        Password = currentPassword
                    });
                }
            }


            return results;
        }






    }

}
