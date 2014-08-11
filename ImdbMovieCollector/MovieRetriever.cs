using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ImdbMovieCollector {
    public class MovieRetriever {
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        public string ImdbId { get; private set; }

        #region Constructors
        public MovieRetriever(string imdbId) {
            SetId(imdbId);
        }

        public MovieRetriever(string imdbUri) {
            if(imdbUri == null) throw new ArgumentNullException(URI_NOT_VALID);
            string imdbId = null;
            imdbId = ParseImdbUriString(imdbUri);
            if(imdbId != null) SetId(imdbId);
            else throw new ArgumentException();
        }

        public MovieRetriever(Uri imdbUri) {
            if(imdbUri == null) throw new ArgumentNullException(URI_NOT_VALID);
            string imdbId = null;
            imdbId = ParseImdbUri(imdbUri);
            if(imdbId != null) SetId(imdbId);
            else throw new ArgumentException();
        }
        #endregion

        #region Init helpers
        private void SetId(string imdbId) {
            if(CheckIdValidity(imdbId)) {
                this.ImdbId = imdbId;
            } else throw new ArgumentException(URI_NOT_VALID);
        }

        private bool CheckIdValidity(string imdbId){
            return imdbId.Contains("imdb.com") && Regex.IsMatch(imdbId, @"tt\d+");
        }

        private string ParseImdbUriString(string imdbUri) {
            //TODO: Parse uri string for imdb id
        }

        private string ParseImdbUri(Uri imdbUri) {
            //TODO: convert uri to string and call ParseImdbUriString
        }
        #endregion
    }
}
