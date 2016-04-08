using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectRH {

    public interface IFirmwareDefinition {
        int AdministratorCount          { get; }
        int UsernameLength              { get; }
        int SupervisorPasswordLength    { get; }
        int AdministratorPasswordLength { get; }
        int PrePadCount                 { get; }
        int PostPadCount                { get; }
        int UADVersion                  { get; }
        bool EncryptedPassword          { get; }
        bool ReverseLoginByte           { get; }
        byte UsernamePadByte            { get; }
        string[] ValidFirmwareStrings   { get; }
    }

    public abstract class AbstractFirmware : IFirmwareDefinition{
        virtual public int AdministratorCount           { get { return 5;    } }
        virtual public int UsernameLength               { get { return 0x20; } }
        virtual public int SupervisorPasswordLength     { get { return 0x20; } }
        virtual public int AdministratorPasswordLength  { get { return 0x40; } }
        virtual public bool EncryptedPassword           { get { return true; } }
        abstract public byte UsernamePadByte { get; }
        abstract public bool ReverseLoginByte { get; protected set; }
        abstract public int UADVersion { get; protected set; }
        abstract public int PrePadCount { get; protected set; }
        abstract public int PostPadCount { get; protected set; }
        abstract public string[] ValidFirmwareStrings { get; protected set; }
    }


    public class DefaultFirmware : AbstractFirmware {
        override public string[] ValidFirmwareStrings { get; protected set; }
        override public int UADVersion { get; protected set; }
        override public bool ReverseLoginByte { get; protected set; }
        override public int PrePadCount { get; protected set; }
        override public int PostPadCount { get; protected set; }
        override public byte UsernamePadByte    { get { return UADVersion==9 ? (byte)0x72 : (byte)0 ; } }

        public DefaultFirmware() { }

        public DefaultFirmware(string[] ValidFirmwareStrings, int UADVersion, bool ReverseLoginByte, int PrePadCount, int PostPadCount) {
            this.ValidFirmwareStrings = ValidFirmwareStrings;
            this.UADVersion = UADVersion;
            this.ReverseLoginByte = ReverseLoginByte;
            this.PrePadCount = PrePadCount;
            this.PostPadCount = PostPadCount;
        }
    }


    public static class Firmware {

        public static IFirmwareDefinition GetFirmware(string[] ValidFirmwareStrings, int UADVersion, bool ReverseLoginByte, int PrePadCount, int PostPadCount) {
            return new DefaultFirmware(
                ValidFirmwareStrings: ValidFirmwareStrings,
                UADVersion: UADVersion,
                ReverseLoginByte: ReverseLoginByte,
                PrePadCount: PrePadCount,
                PostPadCount: PostPadCount
            );

        }
    }

}
