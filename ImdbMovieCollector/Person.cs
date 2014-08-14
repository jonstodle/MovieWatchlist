using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbInterface {
    public class Person : INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name {
            get { return _name; }

            set {
                if(value != _name) {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _character;
        public string Character {
            get { return _character; }

            set {
                if(value != _character) {
                    _character = value;
                    OnPropertyChanged("Character");
                }
            }
        }

        private string _imdbId;
        public string ImdbId {
            get { return _imdbId; }

            set {
                if(value != _imdbId) {
                    _imdbId = value;
                    OnPropertyChanged("ImdbId");
                }
            }
        }

        private Uri _portraitUri;
        public Uri PorttraitUri {
            get { return _portraitUri; }

            set {
                if(value != _portraitUri) {
                    _portraitUri = value;
                    OnPropertyChanged("PorttraitUri");
                }
            }
        }

        private List<string> _roles;
        public List<string> Roles {
            get { return _roles; }

            set {
                if(value != _roles) {
                    _roles = value;
                    OnPropertyChanged("Roles");
                }
            }
        }

        private string _description;
        public string Description {
            get { return _description; }

            set {
                if(value != _description) {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        //private DateLocation _born;
        //public DateLocation Born {
        //    get { return _born; }

        //    set {
        //        if(value != _born) {
        //            _born = value;
        //            OnPropertyChanged("Born");
        //        }
        //    }
        //}

        private void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
