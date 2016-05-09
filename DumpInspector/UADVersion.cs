using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRH.DumpInspector {

    public class UadVersion {

        public int VersionNumber      { get; private set; }

        public bool EncryptedPassword { get; private set; }

        public bool EncryptedUsername { get; private set; }

        public static readonly UadVersion Default = new UadVersion {
            VersionNumber = 0,
            EncryptedPassword = true,
            EncryptedUsername = false
        };

        public static readonly UadVersion Auto = Default;

        public static readonly IList<UadVersion> KnownVersions = new List<UadVersion> {
            Default,
            new UadVersion {
                VersionNumber = 6,
                EncryptedPassword = true,
                EncryptedUsername = false,
            },
            new UadVersion {
                VersionNumber = 7,
                EncryptedPassword = true,
                EncryptedUsername = false,
            },
            new UadVersion {
                VersionNumber = 9,
                EncryptedUsername = true,
                EncryptedPassword = true
            }
        };
            
        public static UadVersion Get(int versionNumber) {
            return KnownVersions.FirstOrDefault(u => u.VersionNumber == versionNumber) ?? Default;
        }

        public override string ToString() {
            return VersionNumber == 0 ? "Custom" : string.Format("UAD{0}", VersionNumber);
        }
    }
}
