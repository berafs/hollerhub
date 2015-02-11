using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.IO;

namespace HollerHubProject.Controllers
{
    public class GitHubController : ApiController
    {        
        private Uri gitHubBaseUri = new Uri("https://api.github.com");
        const string repoFmt = "/repos/{0}/{1}";
        const string contribFmt = "/repos/{0}/{1}/contributors";
        const string userFmt = "/users/{0}";
        const string userRepoFmt = "/users/{0}/repos";

        const string auth = "?client_id=1c230d149965dc938e05&client_secret=506ff51bd78e776444a33b2b3d63049fe4118063";

        //Given name of repo, return the repo
        //Arguments: owner/name
        [HttpGet]
        [Route("~/repo/{owner}/{name}")]
        public IHttpActionResult GetRepoInfo(string owner, string name)
        {
            var client = new HttpClient() { BaseAddress = gitHubBaseUri };
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Fiddler");
            client.DefaultRequestHeaders.Remove("Connection");

            //create object repo to populate with necessary json data
            var repo = new Repo();
            //get the repo json and parse it
            var repoContent = client.GetStringAsync(makeRepoRequestUri(owner, name));
            var repoResult = repoContent.Result;
            JToken repoToken = JObject.Parse(repoResult);
            //populate the repo object
            repo.Name = (string)repoToken.SelectToken("full_name");
            repo.Id = (int)repoToken.SelectToken("id");
            repo.NumberForks = (int)repoToken.SelectToken("forks");
            repo.NumberStargazers = (int)repoToken.SelectToken("stargazers_count");
            repo.NumberSubscribers = (int)repoToken.SelectToken("subscribers_count");
            repo.AvatarUrl = (string)repoToken.SelectToken("owner").SelectToken("avatar_url");
            repo.HasWiki = (bool)repoToken.SelectToken("has_wiki");
            repo.RepoUrl = (string)repoToken.SelectToken("url");
            repo.Description = (string)repoToken.SelectToken("description");

            //get the contributors associated with the repo and parse it
            var contribContent = client.GetStringAsync(makeContribRequestUri(owner, name));
            var contribResult = contribContent.Result;
            dynamic dynObj = JsonConvert.DeserializeObject(contribResult);
            //list of contributor objects
            List<Contributor> contributors = new List<Contributor>();
            //loop through each contributor in json and make object
            foreach (var t in dynObj)
            {
                contributors.Add(new Contributor((int)t.SelectToken("id"), (string)t.SelectToken("login"), (string)t.SelectToken("url"), (int)t.SelectToken("contributions"), (string)t.SelectToken("avatar_url")));

            }
            //add contributors to repo object
            repo.Contributors = contributors;
            //return json of repo object
            return Json(repo);
        }

        private string makeContribRequestUri(string owner, string name)
        {
            return string.Format(contribFmt, owner, name) + auth;
        }

        private string makeRepoRequestUri(string owner, string name)
        {
            return string.Format(repoFmt, owner, name) + auth;
        }

        //Given name of a user, return the info and repos
        //Arguments: github alias
        [HttpGet]
        [Route("~/user/{alias}")]
        public IHttpActionResult GetUserInfo(string alias)
        {
            var client = new HttpClient() { BaseAddress = gitHubBaseUri };
            client.DefaultRequestHeaders.Add("User-Agent", "Fiddler");
            client.DefaultRequestHeaders.Remove("Connection");

            //create object repo to populate with necessary json data
            var user = new Contributor();
            //get the user json and parse it
            var userContent = client.GetStringAsync(makeUserRequestUri(alias));
            var userResult = userContent.Result;
            JToken uToken = JObject.Parse(userResult);
            //populate the contributor object
            user.Id = (int)uToken.SelectToken("id");
            user.Name = (string)uToken.SelectToken("name") != null ? (string)uToken.SelectToken("name") : "";
            user.Login = (string)uToken.SelectToken("login");
            user.Company = (string)uToken.SelectToken("company") != null ? (string)uToken.SelectToken("company") : "";
            user.StartDate = (string)uToken.SelectToken("created_at");
            user.NumberFollowers = (int)uToken.SelectToken("followers");
            user.AvatarUrl = (string)uToken.SelectToken("avatar_url");
            user.ProfileUrl = (string)uToken.SelectToken("url");

            //get the contributors associated with the repo and parse it
            var repoContent = client.GetStringAsync(makeUserRepoRequestUri(alias));
            var repoResult = repoContent.Result;
            dynamic dynObj = JsonConvert.DeserializeObject(repoResult);
            //list of contributor objects
            List<Repo> repos = new List<Repo>();
            //loop through each contributor in json and make object
            foreach (var t in dynObj)
            {
                var r = new Repo();
                r.Name = (string)t.SelectToken("full_name");
                r.Id = (int)t.SelectToken("id");
                r.NumberForks = (int)t.SelectToken("forks");
                r.NumberStargazers = (int)t.SelectToken("stargazers_count");
                r.AvatarUrl = (string)t.SelectToken("owner").SelectToken("avatar_url");
                r.HasWiki = (bool)t.SelectToken("has_wiki");
                r.RepoUrl = (string)t.SelectToken("url");
                repos.Add(r);
            }
            //add repos to contributor object
            user.Repos = repos;
            //return json of repo object
            return Json(user);
        }

        private string makeUserRequestUri(string alias)
        {
            return string.Format(userFmt, alias) + auth;
        }
        private string makeUserRepoRequestUri(string alias)
        {
            return string.Format(userRepoFmt, alias) + auth;
        }



    }
}
