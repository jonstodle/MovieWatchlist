using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWatchlist {
    public sealed class DataHelper : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DataHelper _current;

        public static DataHelper(){
            _current = new DataHelper();
        }

        public static DataHelper Current {
            get {
                return _current;
            }
        }
    }
}
