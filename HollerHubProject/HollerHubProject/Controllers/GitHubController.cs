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

namespace HollerHubProject.Controllers
{
    public class GitHubController : ApiController
    {        
        private Uri gitHubBaseUri = new Uri("https://api.github.com");
        const string repoFmt = "/repos/{0}/{1}";
        const string auth = "?client_id=1c230d149965dc938e05&client_secret=506ff51bd78e776444a33b2b3d63049fe4118063";


        //Given name of repo, return the repo
        //Arguments: owner/name
        [HttpGet]
        [Route("~/repo/{owner}/{name}")]
        public Task<HttpResponseMessage> GetRepoInfo(string owner, string name)
        {
            var client = new HttpClient() { BaseAddress = gitHubBaseUri };
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Fiddler");
            client.DefaultRequestHeaders.Remove("Connection");
            
            var resp = client.GetAsync(makeRepoRequestUri(owner,name));
            return resp;
        }

        private string makeRepoRequestUri(string owner, string name)
        {
            return string.Format(repoFmt, owner, name) + auth;
        }

    }
}
