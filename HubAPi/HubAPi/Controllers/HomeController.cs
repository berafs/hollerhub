﻿﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Octokit;
using HubApi.Models;
using HubAPi.Models;
using System.Collections.Generic;

namespace HubApi.Controllers
{
    public class HomeController : Controller
    {
        // TODO: Replace the following values with the values from your application registration. Register an
        // application at https://github.com/settings/applications/new to get these values.
        const string clientId = "1c230d149965dc938e05";
        private const string clientSecret = "506ff51bd78e776444a33b2b3d63049fe4118063";
        readonly GitHubClient client =
            new GitHubClient(new ProductHeaderValue("Demo"));

        // This URL uses the GitHub API to get a list of the current user's
        // repositories which include public and private repositories.
        public async Task<ActionResult> Index()
        {
            var accessToken = Session["OAuthToken"] as string;
            if (accessToken != null)
            {
                // This allows the client to make requests to the GitHub API on the user's behalf
                // without ever having the user's OAuth credentials.
                client.Credentials = new Credentials(accessToken);
            }

            try
            {
                // The following requests retrieves all of the user's repositories and
                // requires that the user be logged in to work.
                var repositories = await client.Repository.GetAllForCurrent();
                var result = await GetRepoInfo();
                var r = Json(result);
                var model = new IndexViewModel(repositories, r);
                return View(model);
            }
            catch (AuthorizationException)
            {
                // Either the accessToken is null or it's invalid. This redirects
                // to the GitHub OAuth login page. That page will redirect back to the
                // Authorize action.
                return Redirect(GetOauthLoginUrl());
            }
        }

        // This is the Callback URL that the GitHub OAuth Login page will redirect back to.
        public async Task<ActionResult> Authorize(string code, string state)
        {
            if (!String.IsNullOrEmpty(code))
            {
                var expectedState = Session["CSRF:State"] as string;
                if (state != expectedState) throw new InvalidOperationException("SECURITY FAIL!");
                Session["CSRF:State"] = null;

                var token = await client.Oauth.CreateAccessToken(
                    new OauthTokenRequest(clientId, clientSecret, code));
                Session["OAuthToken"] = token.AccessToken;
            }

            return RedirectToAction("Index");
        }

        private string GetOauthLoginUrl()
        {
            string csrf = Membership.GeneratePassword(24, 1);
            Session["CSRF:State"] = csrf;

            // 1. Redirect users to request GitHub access
            var request = new OauthLoginRequest(clientId)
            {
                Scopes = { "user", "notifications" },
                State = csrf
            };
            var oauthLoginUrl = client.Oauth.GetGitHubLoginUrl(request);
            return oauthLoginUrl.ToString();
        }

        public async Task<ActionResult> Emojis()
        {
            var emojis = await client.Miscellaneous.GetEmojis();

            return View(emojis);
        }

        public async Task<ActionResult> GetRepoInfo()
        {
            var repositories = await client.Repository.GetAllForCurrent();
            List<RepoPageInfo> info = new List<RepoPageInfo>();
            foreach (var r in repositories)
            {
                if (r.FullName != null)
                {
                    var reponame = r.FullName.Split('/');
                    if (reponame.Length > 1)
                    {
                        var contribs = await client.Repository.GetAllContributors(reponame[0], reponame[1], true);
                        if (contribs.Count > 0)
                        {
                            info.Add(new RepoPageInfo(r, contribs));
                        }
                        else
                        {
                            info.Add(new RepoPageInfo(r, null));
                        }
                    }
                    
                }
                
            }

            return Json(info, JsonRequestBehavior.AllowGet);
        } 
    }
}