using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {

    public class UADVersion {

        public int VersionNumber      { get; private set; }

        public bool EncryptedPassword { get; private set; }

        public bool EncryptedUsername { get; private set; }

        public static readonly UADVersion Default = new UADVersion() {
            VersionNumber = 0,
            EncryptedPassword = true,
            EncryptedUsername = false
        };

        public static readonly IList<UADVersion> KnownVersions = new List<UADVersion>() {
            Default,
            new UADVersion() {
                VersionNumber = 6,
                EncryptedPassword = true,
                EncryptedUsername = false,
            },
            new UADVersion() {
                VersionNumber = 7,
                EncryptedPassword = true,
                EncryptedUsername = false,
            },
            new UADVersion() {
                VersionNumber = 9,
                EncryptedUsername = true,
                EncryptedPassword = true
            }
        };
            
        public static UADVersion Get(int VersionNumber) {
            return KnownVersions.FirstOrDefault(u => u.VersionNumber == VersionNumber) ?? Default;
        }

        public override string ToString() {
            if (VersionNumber == 0)
                return "Custom";
            else
                return String.Format("UAD{0}", VersionNumber);
        }

    }
}
