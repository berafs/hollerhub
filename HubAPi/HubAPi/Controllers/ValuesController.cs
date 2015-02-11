using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Octokit;
using HubAPi.Models;

namespace HubAPi.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetRepoInfo()
        {

            const string clientId = "c6bd1ef2ce2b770922ce";
            const string clientSecret = "e44ac7a893c76ca20e85d2ff7a414f199e39dcef";
            var oauthTokenRequest = new OauthTokenRequest(clientId, clientSecret, "200");
            var client = new GitHubClient(new ProductHeaderValue("Hollerhub"));
            var token = await client.Oauth.CreateAccessToken(oauthTokenRequest);
            var credentials = new Credentials(token.AccessToken);
            client.Credentials = credentials;

            var currentUser = await client.User.Current();
                  
            
            
            var repositories = await client.Repository.GetAllForCurrent();
            List<RepoPageInfo> info = new List<RepoPageInfo>();
            foreach (var r in repositories)
            {
                var contribs = await client.Repository.GetAllContributors(r.Owner.Name, r.FullName, true);
                info.Add(new RepoPageInfo(r, contribs));
            }

            return Json(repositories);
        } 
    }
    
}
