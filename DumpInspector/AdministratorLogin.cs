using System;

namespace ProjectRH {

    public class AdministratorLogin {

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString() {
            return string.Format("Administrator {0}: ({1}:{2})", ID, Username, Password);
        }

    }

}
