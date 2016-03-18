using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjectRH {

    class NVInspector {

        public byte[] Bytes { get; private set; }

        public NVInspector(string filename) {
            this.Bytes = File.ReadAllBytes(filename);
        }



        public string[] GetLines(int bytesPerLine) {

            List<string> lines = new List<string>();

            StringBuilder sb = new StringBuilder();

            bytesPerLine -= bytesPerLine % 8;
            int columns = bytesPerLine / 8;

            for (int i = 0; i < this.Bytes.Length; i += bytesPerLine) {

                
                for (int j = 0; j < columns; j++) {

                    int k_start = i + (8*j);
                    int k_end = Math.Min(this.Bytes.Length, k_start + 8);

                    for (int k = k_start; k < k_end; k++) {
                        sb.Append(this.Bytes[k].ToString("x2"));
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
