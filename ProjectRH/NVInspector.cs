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


        public string AsString() {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in this.Bytes) {
                sb.Append(b.ToString("x"));
            }
            return sb.ToString();
        }



    }

}
