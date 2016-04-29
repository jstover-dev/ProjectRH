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
        byte PasswordPadByte            { get; }
        byte UsernamePadByte            { get; }
        bool ReverseLoginByte           { get; set; }
        byte LoginMajorByte             { get; set; }
        byte[] LoginMinorBytes          { get; set; }
    }


    public abstract class AbstractFirmwareDefinition : IFirmwareDefinition {
        public virtual int AdministratorCount { get { return 5; } }
        public virtual int UsernameLength               { get { return 0x20; } }
        public virtual int SupervisorPasswordLength     { get { return 0x20; } }
        public virtual int AdministratorPasswordLength  { get { return 0x40; } }
        public virtual bool EncryptedPassword           { get { return true; } }
        public virtual bool EncryptedUsername           { get { return UADVersion == 9; } }
        public virtual byte PasswordPadByte             { get { return EncryptedPassword ? (byte)0x72 : (byte)0; } }
        public virtual byte UsernamePadByte             { get { return EncryptedUsername ? (byte)0x72 : (byte)0; } }
        public virtual int PostPadCount                 { get { return 8 - PrePadCount; } }
        public virtual int PrePadCount                  { get { return ReverseLoginByte ? 6 : 4; } }

        public abstract int UADVersion                  { get; set; }
        public abstract byte LoginMajorByte             { get; set; }
        public abstract byte[] LoginMinorBytes          { get; set; }
        public abstract bool ReverseLoginByte           { get; set; }
    }


    public class DefaultFirmwareDefinition : AbstractFirmwareDefinition {

        public override int UADVersion                  { get; set; }
        public override byte LoginMajorByte             { get; set; }
        public override bool ReverseLoginByte           { get; set; }
        public override byte[] LoginMinorBytes          { get; set; }

        public DefaultFirmwareDefinition() {
            this.UADVersion = 9;
            this.ReverseLoginByte = false;
            this.LoginMajorByte = (byte)0xC3;
            this.LoginMinorBytes = new byte[] { 0x5B, 0x5C, 0x5D, 0x5E, 0x5F };
        }

    }





}

