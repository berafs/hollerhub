using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Octokit;
using HubAPi.Models;
using HubApi.Models;
using System.Web.Security;

namespace HubApi.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<Repository> repositories, ActionResult json)
        {
            Repositories = repositories;
            Json = json;
        }

        public IEnumerable<Repository> Repositories { get; private set; }
        public ActionResult Json { get; set; }
    }
}