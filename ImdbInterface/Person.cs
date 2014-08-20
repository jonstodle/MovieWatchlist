using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImdbInterface {
    public class Person : INotifyPropertyChanged {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\bnm\d+\b";
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructors
        public Person() { }

        public Person(string imdbId) {
            if(imdbId == null) throw new ArgumentNullException(ID_NOT_VALID);
            ImdbId = imdbId;
        }

        public Person(Uri imdbUri) {
            if(imdbUri == null) throw new ArgumentNullException(URI_NOT_VALID);
            ImdbId = ParseImdbUri(imdbUri);
        }
        #endregion

        #region Parsers
        private string ParseImdbUriString(string imdbUriString) {
            var match = Regex.Match(imdbUriString, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
            if(match.Success) return match.Value;
            else throw new ArgumentException(URI_NOT_VALID);
        }
        //TODO: Implement uri parser
        private string ParseImdbUri(Uri imdbUri) {
            return ParseImdbUriString(imdbUri.ToString());
        }
        #endregion

        private bool CheckValidity(string imdbId) {
            return Regex.IsMatch(imdbId, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
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

        private string _name;
        public string Name {
            get { return _name; }

            set {
                if(CheckValidity(value)) {
                    if(value != _name) {
                        _name = value;
                        OnPropertyChanged("Name");
                    }
                } else throw new ArgumentException(ID_NOT_VALID);
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
