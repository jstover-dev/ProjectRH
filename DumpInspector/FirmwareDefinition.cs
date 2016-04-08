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
        bool EncryptedUsername          { get; }
        bool ReverseLoginByte           { get; }
        byte PasswordPadByte            { get; }
        byte UsernamePadByte            { get; }
        string[] ValidFirmwareStrings   { get; }
    }


    public abstract class AbstractFirmwareDefinition : IFirmwareDefinition {
        virtual public int AdministratorCount           { get { return 5; } }
        virtual public int UsernameLength               { get { return 0x20; } }
        virtual public int SupervisorPasswordLength     { get { return 0x20; } }
        virtual public int AdministratorPasswordLength  { get { return 0x40; } }
        virtual public bool EncryptedPassword           { get { return true; } }
        virtual public bool EncryptedUsername           { get { return UADVersion == 9; } }
        virtual public byte PasswordPadByte             { get { return (byte)0x72; } }
        virtual public byte UsernamePadByte             { get { return UADVersion == 9 ? (byte)0x72 : (byte)0; } }
        virtual public int PostPadCount                 { get { return 8 - PrePadCount; } }
        virtual public int PrePadCount                  { get { return ReverseLoginByte ? 6 : 4; } }
        abstract public bool ReverseLoginByte           { get; protected set; }
        abstract public int UADVersion                  { get; protected set; }
        abstract public string[] ValidFirmwareStrings   { get; protected set; }
    }


    public class BasicFirmwareDefinition : AbstractFirmwareDefinition {

        override public string[] ValidFirmwareStrings   { get; protected set; }
        override public int UADVersion                  { get; protected set; }
        override public bool ReverseLoginByte           { get; protected set; }

        public BasicFirmwareDefinition(string[] ValidFirmwareStrings, int UADVersion, bool ReverseIDByteOrder) {
            this.ValidFirmwareStrings = ValidFirmwareStrings;
            this.UADVersion = UADVersion;
            this.ReverseLoginByte = ReverseIDByteOrder;
        }
    }


}

