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

        public Contributor(int id, string login, string url, int contrib, string avatar)
        {
            Id = id;
            Login = login;
            ProfileUrl = url;
            NumberContributions = contrib;
            AvatarUrl = avatar;
        }
    }
}