using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ImdbInterface {
    public class Movie : INotifyPropertyChanged {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";
        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructors
        public Movie() { }

        public Movie(string imdbId) {
            if(imdbId == null) throw new ArgumentNullException(ID_NOT_VALID);
            ImdbId = imdbId;
        }

        public Movie(Uri imdbUri) {
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

        private bool CheckIdValidity(string imdbId) {
            return Regex.IsMatch(imdbId, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
        }

        #region Properties
        private string _imdbId;
        public string ImdbId {
            get { return _imdbId; }

            set {
                if(CheckIdValidity(value)) {
                    if(value != _imdbId) {
                        _imdbId = value;
                        OnPropertyChanged("ImdbId");
                    }
                } else throw new ArgumentException(ID_NOT_VALID);
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
            get {
                //TODO: Implement check for local file
                return _poster; 
            }

            set {
                if(value != _poster) {
                    _poster = value;
                    OnPropertyChanged("Poster");
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

        private DateTime _releaseDate;
        public DateTime ReleaseDate {
            get { return _releaseDate.Date; }

            set {
                if(value != _releaseDate) {
                    _releaseDate = value;
                    OnPropertyChanged("ReleaseDAtes");
                }
            }
        }

        private Dictionary<string, double> _ratingScores;
        public Dictionary<string, double> RatingScores {
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

        private List<Person> _directors;
        public List<Person> Directors {
            get { return _directors; }

            set {
                if(value != _directors) {
                    _directors = value;
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

        private Dictionary<string, Person> _cast;
        public Dictionary<string, Person> Cast {
            get { return _cast; }

            set {
                if(value != _cast) {
                    _cast = value;
                    OnPropertyChanged("Cast");
                }
            }
        }

        public bool HasOnlyNonDefaultValues {
            get {
                int defaultValuesCount = 0;
                if(string.IsNullOrWhiteSpace(ImdbId)) defaultValuesCount++;
                if(WebUri == null) defaultValuesCount++;
                if(LastFetch == null) defaultValuesCount++;
                if(PosterThumbnailUri == null) defaultValuesCount++;
                if(Poster == null) defaultValuesCount++;
                if(string.IsNullOrWhiteSpace(Title)) defaultValuesCount++;
                if(ReleaseYear == 0) defaultValuesCount++;
                if(ContentRatings == null) defaultValuesCount++;
                if(Duration == 0) defaultValuesCount++;
                if(Genres == null) defaultValuesCount++;
                if(ReleaseDate == null) defaultValuesCount++;
                if(RatingScores == null) defaultValuesCount++;
                if(string.IsNullOrWhiteSpace(Description)) defaultValuesCount++;
                if(Directors == null) defaultValuesCount++;
                if(Writers == null) defaultValuesCount++;
                if(Stars == null) defaultValuesCount++;
                if(AlsoLiked == null) defaultValuesCount++;
                if(Cast == null) defaultValuesCount++;
                return defaultValuesCount == 0;
            }
        }

        private bool _hasFetchedAllData;
        public bool HasFetchedAllData {
            get { return _hasFetchedAllData; }

            set {
                if(value != _hasFetchedAllData) {
                    _hasFetchedAllData = value;
                    OnPropertyChanged("HasFetchedAllData");
                }
            }
        }
        #endregion

        public void FetchAllData() {
            //TODO: Populate object
            var retriever = new MoviePageParser(this);

            HasFetchedAllData = true;
            LastFetch = DateTime.UtcNow;
        }

        public void Refresh() {
            FetchAllData();
        }

        private void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
