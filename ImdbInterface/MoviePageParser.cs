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
    public class MoviePageParser {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";

        private HtmlDocument moviePage;

        public Movie OriginMovie { get; private set; }
        public Uri ImdbUri { get { return new Uri(IMDB_BASE_URI + OriginMovie.ImdbId); } }

        #region Constructors
        public MoviePageParser(Movie movie) {
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

        #region Poster
        HtmlNode imgPrimary;

        private HtmlNode GetImgPrimary() {
            if(imgPrimary == null) imgPrimary = moviePage.GetElementbyId("img_primary");
            return imgPrimary;
        }

        public Uri GetPosterUri() {
            Uri posterUri = new Uri("");
            try {
                var ip = GetImgPrimary();
                var uriString = ip.Descendants("img").First().GetAttributeValue("src", "");
                posterUri = new Uri(uriString);
            } catch(Exception) { }
            return posterUri;
        }
        #endregion

        #region Movie Overview
        private HtmlNode overviewTop;

        private HtmlNode GetMovieOverview() {
            if(overviewTop == null) overviewTop = moviePage.GetElementbyId("overview-top");
            return overviewTop;
        }

        public void GetTitle() {
            string title = "";
            try {
                var ot = GetMovieOverview();
                var headerNode = ot.Descendants("h1").First();
                title = headerNode.Descendants("span").First().InnerText;
            } catch(Exception) { return; }
            OriginMovie.Title = title;
        }

        public void GetReleaseYear() {
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
            } catch(Exception) { }
            return contentRatings;
        }

        public int GetDuration() {
            int duration = 0;
            try {
                var ot = GetMovieOverview();
                var timeElement = ot.Descendants("time").First();
                var teText = timeElement.InnerText;
                duration = int.Parse(Regex.Match(teText, @"\b\d+\b").Value);
            } catch(Exception) { }
            return duration;
        }

        public List<string> GetGenres() {
            List<string> genres = new List<string>();
            try {
                var ot = GetMovieOverview();
                var infoBar = ot.Descendants("div").Where(n => n.GetAttributeValue("class", "") == "infobar").First();
                var genreNodes = infoBar.Descendants("span").Where(n => n.GetAttributeValue("itemprop", "") == "genre");
                genres = (List<string>)genreNodes.Select(n => n.InnerText);
            } catch(Exception) { }
            return genres;
        }

        public DateTime GetReleaseDate() {
            DateTime releaseDate = new DateTime();
            try {
                var ot = GetMovieOverview();
                var publishedNodes = ot.Descendants("meta").Where(n => n.GetAttributeValue("itemprop", "").Contains("datePublished"));
                var dateArray = publishedNodes.First().GetAttributeValue("content", "").Split('-');
                releaseDate = new DateTime(int.Parse(dateArray[0]), int.Parse(dateArray[1]), int.Parse(dateArray[2]));
            } catch(Exception) { }
            return releaseDate;
        }

        public Dictionary<string, double> GetRatingScores() {
            Dictionary<string, double> ratingScores = new Dictionary<string, double>();
            try {
                var ot = GetMovieOverview();
                var starBox = ot.Descendants("div").Where(n => n.GetAttributeValue("class", "").Contains("star-box")).First();
                ratingScores["IMDb"] = double.Parse(starBox.Descendants("span").First().GetAttributeValue("itemprop", ""));
                var metaScore = starBox.Descendants("a").Where(n => n.GetAttributeValue("title", "").Contains("metacritic")).First().InnerText;
                var metaScoreArray = metaScore.Split('/');
                ratingScores["MetaCritic"] = double.Parse(metaScoreArray[0]);
            } catch(Exception) { }
            return ratingScores;
        }

        public string GetDescription() {
            string description = "";
            try {
                var ot = GetMovieOverview();
                var descriptionNode = ot.Descendants("p").Where(n => n.GetAttributeValue("itemprop", "") == "description").First();
                description = descriptionNode.InnerText;
            } catch(Exception) { }
            return description;
        }

        private List<Person> GetDirectors() {
            List<Person> directors = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var directorNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("director")).First();
                foreach(var pn in directorNode.Descendants("a")) {
                    var personUriString = pn.GetAttributeValue("href", "");
                    var personName = pn.InnerText;
                    directors.Add(new Person(new Uri(personUriString)) { Name = personName });
                }
            } catch(Exception) { }
            return directors;
        }

        private List<Person> GetWriters() {
            List<Person> writers = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var creatorNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("creator")).First();
                foreach(var pn in creatorNode.Descendants("a")) {
                    var personUriString = pn.GetAttributeValue("href", "");
                    var personName = pn.InnerText;
                    writers.Add(new Person(new Uri(personUriString)) { Name = personName });
                }
            } catch(Exception) { }
            return writers;
        }

        private List<Person> GetStars() {
            List<Person> stars = new List<Person>();
            try {
                var ot = GetMovieOverview();
                var actorNode = ot.Descendants("div").Where(n => n.GetAttributeValue("itemprop", "").Contains("actor")).First();
                foreach(var pn in actorNode.Descendants("a")) {
                    var personUriString = pn.GetAttributeValue("href", "");
                    var personName = pn.InnerText;
                    stars.Add(new Person(new Uri(personUriString)) { Name = personName });
                }
            } catch(Exception) { }
            return stars;
        }
        #endregion

        #region Recommendations
        private HtmlNode titleRecommendations;

        private HtmlNode GetTitleRecommendations() {
            if(titleRecommendations == null) titleRecommendations = moviePage.GetElementbyId("titleRecs");
            return titleRecommendations;
        }

        public List<Movie> GetAlsoLiked() {
            List<Movie> alsoLiked = new List<Movie>();
            try {
                var tr = GetTitleRecommendations();
                var movieNodes = tr.Descendants("div").Where(n => n.GetAttributeValue("class", "").Contains("rec_item"));
                foreach(var mn in movieNodes) {
                    var imdbId = mn.GetAttributeValue("data-tconst", "");
                    var title = mn.GetAttributeValue("title", "");
                    var thumbnailUriString = mn.Descendants("img").First().GetAttributeValue("loadlate", "");
                    alsoLiked.Add(new Movie(imdbId) { Title = title, PosterThumbnailUri = new Uri(thumbnailUriString) });
                }
            } catch(Exception) { }
            return alsoLiked;
        }
        #endregion

        #region Title cast
        private HtmlNode titleCast;

        private HtmlNode GetTitleCast() {
            if(titleCast == null) titleCast = moviePage.GetElementbyId("titleCast");
            return titleCast;
        }

        public Dictionary<string, Person> GetCast() {
            Dictionary<string, Person> cast = new Dictionary<string, Person>();
            try {
                var tc = GetTitleCast();
                var personNodes = tc.Descendants("tr").Where(n => n.GetAttributeValue("class", false));
                foreach(var pn in personNodes) {
                    var imdbUriString = pn.Descendants("a").First().GetAttributeValue("href", "");
                    var thumbnailUriString = pn.Descendants("img").First().GetAttributeValue("loadlate", "");
                    var name = pn.Descendants("span").Where(n => n.GetAttributeValue("itemprop", "").Contains("name")).First().InnerText;
                    var character = pn.Descendants("td").Where(n => n.GetAttributeValue("class", "").Contains("character")).First().InnerText;
                    cast[character] = new Person(new Uri(imdbUriString)) { Name = name, PorttraitUri = new Uri(thumbnailUriString) };
                }
            } catch(Exception) { }
            return cast;
        }
        #endregion
    }
}
