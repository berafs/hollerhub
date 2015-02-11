using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Octokit;

namespace HubAPi.Models
{
    public class RepoPageInfo
    {
        public Repository repo { get; set; }
        public IEnumerable<RepositoryContributor> contributors { get; set; }

        public RepoPageInfo(Repository r, IEnumerable<RepositoryContributor> c)
        {
            repo = r;
            contributors = c;
        }
    }
}