using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectRH;
using System.Diagnostics;

namespace ProjectRH {

    public enum AdminIdLookupDirection { Behind=-1, Ahead=1 }

    public class FirmwareScanner {

        private static readonly byte[] RICOH_ADMIN_ID = new byte[] { 0x5B, 0x5C, 0x5D, 0x5E, 0x5F };
        
        protected static readonly byte RICOH_ADMIN_START = 0xC3;

        private byte[] Data { get; set; }


        public FirmwareScanner(byte[] data) {
            Data = data;
        }

        public string GetFirmwareString() {
            StringBuilder sb = new StringBuilder();
            int cursor = Array.IndexOf(Data, (byte)0x28) + 1;
            int maxLength = 32;
            while (Data[cursor] != 0x29 && (--maxLength > 0)) {
                sb.Append((char)Data[cursor]);
                cursor++;
            }
            return sb.ToString();
        }

        public int GetUADVersion() {
            for (int i = 0; i < Data.Length - 3; i++) {
                if (Data[i] == 0x55 && Data[i + 1] == 0x41 && Data[i + 2] == 0x44) {
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
            return decode? Firmware.HexConv(sb.ToString()) : sb.ToString() ;
        }


        public List<AdministratorLogin> GetPasswords(IFirmwareDefinition fw) {
            List<AdministratorLogin> results = new List<AdministratorLogin>();

            byte adminByte;
            int currentAdmin;
            StringBuilder currentUsername = new StringBuilder();
            StringBuilder currentPassword = new StringBuilder();

            for (int i = 1; i < Data.Length; i++) {
                if (Data[i] == RICOH_ADMIN_START) {

                    if (fw.ReverseLoginByte) {
                        adminByte = Data[i - 1];
                    } else {
                        adminByte = Data[i + 1];
                        i++;
                    }
                    
                    if ((currentAdmin = Array.IndexOf(RICOH_ADMIN_ID, adminByte)) < 0) {
                        continue;
                    }

                    i += fw.PrePadCount;
                    i++;

                    string supervisor_username = ReadString(i, fw.UsernameLength, fw.UsernamePadByte);
                    string supervisor_password = ReadString(i, fw.SupervisorPasswordLength, fw.PasswordPadByte, true);
                    Console.WriteLine("Administrator {0}: {1}:{2}", currentAdmin, supervisor_username, supervisor_password);
                    Console.WriteLine();
                }
            }


            return results;
        }






    }

}
