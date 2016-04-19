﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectRH.DumpInspector;

namespace ProjectRH {

    public enum FirmwareRuleType {
        LoginByteOrder,
        LoginByteSet
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

            var rules = new List<FirmwareRule>();

            // These firmware versions reverse the order of the Admin Login ID bytes
            rules.Add(
                new FirmwareRule(FirmwareRuleType.LoginByteOrder) {
                    MatchingFirmwareStrings = new[] { "APOLLON_C2", "APOLLON_C25", "APOLLON_C3", "ATHENA_C25", "ATHENA_C3", "BELLINI_C3", "DIANA_C15", "MARTINI_C3", "METIS_C1_X86", "VENUS_C2" },
                    ReverseLoginByteOrder = true
            });


            // These versions use a different character set for the Admin ID bytes
            rules.Add(
                new FirmwareRule(FirmwareRuleType.LoginByteSet) {
                    MatchingFirmwareStrings = new string[] { "STELLA_C3", "STELLA_C4" },
                    LoginMajorByte = 0x03,
                    LoginMinorBytes = new byte[] { 0xF2, 0xF3, 0xF4, 0xF5, 0xF6 }
            });

            return rules;
        }

    }

}