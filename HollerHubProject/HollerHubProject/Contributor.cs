using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HollerHubProject
{
    public class Contributor
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string ProfileUrl { get; set; }
        public int NumberContributions { get; set; }
        public string AvatarUrl { get; set; }

        public string Company { get; set; }
        public List<Repo> Repos { get; set; }
        public int NumberFollowers { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }

        public Contributor(int id, string login, string url, int contrib, string avatar)
        {
            Id = id;
            Login = login;
            ProfileUrl = url;
            NumberContributions = contrib;
            AvatarUrl = avatar;
        }

        public Contributor(int id, string login, string url, string avatar, string company, List<Repo> repos, int followers, string name, string date)
        {
            Id = id;
            Login = login;
            ProfileUrl = url;
            AvatarUrl = avatar;
            Company = company;
            Repos = repos;
            NumberFollowers = followers;
            Name = name;
            StartDate = date;
        }

        public Contributor()
        {

        }
    }
}