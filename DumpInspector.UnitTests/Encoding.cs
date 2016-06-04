using System;
using System.Linq;
using Xunit;
using ProjectRH.DumpInspector;
using System.Collections.Generic;

namespace UnitTests {
    public class Encoding {

        [Fact]
        public void HasEqualLengthCharacterSets() {
            Assert.Equal(FirmwareEncoding.DecodedCharacters.Length, FirmwareEncoding.EncodedBytes.Length);
        }

        [Fact]
        public void IsSymmetric() {
            foreach(var c in FirmwareEncoding.DecodedCharacters) {
                Assert.Equal(c, FirmwareEncoding.Decode(FirmwareEncoding.Encode(c)));
            }
        }

        [Fact]
        public void OneToOne() {
            foreach(char c in FirmwareEncoding.DecodedCharacters) {
                var targetBytes = new List<byte>();
                var encoded = FirmwareEncoding.Encode(c);
                Assert.DoesNotContain(encoded, targetBytes);
                targetBytes.Add(encoded);
            }
            foreach(byte b in FirmwareEncoding.EncodedBytes) {
                var targetChars = new List<char>();
                var decoded = FirmwareEncoding.Decode(b);
                Assert.DoesNotContain(decoded, targetChars);
                targetChars.Add(decoded);
            }
        }

    }
}
