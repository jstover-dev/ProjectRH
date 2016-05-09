namespace ProjectRH.DumpInspector {

    public interface IFirmwareDefinition {
        int AdministratorCount          { get; }
        int UsernameLength              { get; }
        int SupervisorPasswordLength    { get; }
        int AdministratorPasswordLength { get; }
        int PrePadCount                 { get; }
        int PostPadCount                { get; }
        bool EncryptedPassword          { get; }
        bool EncryptedUsername          { get; }
        byte PasswordPadByte            { get; }
        byte UsernamePadByte            { get; }
        UadVersion UadVersion           { get; set; }
        bool ReverseLoginByte           { get; set; }
        byte LoginMajorByte             { get; set; }
        byte[] LoginMinorBytes          { get; set; }
    }


    public abstract class AbstractFirmwareDefinition : IFirmwareDefinition {
        public virtual int AdministratorCount           { get { return 5; } }
        public virtual int UsernameLength               { get { return 0x20; } }
        public virtual int SupervisorPasswordLength     { get { return 0x20; } }
        public virtual int AdministratorPasswordLength  { get { return 0x40; } }
        public virtual bool EncryptedPassword           { get { return UadVersion.EncryptedPassword; } }
        public virtual bool EncryptedUsername           { get { return UadVersion.EncryptedUsername; } }
        public virtual byte PasswordPadByte             { get { return EncryptedPassword ? (byte)0x72 : (byte)0; } }
        public virtual byte UsernamePadByte             { get { return EncryptedUsername ? (byte)0x72 : (byte)0; } }
        public virtual int PostPadCount                 { get { return 8 - PrePadCount; } }
        public virtual int PrePadCount                  { get { return ReverseLoginByte ? 6 : 4;} }

        public virtual UadVersion UadVersion            { get; set; }
        public virtual byte LoginMajorByte              { get; set; }
        public virtual byte[] LoginMinorBytes           { get; set; }
        public virtual bool ReverseLoginByte            { get; set; }
    }


    public sealed class DefaultFirmwareDefinition : AbstractFirmwareDefinition {
        public DefaultFirmwareDefinition() {
            this.ApplyRule(FirmwareRule.Default);
        }
    }

}

