using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HollerHubProject
{
    public class Repo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberForks { get; set; }
        public int NumberStargazers { get; set; }
        public int NumberSubscribers { get; set; }
        public string AvatarUrl { get; set; }
        public bool HasWiki { get; set; }
        public List<Contributor> Contributors { get; set; }
        public string RepoUrl { get; set; }
        public string Description { get; set; }

        public Repo(int id, string name, int forks, int stargazers, int subscribers, string avatar, bool wiki, List<Contributor> contrib, string url, string descrip)
        {
            Id = id;
            Name = name;
            NumberForks = forks;
            NumberStargazers = stargazers;
            NumberSubscribers = subscribers;
            AvatarUrl = avatar;
            HasWiki = wiki;
            Contributors = contrib;
            RepoUrl = url;
            Description = descrip;
        }

        public Repo()
        {

        }

    }
}