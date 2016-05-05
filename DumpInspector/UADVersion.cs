using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {

    public class UADVersion {

        public bool EncryptedPassword { get; private set; }

        public bool EncryptedUsername { get; private set; }

        public static readonly UADVersion Default = new UADVersion() {
            EncryptedPassword = true,
            EncryptedUsername = false
        };

        public static readonly IDictionary<int, UADVersion> KnownVersions = new Dictionary<int, UADVersion>() {
            {6, new UADVersion() {
                EncryptedPassword = true,
                EncryptedUsername = false,
            }},

            { 7, new UADVersion() {
                EncryptedPassword = true,
                EncryptedUsername = false,
            }},

            { 9, new UADVersion() {
                EncryptedUsername = true,
                EncryptedPassword = true
            }}
        };
            
        public static UADVersion Get(int VersionNumber) {
            UADVersion uad;
            return KnownVersions.TryGetValue(VersionNumber, out uad) ? uad : UADVersion.Default ;
        }

    }
}
