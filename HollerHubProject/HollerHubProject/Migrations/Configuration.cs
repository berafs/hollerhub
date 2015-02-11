namespace HollerHubProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HollerHubProject.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HollerHubProject.Models.HollerHubProjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HollerHubProject.Models.HollerHubProjectContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Reviews.AddOrUpdate(
              p => p.Id,
              new Review { Id = 1, Title = "Fun Repo", RatingStars = 7.5, RepoId = 29078997, ReviewerAlias="kuhlenh", Text="lorem ipsum dolor!" },
              new Review { Id = 2, Title = "Best Repo Ever", RatingStars = 7.5, RepoId = 29078997, ReviewerAlias = "brafs", Text = "lorem ipsum dolor!!" },
              new Review { Id = 3, Title = "Whee! Code", RatingStars = 7.5, RepoId = 29078997, ReviewerAlias = "dyhi", Text = "lorem ipsum dolor!!!" }
            );
            
        }
    }
}
