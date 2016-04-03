using System;

namespace ProjectRH {

    public class AdministratorPassword {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        private uint NameStartByte { get; set; }
        private uint NameEndByte { get; set; }
        private uint PasswordStartByte { get; set; }
        private uint PasswordEndByte { get; set; }

    }

}
