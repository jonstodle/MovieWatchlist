using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbParser {
    public class Media : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private int _productionYear;
        public int ProductionYear {
            get { return _productionYear; }

            set {
                if(value != _productionYear) {
                    _productionYear = value;
                    OnPropertyChanged("ProductionYear");
                }
            }
        }

        private string _originalTitle;
        public string OriginalTitle {
            get { return _originalTitle; }

            set {
                if(value != _originalTitle) {
                    _originalTitle = value;
                    OnPropertyChanged("OriginalTitle");
                }
            }
        }

        private Dictionary<string, string> _contentRatings;
        public Dictionary<string, string> ContentRatings {
            get { return _contentRatings; }

            set {
                if(value != _contentRatings) {
                    _contentRatings = value;
                    OnPropertyChanged("ContentRatings");
                }
            }
        }

        private int _runtime;
        public int Runtime {
            get { return _runtime; }

            set {
                if(value != _runtime) {
                    _runtime = value;
                    OnPropertyChanged("Runtime");
                }
            }
        }

        private List<string> _categories;
        public List<string> Categories {
            get { return _categories; }

            set {
                if(value != _categories) {
                    _categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        private DateLocation _releaseDate;
        public DateLocation ReleaseDate {
            get { return _releaseDate; }

            set {
                if(value != _releaseDate) {
                    _releaseDate = value;
                    OnPropertyChanged("ReleaseDate");
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



        private void OnPropertyChanged(string property) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
