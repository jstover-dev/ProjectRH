using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectRH.DumpInspector {

    public enum FirmwareRuleType {
        LoginByteOrder,
        LoginByteSet
    }

    public static class FirmwareRuleExtensions {
        public static IFirmwareDefinition ApplyRules(this IFirmwareDefinition fw, string fwversion) {
            foreach (var rule in FirmwareRule.GetRules().Where(r => r.MatchingFirmwareStrings.Contains(fwversion))) {
                Debug.WriteLine("Applying {0} Rule", rule.FirmwareRuleType);
                if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteOrder) {
                    fw.ReverseLoginByte = rule.ReverseLoginByteOrder;
                }
                if (rule.FirmwareRuleType == FirmwareRuleType.LoginByteSet) {
                    fw.LoginMajorByte = rule.LoginMajorByte;
                    fw.LoginMinorBytes = rule.LoginMinorBytes;
                }
            }
            return fw;
        }
    }

    public class FirmwareRule {

        public FirmwareRuleType FirmwareRuleType { get; private set; }

        public string[] MatchingFirmwareStrings { get; set; }

        public bool ReverseLoginByteOrder { get; set; }

        public byte LoginMajorByte { get; set; }

        public byte[] LoginMinorBytes { get; set; }

        public FirmwareRule(FirmwareRuleType type){
            FirmwareRuleType = type;
        }


        public static List<FirmwareRule> GetRules() {

            return new List<FirmwareRule> {

                // These firmware versions reverse the order of the Admin Login Id bytes
                new FirmwareRule(FirmwareRuleType.LoginByteOrder) {
                    MatchingFirmwareStrings =
                        new[] {
                            "APOLLON_C2", "APOLLON_C25", "APOLLON_C3", "ATHENA_C25", "ATHENA_C3", "BELLINI_C3",
                            "DIANA_C15", "MARTINI_C3", "METIS_C1_X86", "VENUS_C2"
                        },
                    ReverseLoginByteOrder = true
                },

                // These versions use a different character set for the Admin Id bytes
                new FirmwareRule(FirmwareRuleType.LoginByteSet) {
                    MatchingFirmwareStrings = new [] {"STELLA_C3", "STELLA_C4"},
                    LoginMajorByte = 0x03,
                    LoginMinorBytes = new byte[] {0xF2, 0xF3, 0xF4, 0xF5, 0xF6}
                }
            };
        }

    }

}
