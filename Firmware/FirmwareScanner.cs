using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.Firmware {

    public enum AdminIdLookupDirection { Behind=-1, Ahead=1 }

    public class FirmwareScanner {

        private static readonly byte[] RICOH_ADMIN_ID = new byte[] { 0x5B, 0x5C, 0x5D, 0x5E, 0x5F };
        
        protected static readonly byte RICOH_ADMIN_START = 0xC3;

        private byte[] Data { get; set; }

        public FirmwareCursor Cursor { get; private set; }

        public FirmwareScanner(byte[] data) {
            Cursor = new FirmwareCursor(data.Length);
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

        public IEnumerable<FirmwareCursor> GetAdminLoginPositions(AdminIdLookupDirection d) {
            for (int i = 0; i < Data.Length; i++) {
                if (Data[i] == RICOH_ADMIN_START) {
                    if (Array.IndexOf(RICOH_ADMIN_ID, Data[i + (int)d]) >= 0) {
                        yield return new FirmwareCursor(Data.Length).MoveTo(i);
                    }
                }
            }
        }

        public List<byte> ReadUntilByte(byte b) {
            List<byte> result = new List<byte>();
            while (Data[Cursor.Position] != b) {
                result.Add(Data[Cursor.Position]);
                Cursor.Next();
            }
            return result;
        }
    }

    public class FirmwareScannerException : FirmwareException {
        public FirmwareScannerException(string msg) : base(msg) { }
    }
}
