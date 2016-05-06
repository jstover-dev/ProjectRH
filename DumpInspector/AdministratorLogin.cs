namespace ProjectRH.DumpInspector {

    public class AdministratorLogin {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString() {
            return string.Format("Administrator {0}: ({1}:{2})", UserId, Username, Password);
        }

    }

}
