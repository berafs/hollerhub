using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HollerHubProject.Models
{
    public class Review
    {
        [Required] 
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Text { get; set; }
        public string ReviewerAlias { get; set; }
        public double RatingStars { get; set; }
        public int RepoId { get; set; }

        public Review(int id, string title, string text, string alias, double stars, int repoid)
        {
            Id = id;
            Title = title;
            Text = text;
            ReviewerAlias = alias;
            RatingStars = stars;
            RepoId = repoid;
        }

        public Review()
        {

        }
    }
}
