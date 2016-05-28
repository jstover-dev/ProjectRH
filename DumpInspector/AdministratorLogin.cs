using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProjectRH.DumpInspector {

    public class AdministratorLogin : INotifyPropertyChanged  {

        public int UserId { get; set; }

        private string _username;
        public string Username {
            get { return _username; }
            set {
                _username = value;
                NotifyPropertyChanged("Username");
            }
        }

        private string _password;
        public string Password {
            get { return _password; }
            set {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() {
            return string.Format("Administrator {0}: ({1}:{2})", UserId, Username, Password);
        }

    }

}
