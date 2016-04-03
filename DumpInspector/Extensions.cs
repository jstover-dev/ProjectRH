using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;

namespace ProjectRH.DumpInspector {
    public static class Extensions {

        public static InputGestureCollection AsCollection(this InputGesture g) {
            return new InputGestureCollection(new[] { g });
        }


        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string AsHumanFileSize(this Int64 value) {
            if (value < 0) { return "-" + AsHumanFileSize(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }



        public static string RicohHexConv(this string s) {
            return new String(s.Select(c => (char)RicohHexConv((byte)c)).ToArray());
        }

        public static string GetDirectoryName(this string s) {
            return Path.GetDirectoryName(s);
        }

        public static char RicohHexConv(this byte b) {
            switch (b) {
                case 0x2A: return 'a';
                case 0xEA: return 'b';
                case 0xAA: return 'c';
                case 0x6B: return 'd';
                case 0x2B: return 'e';
                case 0xEB: return 'f';
                case 0xAB: return 'g';
                case 0x68: return 'h';
                case 0x28: return 'i';
                case 0xE8: return 'j';
                case 0xA8: return 'k';
                case 0x69: return 'l';
                case 0x29: return 'm';
                case 0xE9: return 'n';
                case 0xA9: return 'o';
                case 0x6E: return 'p';
                case 0x2E: return 'q';
                case 0xEE: return 'r';
                case 0xAE: return 's';
                case 0x6F: return 't';
                case 0x2F: return 'u';
                case 0xEF: return 'v';
                case 0xAF: return 'w';
                case 0x6C: return 'x';
                case 0x2C: return 'y';
                case 0xEC: return 'z';
                case 0x22: return 'A';
                case 0xE2: return 'B';
                case 0xA2: return 'C';
                case 0x63: return 'D';
                case 0x23: return 'E';
                case 0xE3: return 'F';
                case 0xA3: return 'G';
                case 0x60: return 'H';
                case 0x20: return 'I';
                case 0xE0: return 'J';
                case 0xA0: return 'K';
                case 0x61: return 'L';
                case 0x21: return 'M';
                case 0xE1: return 'N';
                case 0xA1: return 'O';
                case 0x66: return 'P';
                case 0x26: return 'Q';
                case 0xE6: return 'R';
                case 0xA6: return 'S';
                case 0x67: return 'T';
                case 0x27: return 'U';
                case 0xE7: return 'V';
                case 0xA7: return 'W';
                case 0x64: return 'X';
                case 0x24: return 'Y';
                case 0xE4: return 'Z';
                case 0x3E: return '1';
                case 0xFE: return '2';
                case 0xBE: return '3';
                case 0x7F: return '4';
                case 0x3F: return '5';
                case 0xFF: return '6';
                case 0xBF: return '7';
                case 0x7C: return '8';
                case 0x3C: return '9';
                case 0x7E: return '0';
                case 0x39: return '-';
                case 0x3D: return '=';
                case 0x62: return '@';
                case 0xB8: return '+';
                case 0x3A: return '!';
                case 0xBA: return '#';
                case 0x7B: return '$';
                case 0x3B: return '%';
                case 0xE5: return '^';
                case 0xFB: return '&';
                case 0xF8: return '*';
                case 0x78: return '(';
                case 0x38: return ')';
                case 0xA5: return '_';
                case 0x79: return ',';
                case 0xF9: return '.';
                case 0xBD: return '?';
                default: return (char)0;

            }
        }
    }
}
