using System;
using System.Linq;
using Xunit;
using ProjectRH.DumpInspector;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Xunit.Abstractions;

namespace UnitTests {
    public class Scanner {

        private readonly ITestOutputHelper _console;
   
        public Scanner(ITestOutputHelper testOutputHelper) { _console = testOutputHelper; }

        private static readonly string testDataDir = @"C:\Users\Josh\Source\ProjectRH\TestData";

        [Fact]
        public void TestDataExists() {
            Assert.NotEmpty(Directory.EnumerateFiles(testDataDir));
        }

        [Fact]
        public void EmptyDataTest() {
            Assert.Throws<FirmwareScannerException>(() => new FirmwareScanner(Enumerable.Repeat<byte>(0, 127).ToArray()));
            Assert.Throws<FirmwareScannerException>(() => new FirmwareScanner(Enumerable.Repeat<byte>(0, 128).ToArray()).GetFirmwareString());
        }

        [Fact]
        public void FirmwareStringMatchesFilename() {
            ForEachTestFile(f => {
                var scanner = new FirmwareScanner(File.ReadAllBytes(f));
                var name = Path.GetFileName(f);
                Assert.Equal(name.Remove(name.IndexOf('.')), scanner.GetFirmwareString());
            });
        }

        [Fact]
        public void ReadFirmwareString() {
            ScanEachTestFile(scanner => {

            });

            foreach (var f in Directory.EnumerateFiles(testDataDir)) {
                var filename = Path.GetFileName(f);
                var fwname = filename.Remove(filename.IndexOf('.'));
                var scanner = new FirmwareScanner(File.ReadAllBytes(f));
                Assert.Equal(fwname, scanner.GetFirmwareString());
            }
        }

        [Fact]
        public void ReadUadPosition() {
            ScanEachTestFile(scanner => {
                Assert.True(scanner.GetUadPosition() > 0);
            });
        }

        [Fact]
        public void ReadUadVersion() {
            ScanEachTestFile(scanner => {
                Assert.NotEqual(scanner.GetUadVersion().VersionNumber, UadVersion.Default.VersionNumber);
            });
        }

        [Fact]
        public void ReadLogins() {
            ScanEachTestFile(scanner => {
                var fwd = new DefaultFirmwareDefinition() {
                    UadVersion = scanner.GetUadVersion()
                }.ApplyRules(scanner.GetFirmwareString());
                Assert.Equal(fwd.AdministratorCount, scanner.GetLogins(fwd).Count);
            });
        }

        [Fact]
        public void InvariantAdministratorCount() {
            ScanEachTestFile(scanner => {
                var fwd = new DefaultFirmwareDefinition() {
                    UadVersion = scanner.GetUadVersion()
                }.ApplyRules(scanner.GetFirmwareString());
                Assert.Equal(fwd.AdministratorCount, 5);
            });
        }


        public static void ScanEachTestFile(Action<FirmwareScanner> scannerAction) {
            ForEachTestFile(f => scannerAction(new FirmwareScanner(File.ReadAllBytes(f))));
        }

        public static void ForEachTestFile(Action<string> fileAction) {
            foreach( var f in Directory.EnumerateFiles(testDataDir)) {
                fileAction(f);
            }
        }

    }
}
