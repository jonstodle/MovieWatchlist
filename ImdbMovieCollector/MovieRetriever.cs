using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.Web.Http;
using Windows.Data.Html;
using HtmlAgilityPack;

namespace ImdbMovieCollector {
    public class MovieRetriever {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";

        private HtmlDocument moviePage;
        private HtmlNode overviewTop;
        private HtmlNode titleRecommendations;

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
            moviePage = new HtmlDocument();
            moviePage.LoadHtml(await new HttpClient().GetStringAsync(ImdbUri));
        }

        #region Movie Overview
        private HtmlNode GetMovieOverview() {
            if(overviewTop == null) overviewTop = moviePage.GetElementbyId("overview-top");
            return overviewTop;
        }

        private string GetTitle() {
            string title = "";
            try {
                var ot = GetMovieOverview();
                var headerNode = ot.Descendants("h1").First();
                title = headerNode.Descendants("span").First().InnerText;
            } catch(Exception) {}
            return title;
        }

        private int GetReleaseYear() {
            int releaseYear = 0;
            try {
                var ot = GetMovieOverview();
                var headerNode = ot.Descendants("h1").First();
                var yearNode = headerNode.Descendants("a").First();
                releaseYear = int.Parse(Regex.Match(yearNode.InnerText, @"\b\d+\b").Value);
            } catch(Exception) {}
            return releaseYear;
        }

        private List<string> GetContentRatings() {
            List<string> contentRatings = new List<string>();
            try {
                var ot = GetMovieOverview();
                var ratings = ot.Descendants().Where(n => n.GetAttributeValue("itemprop", "") == "contentRating");
                contentRatings = (List<string>)ratings.Select(n => n.Attributes["content"].Value);
            } catch(Exception) {}
            return contentRatings;
        }

        private int GetDuration() {
            int duration = 0;
            try {
                var ot = GetMovieOverview();
                var timeElement = ot.Descendants("time").First();
                var teText = timeElement.InnerText;
                duration = int.Parse(Regex.Match(teText, @"\b\d+\b").Value);
            } catch(Exception) {}
            return duration;
        }

        private List<string> GetGenres() {
            List<string> genres = new List<string>();
            try {
                var ot = GetMovieOverview();
                var infoBar = ot.Descendants("div").Where(n => n.GetAttributeValue("class", "") == "infobar").First();
                var genreNodes = infoBar.Descendants("span").Where(n => n.GetAttributeValue("itemprop", "") == "genre");
                genres = (List<string>)genreNodes.Select(n => n.InnerText);
            } catch(Exception) {}
            return genres;
        }

        private List<DateLocation> GetReleaseDates() {
            List<DateLocation> releaseDates = new List<DateLocation>();
            try {
                //TODO: implement DateLocation properly
            } catch(Exception) {}
            return releaseDates;
        }

        private Dictionary<string, int> GetRatingScores() {
            Dictionary<string, int> ratingScores = new Dictionary<string, int>();
            try {
                //TODO: implement RatingScore struct
            } catch(Exception) {}
            return ratingScores;
        }

        private string GetDescription() {
            string description = "";
            try {
                var ot = GetMovieOverview();
                var descriptionNode = ot.Descendants("p").Where(n => n.GetAttributeValue("itemprop", "") == "description").First();
                description = descriptionNode.InnerText;
            } catch(Exception) {}
            return description;
        }

        /* TODO: Redo these to use Person class
        private List<Person> GetDirectors() {
            List<Person> directors = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var directorNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("director")).First();
                directors = (List<Person>)directorNode.Descendants("a").Where(n=>n.GetAttributeValue("href","").Contains("name")).Select(n => n.InnerText);
            } catch(Exception) {}
            return directors;
        }

        private List<Person> GetWriters() {
            List<Person> writers = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var writerNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("creator")).First();
                writers = (List<Person>)writerNode.Descendants("a").Where(n => n.GetAttributeValue("href", "").Contains("name")).Select(n => n.InnerText);
            } catch(Exception) {}
            return writers;
        }

        private List<Person> GetStars() {
            List<Person> stars = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var starNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("actor")).First();
                stars = (List<Person>)starNode.Descendants("a").Where(n => n.GetAttributeValue("href", "").Contains("name")).Select(n => n.InnerText);
            } catch(Exception) {}
            return stars;
        }
        */
        #endregion

        private HtmlNode GetTitleRecommendations() {
            if(titleRecommendations == null) titleRecommendations = moviePage.GetElementbyId("titleRecs");
            return titleRecommendations;
        }

        public async Task Test() {
            await GetMoviePageHtml();
        }
        public string Html { get { return pageHtml; } }
    }
}
