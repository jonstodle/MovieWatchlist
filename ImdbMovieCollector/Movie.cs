using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbMovieCollector {
    public class Movie : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private string _title;
        public string Title {
            get { return _title; }

            set {
                if(value != _title) {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private int _releaseYear;
        public int ReleaseYear {
            get { return _releaseYear; }

            set {
                if(value != _releaseYear) {
                    _releaseYear = value;
                    OnPropertyChanged("ReleaseYear");
                }
            }
        }

        private List<string> _contentRatings;
        public List<string> ContentRatings {
            get { return _contentRatings; }
            set { _contentRatings = value; }
        }


        private int _duration;
        public int Duration {
            get { return _duration; }

            set {
                if(value != _duration) {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        private List<string> _genres;
        public List<string> Genres {
            get { return _genres; }

            set {
                if(value != _genres) {
                    _genres = value;
                    OnPropertyChanged("Genres");
                }
            }
        }

        private List<DateLocation> _releaseDates;
        public List<DateLocation> ReleaseDAtes {
            get { return _releaseDates; }

            set {
                if(value != _releaseDates) {
                    _releaseDates = value;
                    OnPropertyChanged("ReleaseDAtes");
                }
            }
        }

        private Dictionary<string, int> _ratingScores;
        public Dictionary<string, int> RatingScores {
            get { return _ratingScores; }

            set {
                if(value != _ratingScores) {
                    _ratingScores = value;
                    OnPropertyChanged("RatingScores");
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

        private Person _director;
        public Person Director {
            get { return _director; }

            set {
                if(value != _director) {
                    _director = value;
                    OnPropertyChanged("Director");
                }
            }
        }

        private List<Person> _writers;
        public List<Person> Writers {
            get { return _writers; }

            set {
                if(value != _writers) {
                    _writers = value;
                    OnPropertyChanged("Writers");
                }
            }
        }

        private List<Person> _stars;
        public List<Person> Stars {
            get { return _stars; }

            set {
                if(value != _stars) {
                    _stars = value;
                    OnPropertyChanged("Stars");
                }
            }
        }

        private List<Movie> _alsoLiked;
        public List<Movie> AlsoLiked {
            get { return _alsoLiked; }

            set {
                if(value != _alsoLiked) {
                    _alsoLiked = value;
                    OnPropertyChanged("AlsoLiked");
                }
            }
        }

        private List<Person> _cast;
        public List<Person> Cast {
            get { return _cast; }

            set {
                if(value != _cast) {
                    _cast = value;
                    OnPropertyChanged("Cast");
                }
            }
        }

        private void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
