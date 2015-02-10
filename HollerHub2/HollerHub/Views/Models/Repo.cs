using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HollerHub.Models
{
    public class Repo
    {
        public int RepoId { get; set; }
        public string Name { get; set; }
        public string Self
        {
            get { return $"api/repos/{this.RepoId}"; }
            set {  }
        }
    }
}