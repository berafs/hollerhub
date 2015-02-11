using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Octokit;
using System.Threading.Tasks;

namespace HollerHubProject.Controllers
{
    public class GitHubController : ApiController
    {
        const string clientId = "1c230d149965dc938e05";
        const string clientSecret = "506ff51bd78e776444a33b2b3d63049fe4118063";
        readonly GitHubClient client = new GitHubClient(new ProductHeaderValue("HollerHub"));
        



        //Given name of repo, return the repo
        //Arguments: owner/name
        [HttpGet]
        [Route("~/repo/{owner}/{name}")]
        public async Task<IHttpActionResult> GetRepoInfo(string owner, string name)
        {
            var url = string.Format("/repo/{0}/{1}?=client_id={2}&client_secret={3}", owner, name, clientId, clientSecret);
            var client = new HttpClient() { BaseAddress = new Uri("https://api.github.com") };
            var resp = await client.GetAsync(url);
            return Ok(resp.Content);
        }

    }
}
