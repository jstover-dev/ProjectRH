using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectRH.DumpInspector {

    public enum FirmwareRuleType {
        Default,
        LoginByteOrder,
        LoginByteSet,
        UadVersion
    }

    public static class FirmwareRuleExtensions {

        // Allow firmware versions to apply rules to themselves based on a version string
        public static IFirmwareDefinition ApplyRules(this IFirmwareDefinition fw, string fwversion) {
            foreach (var rule in FirmwareRule.GetRules(fwversion)) {
                Debug.WriteLine("Applying {0} Rule for {1}", rule.FirmwareRuleType, fwversion);
                fw.ApplyRule(rule);
            }
            return fw;
        }

        // Apply rules to the target firmware definition
        public static IFirmwareDefinition ApplyRule(this IFirmwareDefinition fw, FirmwareRule rule) {
            if (rule.FirmwareRuleType == FirmwareRuleType.Default) {
                fw.UadVersion = UadVersion.Get(rule.UadVersion);
                fw.ReverseLoginByte = rule.ReverseLoginByteOrder;
                fw.LoginMajorByte = rule.LoginMajorByte;
                fw.LoginMinorBytes = rule.LoginMinorBytes;
            }
            if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteOrder) {
                fw.ReverseLoginByte = rule.ReverseLoginByteOrder;
            }
            if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteSet) {
                fw.LoginMajorByte = rule.LoginMajorByte;
                fw.LoginMinorBytes = rule.LoginMinorBytes;
            }
            if (rule.FirmwareRuleType == FirmwareRuleType.UadVersion) {
                fw.UadVersion = UadVersion.Get(rule.UadVersion);
            }
            return fw;
        }
    }

    public class FirmwareRule {

        public FirmwareRuleType FirmwareRuleType { get; private set; }

        public List<string> MatchingFirmwareStrings { get; set; }

        public bool ReverseLoginByteOrder { get; set; }

        public byte LoginMajorByte { get; set; }

        public byte[] LoginMinorBytes { get; set; }
        
        public int UadVersion { get; set; }

        public FirmwareRule(FirmwareRuleType type){
            FirmwareRuleType = type;
            MatchingFirmwareStrings = new List<string>();
        }

        public static IEnumerable<FirmwareRule> GetRules(string version) {
            return Rules.Where(r => r.MatchingFirmwareStrings.Contains(version));
        }        

        public static readonly FirmwareRule Default = new FirmwareRule(FirmwareRuleType.Default) {
            ReverseLoginByteOrder = false,
            LoginMajorByte = 0xC3,
            LoginMinorBytes = new byte[] {0x5B, 0x5C, 0x5D, 0x5E, 0x5F}
        };

        public static readonly IList<FirmwareRule> Rules = new List<FirmwareRule> {
            Default,

            // These firmware versions reverse the order of the Admin Login Id bytes
            new FirmwareRule(FirmwareRuleType.LoginByteOrder) {
                MatchingFirmwareStrings =
                    new List<string> {
                        "APOLLON_C2", "APOLLON_C25", "APOLLON_C3", "ATHENA_C25", "ATHENA_C3", "BELLINI_C3",
                        "DIANA_C15", "MARTINI_C3", "METIS_C1_X86", "VENUS_C2", "MARTINI_C4", "APOLLON_P1"
                    },
                ReverseLoginByteOrder = true
            },

            // These versions use a different character set for the Admin Id bytes
            new FirmwareRule(FirmwareRuleType.LoginByteSet) {
                MatchingFirmwareStrings = new List<string> {"STELLA_C3", "STELLA_C4", "KIR_C3", "APOLLON_P1", "ALEX_P1", "ZEUS_P1", "SINCERE_P3"},
                LoginMajorByte = 0x03,
                LoginMinorBytes = new byte[] {0xF2, 0xF3, 0xF4, 0xF5, 0xF6}
            }

        };


    }
}
