using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ProjectRH.DumpInspector {

    [TestFixture]
    class NVScannerTest {

        [Test]
        public void ScanFile_L9067030164_1() {
            NVRAM nv = new NVRAM("Files\\L9067030164-1.nv");
            Assert.NotNull(nv);
            IEnumerable<AdministratorPassword> passwords = nv.GetPasswords();
            Assert.NotNull(passwords);
            List<AdministratorPassword> pwlist = passwords.ToList();
            for(int i=0; i<pwlist.Count; i++){
                AdministratorPassword pw = pwlist.ElementAt(i);
                Assert.AreEqual(pw.ID, i);
                if (i==0){
                    Assert.AreEqual(pw.Name, "supervisor");
                }
                if (i==1){
                    Assert.AreEqual(pw.Name, "admin");
                    Assert.AreEqual(pw.Password,"ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                }
            }
        }

        [Test]
        public void ScanFile_L9067030164_2() {
            NVRAM nv = new NVRAM("Files\\L9067030164-2.nv");
            Assert.NotNull(nv);
            IEnumerable<AdministratorPassword> passwords = nv.GetPasswords();
            Assert.NotNull(passwords);
            List<AdministratorPassword> pwlist = passwords.ToList();
            for (int i = 0; i < pwlist.Count; i++) {
                AdministratorPassword pw = pwlist.ElementAt(i);
                Assert.AreEqual(pw.ID, i);
                if (i == 0) {
                    Assert.AreEqual(pw.Name, "supervisor");
                }
                if (i == 1) {
                    Assert.AreEqual(pw.Name, "admin");
                    Assert.AreEqual(pw.Password, "password");
                }
            }
        }

        [Test]
        public void ScanFile_MPC2500() {
            NVRAM nv = new NVRAM("Files\\L3676120258-MPC2500.nv");
            Assert.NotNull(nv);
            IEnumerable<AdministratorPassword> passwords = nv.GetPasswords();
            Assert.NotNull(passwords);
            List<AdministratorPassword> pwlist = passwords.ToList();
            for (int i = 0; i < pwlist.Count; i++) {
                AdministratorPassword pw = pwlist.ElementAt(i);
                Assert.AreEqual(pw.ID, i);
                if (i == 0) {
                    Assert.AreEqual(pw.Name, "supervisor");
                    Assert.AreEqual(pw.Password, "password");
                }
                if (i == 1) {
                    Assert.AreEqual(pw.Name, "admin");
                    Assert.AreEqual(pw.Password, "password");
                }
            }
        }


    }
}
