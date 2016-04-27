using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectRH;
using System.Diagnostics;

namespace ProjectRH {

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

        public int GetUADVersion() {
            for (int i = 0; i < Data.Length - 3; i++) {
                if (Data[i] == (byte)'U' && Data[i + 1] == (byte)'A' && Data[i + 2] == (byte)'D') {
                    return int.Parse(((char)Data[i + 3]).ToString());
                }
            }
            return 0;
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


        public List<AdministratorLogin> GetPasswords() {
            var fwd = new DefaultFirmwareDefinition(GetUADVersion());
            foreach (var rule in FirmwareRule.GetRules().Where(r => r.MatchingFirmwareStrings.Contains(GetFirmwareString())) ) {
                
                Console.WriteLine("Using {0} Rule", rule.FirmwareRuleType);

                if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteOrder) {
                    fwd.ReverseLoginByte = rule.ReverseLoginByteOrder;
                }

                if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteSet) {
                    fwd.LoginMajorByte = rule.LoginMajorByte;
                    fwd.LoginMinorBytes = rule.LoginMinorBytes;
                }

            }

            return GetPasswords(fwd);
        }


        public List<AdministratorLogin> GetPasswords(IFirmwareDefinition fw) {
            List<AdministratorLogin> results = new List<AdministratorLogin>();

            byte adminByte;
            int currentAdmin;
            string currentUsername;
            string currentPassword;

            for (int i = 1; i < Data.Length; i++) {
                if (Data[i] == fw.LoginMajorByte) {

                    if (fw.ReverseLoginByte) {
                        adminByte = Data[i - 1];
                    } else {
                        adminByte = Data[i + 1];
                        i++;
                    }
                    
                    if ((currentAdmin = Array.IndexOf(fw.LoginMinorBytes, adminByte)) < 0) {
                        continue;
                    }

                    i += fw.PrePadCount;
                    i++;

                    currentUsername = ReadString(i, fw.UsernameLength, fw.UsernamePadByte, fw.EncryptedUsername);
                    i += fw.UsernameLength;

                    if (currentAdmin == 0) {
                        currentPassword = ReadString(i, fw.SupervisorPasswordLength, fw.PasswordPadByte, true);
                        i += fw.SupervisorPasswordLength;
                    } else {
                        currentPassword = ReadString(i, fw.AdministratorPasswordLength, fw.PasswordPadByte, true);
                        i += fw.AdministratorPasswordLength;
                    }

                    results.Add(new AdministratorLogin() {
                        ID = currentAdmin,
                        Username = currentUsername,
                        Password = currentPassword
                    });
                }
            }


            return results;
        }






    }

}
