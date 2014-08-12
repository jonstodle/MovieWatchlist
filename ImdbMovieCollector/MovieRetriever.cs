using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.Web.Http;

namespace ImdbMovieCollector {
    public class MovieRetriever {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";

        private string pageHtml;

        public string ImdbId { get; private set; }
        public Uri ImdbUri { get { return new Uri(IMDB_BASE_URI + ImdbId); } }

        #region Constructors
        public MovieRetriever(string imdbId) {
            SetId(imdbId);
        }

        //public MovieRetriever(string imdbUri) {
        //    if(imdbUri == null) throw new ArgumentNullException(URI_NOT_VALID);
        //    string imdbId = null;
        //    imdbId = ParseImdbUriString(imdbUri);
        //    if(imdbId != null) SetId(imdbId);
        //    else throw new ArgumentException();
        //}

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
            return Regex.IsMatch(imdbId, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
        }

        private string ParseImdbUriString(string imdbUri) {
            var match = Regex.Match(imdbUri, IMDB_ID_REGEX, RegexOptions.IgnoreCase);
            if(match.Success) return match.Value;
            else throw new ArgumentException(ID_NOT_VALID);
        }

        private string ParseImdbUri(Uri imdbUri) {
            return ParseImdbUriString(imdbUri.ToString());
        }
        #endregion

        private async Task GetMoviePageHtml() {
            pageHtml = await new HttpClient().GetStringAsync(ImdbUri);
        }

        public async Task Test() {
            await GetMoviePageHtml();
        }
        public string Html { get { return pageHtml; } }
    }
}
