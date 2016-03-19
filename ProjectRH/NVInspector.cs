using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjectRH {

    class NVInspector {

        // Start byte for login field
        private static byte RICOH_LOGIN_START = 0xC3;    // Ã

        // Login Identifiers
        private static byte[] RICOH_LOGIN_ID = { 0x5B, 0x5C, 0x5D, 0x5E, 0x5F };    //  [\]^_

        // password field length, including padding bytes
        private static uint RICOH_PASSWORD_FIELD_LENGTH = 64; // password field is 64 bytes

        // Padding for username field
        private static byte RICOH_USERNAME_PAD = 0x00;  // NUL

        // Padding for password field
        private static byte RICOH_PASSWORD_PAD = 0x72;  //  r

        // Length of the gap between login ID and name
        private static ushort RICOH_USERNAME_OFFSET = 4;

        // Hold the raw bytes of the file
        public byte[] Data { get; private set; }

        // Login Field Format
        // [START][ID](4)<username>[USERNAME_PAD]<password>[PASSWORD_PAD][NULL]



        public NVInspector(string filename) {
            this.Data = File.ReadAllBytes(filename);

            bool startFound = false;
            bool loginFound = false;
            bool usernameFound = false;
            int skip = RICOH_USERNAME_OFFSET;

            foreach (byte b in this.Data) {
                
                if (startFound) {
                    for(int i=0; i<RICOH_LOGIN_ID.Length; i++){
                        if (b == RICOH_LOGIN_ID[i]) {
                            loginFound = true;
                            Console.WriteLine(b.ToString("x2"));
                        }
                    }
                    startFound = false;
                
                } else if (loginFound) {
                    if (--skip == 0) {
                        skip = RICOH_USERNAME_OFFSET;
                        usernameFound = true;
                        loginFound = false;
                    }

                } else if (usernameFound) {
                    if (b != RICOH_USERNAME_PAD) {
                        Console.Write(b.ToString("x2") + " ");
                    }
                    else {
                        Console.WriteLine();
                        usernameFound = false;
                    }


                } else if (b == RICOH_LOGIN_START) {
                    startFound = true;
                }
            }

        }


        public string[] GetLines(int bytesPerLine = 16) {
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            bytesPerLine -= bytesPerLine % 8;
            int columns = bytesPerLine / 8;
            for (int i = 0; i < this.Data.Length; i += bytesPerLine) {
                for (int j = 0; j < columns; j++) {
                    int k_start = i + (8*j);
                    int k_end = Math.Min(this.Data.Length, k_start + 8);
                    for (int k = k_start; k < k_end; k++) {
                        sb.Append(this.Data[k].ToString("x2"));
                        sb.Append(" ");
                    }
                    sb.Append("  ");
                }
                lines.Add(sb.ToString().Trim().ToUpper());
                sb.Clear();
            }
            return lines.ToArray();
        }


    }

}
