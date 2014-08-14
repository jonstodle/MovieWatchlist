using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.Web.Http;
using Windows.Data.Html;
using HtmlAgilityPack;

namespace ImdbInterface {
    public class MovieDetailsRetriever {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";

        private HtmlDocument moviePage;
        private HtmlNode overviewTop;
        private HtmlNode titleRecommendations;

        public Movie OriginMovie { get; private set; }
        public Uri ImdbUri { get { return new Uri(IMDB_BASE_URI + OriginMovie.ImdbId); } }

        #region Constructors
        public MovieDetailsRetriever(Movie movie) {
            if(movie == null) throw new ArgumentNullException();
            if(string.IsNullOrWhiteSpace(movie.ImdbId)) throw new ArgumentException(ID_NOT_VALID);
            OriginMovie = movie;
        }
        #endregion

        public async Task FetchMovieDetails() {
            await GetMoviePageHtml();
        }

        private async Task GetMoviePageHtml() {
            moviePage = new HtmlDocument();
            try {
                moviePage.LoadHtml(await new HttpClient().GetStringAsync(ImdbUri));
            } catch(Exception) {
                throw new ArgumentException(ID_NOT_VALID);
            }
        }

        #region Movie Overview
        private HtmlNode GetMovieOverview() {
            if(overviewTop == null) overviewTop = moviePage.GetElementbyId("overview-top");
            return overviewTop;
        }

        public void SetTitle() {
            string title = "";
            try {
                var ot = GetMovieOverview();
                var headerNode = ot.Descendants("h1").First();
                title = headerNode.Descendants("span").First().InnerText;
            } catch(Exception) { return; }
            OriginMovie.Title = title;
        }

        public void SetReleaseYear() {
            int releaseYear = 0;
            try {
                var ot = GetMovieOverview();
                var headerNode = ot.Descendants("h1").First();
                var yearNode = headerNode.Descendants("a").First();
                releaseYear = int.Parse(Regex.Match(yearNode.InnerText, @"\b\d+\b").Value);
            } catch(Exception) { return; }
            OriginMovie.ReleaseYear = releaseYear;
        }

        public List<string> GetContentRatings() {
            List<string> contentRatings = new List<string>();
            try {
                var ot = GetMovieOverview();
                var ratings = ot.Descendants().Where(n => n.GetAttributeValue("itemprop", "") == "contentRating");
                contentRatings = (List<string>)ratings.Select(n => n.Attributes["content"].Value);
            } catch(Exception) {}
            return contentRatings;
        }

        public int GetDuration() {
            int duration = 0;
            try {
                var ot = GetMovieOverview();
                var timeElement = ot.Descendants("time").First();
                var teText = timeElement.InnerText;
                duration = int.Parse(Regex.Match(teText, @"\b\d+\b").Value);
            } catch(Exception) {}
            return duration;
        }

        public List<string> GetGenres() {
            List<string> genres = new List<string>();
            try {
                var ot = GetMovieOverview();
                var infoBar = ot.Descendants("div").Where(n => n.GetAttributeValue("class", "") == "infobar").First();
                var genreNodes = infoBar.Descendants("span").Where(n => n.GetAttributeValue("itemprop", "") == "genre");
                genres = (List<string>)genreNodes.Select(n => n.InnerText);
            } catch(Exception) {}
            return genres;
        }

        public List<DateLocation> GetReleaseDates() {
            List<DateLocation> releaseDates = new List<DateLocation>();
            try {
                //TODO: implement DateLocation properly
            } catch(Exception) {}
            return releaseDates;
        }

        public Dictionary<string, int> GetRatingScores() {
            Dictionary<string, int> ratingScores = new Dictionary<string, int>();
            try {
                //TODO: implement RatingScore struct
            } catch(Exception) {}
            return ratingScores;
        }

        public string GetDescription() {
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

        public HtmlNode GetTitleRecommendations() {
            if(titleRecommendations == null) titleRecommendations = moviePage.GetElementbyId("titleRecs");
            return titleRecommendations;
        }

        public async Task Test() {
            await GetMoviePageHtml();
        }
        public string Html { get { return pageHtml; } }
    }
}
