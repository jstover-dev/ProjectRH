using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH.DumpInspector {

    public static class FirmwareEncoding {

        private static readonly Dictionary<char, byte> _forward = new Dictionary<char, byte>();
        private static readonly Dictionary<byte, char> _reverse = new Dictionary<byte, char>();

        private static readonly string _characters = @"abcdefghijklmnopqrstuvwxzyABCDEFGHIJKLMNOPQRSTUVWXZY1234567890-=@+!#$%^&*()_,.?";
        private static readonly byte[] _defaultBytes = new byte[] { 0x2A, 0xEA, 0xAA, 0x6B, 0x2B, 0xEB, 0xAB, 0x68, 0x28, 0xE8, 0xA8, 0x69, 0x29, 0xE9, 0xA9, 0x6E, 0x2E, 0xEE, 0xAE, 0x6F, 0x2F, 0xEF, 0xAF, 0x6C, 0x2C, 0xEC, 0x22, 0xE2, 0xA2, 0x63, 0x23, 0xE3, 0xA3, 0x60, 0x20, 0xE0, 0xA0, 0x61, 0x21, 0xE1, 0xA1, 0x66, 0x26, 0xE6, 0xA6, 0x67, 0x27, 0xE7, 0xA7, 0x64, 0x24, 0xE4, 0x3E, 0xFE, 0xBE, 0x7F, 0x3F, 0xFF, 0xBF, 0x7C, 0x3C, 0x7E, 0x39, 0x3D, 0x62, 0xB8, 0x3A, 0xBA, 0x7B, 0x3B, 0xE5, 0xFB, 0xF8, 0x78, 0x38, 0xA5, 0x79, 0xF9, 0xBD };

        static FirmwareEncoding() {
            for(int i=0; i<_characters.Length; i++) {
                _forward.Add(_characters[i], _defaultBytes[i]);
                _reverse.Add(_defaultBytes[i], _characters[i]);
            }
        }

        public static char Decode(byte b) { return _reverse[b]; }

        public static byte Encode(char c) { return _forward[c]; }

        public static string Decode(string s) { return new String(s.Select(c => Decode((byte)c)).ToArray()); }

        public static IEnumerable<byte> Encode(string s) { return s.Select(c => Encode(c)); }

    }
}
