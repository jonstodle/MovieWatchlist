using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

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

        private string ParseImdbUri(Uri imdbUri) {
            return ParseImdbUriString(imdbUri.ToString());
        }
        #endregion

        private bool CheckValidity(string imdbId) {
            return Regex.IsMatch(imdbId, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
        }

        #region Properties
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

        public Uri WebUri { get { return new Uri(IMDB_BASE_URI + ImdbId); } }

        private DateTime _lastFetch;
        public DateTime LastFetch {
            get { return _lastFetch; }

            set {
                if(value != _lastFetch) {
                    _lastFetch = value;
                    OnPropertyChanged("LastFetch");
                }
            }
        }

        private Uri _posterThumbnailUri;
        public Uri PosterThumbnailUri {
            get { return _posterThumbnailUri; }

            set {
                if(value != _posterThumbnailUri) {
                    _posterThumbnailUri = value;
                    OnPropertyChanged("PosterThumbnailUri");
                }
            }
        }

        private Uri _posterUri;
        public Uri PosterUri {
            get { return _posterUri; }

            set {
                if(value != _posterUri) {
                    _posterUri = value;
                    OnPropertyChanged("PosterUri");
                }
            }
        }

        private BitmapImage _poster;
        public BitmapImage Poster {
            get { return _poster; }

            set {
                if(value != _poster) {
                    _poster = value;
                    OnPropertyChanged("Poster");
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

        private DateTime _birthDate;
        public DateTime BirthDate {
            get { return _birthDate; }

            set {
                if(value != _birthDate) {
                    _birthDate = value;
                    OnPropertyChanged("BirthDate");
                }
            }
        }

        private string _birthPlace;
        public string BirthPlace {
            get { return _birthPlace; }

            set {
                if(value != _birthPlace) {
                    _birthPlace = value;
                    OnPropertyChanged("BirthPlace");
                }
            }
        }

        private List<Movie> _knownFor;
        public List<Movie> KnownFor {
            get { return _knownFor; }

            set {
                if(value != _knownFor) {
                    _knownFor = value;
                    OnPropertyChanged("KnownFor");
                }
            }
        }

        private List<object> _filmography;
        public List<object> Filmography {
            get { return _filmography; }

            set {
                if(value != _filmography) {
                    _filmography = value;
                    OnPropertyChanged("Filmography");
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
