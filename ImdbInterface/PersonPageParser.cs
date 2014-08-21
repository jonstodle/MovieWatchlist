using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace ImdbInterface {
    class PersonPageParser {
        private const string IMDB_BASE_URI = "http://www.imdb.com/title/";
        private const string URI_NOT_VALID = "Not a valid IMDb uri";
        private const string ID_NOT_VALID = "Not a valdi IMDb ID";
        private const string IMDB_ID_REGEX = @"\btt\d+\b";

        private HtmlDocument personPage;

        public Person OriginPerson { get; private set; }
        public Uri ImdbUri { get { return new Uri(IMDB_BASE_URI + OriginPerson.ImdbId); } }

        #region Constructors
        public PersonPageParser(Person person) {
            if(person == null) throw new ArgumentNullException();
            if(string.IsNullOrWhiteSpace(person.ImdbId)) throw new ArgumentException(ID_NOT_VALID);
            OriginPerson = person;
        }
        #endregion

        public async Task FetchPersonDetails() {
            await GetPersonPageHtml();
        }

        private async Task GetPersonPageHtml() {
            personPage = new HtmlDocument();
            try {
                personPage.LoadHtml(await new HttpClient().GetStringAsync(ImdbUri));
            } catch(Exception) {
                throw new ArgumentException(ID_NOT_VALID);
            }
        }

        #region Poster
        HtmlNode imgPrimary;

        private HtmlNode GetImgPrimary() {
            if(imgPrimary == null) personPage.GetElementbyId("img_primary");
            return imgPrimary;
        }

        public Uri GetPosterUri() {
            Uri posterUri = new Uri("");
            try {
                var ip = GetImgPrimary();
                posterUri = new Uri(ip.Descendants("img").First().GetAttributeValue("src", ""));
            } catch(Exception) {}
            return posterUri;
        }
        #endregion

        #region Person Overview
        HtmlNode overviewTop;

        private HtmlNode GetOverviewTop() {
            if(overviewTop == null) personPage.GetElementbyId("overview-top");
            return overviewTop;
        }

        public string GetName() {
            string name = "";
            try {
                var ot = GetOverviewTop();
                var headerNode = ot.Descendants("h1").First();
                name = headerNode.Descendants("span").First().InnerText;
            } catch(Exception) {}
            return name;
        }

        public List<string> GetJobTitles() {
            List<string> jobTitles = new List<string>();
            try {
                var infoBarNode = personPage.GetElementbyId("name-job-categories");
                var jobTitleNodes = infoBarNode.Descendants("span").Where(n => n.GetAttributeValue("itemprop", "").Contains("jobTitle"));
                jobTitles = (List<string>)jobTitleNodes.Select(n => n.InnerText);
            } catch(Exception) {}
            return jobTitles;
        }

        public string GetDescription() {
            string description = "";
            try {
                var bioNode = personPage.GetElementbyId("name-trivia-bio-text");
                bioNode.Descendants("span").First().Remove();
                description = bioNode.InnerText;
            } catch(Exception) {}
            return description;
        }

        public DateTime GetBirthDate() {
            DateTime birthDate = new DateTime();
            try {
                var birthInfoNode = personPage.GetElementbyId("name-born-info");
                var dateArray = birthInfoNode.Descendants("time").First().GetAttributeValue("datetime", "").Split('-');
                birthDate = new DateTime(int.Parse(dateArray[0]), int.Parse(dateArray[1]), int.Parse(dateArray[2]));
            } catch(Exception) {}
            return birthDate;
        }

        public string GetBirthPlace() {
            string birthPlace = "";
            try {
                var birthInfoNode = personPage.GetElementbyId("name-born-info");
                var linkNodes = birthInfoNode.Descendants("a");
                birthPlace = linkNodes.Last().InnerText;
            } catch(Exception) {}
            return birthPlace;
        }

        public List<Movie> GetKnownFor() {
            List<Movie> knownFor = new List<Movie>();
            try {
                var knownForNode = personPage.GetElementbyId("knownfor");
                foreach(var m in knownForNode.ChildNodes) {
                    var uriString = m.Descendants("a").First().GetAttributeValue("href", "");
                    var title = m.Descendants("img").First().GetAttributeValue("title", "");
                    var thumbnailUriString = m.Descendants("img").First().GetAttributeValue("src", "");
                    knownFor.Add(new Movie(new Uri(uriString)) { Title = title, PosterThumbnailUri = new Uri(thumbnailUriString) });
                }
            } catch(Exception) {}
            return knownFor;
        }


        #endregion
    }
}
