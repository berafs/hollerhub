using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HollerHubProject.Models;
using Newtonsoft.Json.Linq;

namespace HollerHubProject.Controllers
{
    public class ReviewsController : ApiController
    {
        private HollerHubProjectContext db = new HollerHubProjectContext();
        private Uri gitHubBaseUri = new Uri("https://api.github.com");
        const string repoFmt = "/repos/{0}/{1}";
        const string contribFmt = "/repos/{0}/{1}/contributors";
        const string userFmt = "/users/{0}";
        const string userRepoFmt = "/users/{0}/repos";

        const string auth = "?client_id=1c230d149965dc938e05&client_secret=506ff51bd78e776444a33b2b3d63049fe4118063";


        // GET: api/Reviews
        public IQueryable<Review> GetReviews()
        {
            return db.Reviews;
        }

        //GET: api/Reviews/5
        //returns user review for specified repo
        [HttpGet]
        [Route("~/api/Reviews/{alias}/{id}")]
        [ResponseType(typeof(Review))]
        public IQueryable<Review> GetReviews(string alias, int id)
        {
            return db.Reviews.Where(q => q.ReviewerAlias==alias).Where(q=>q.RepoId == id);
        }

        //GET: api/Reviews/5
        //returns reviews based on the given repo id
        [ResponseType(typeof(Review))]
        [HttpGet]
        [Route("~/api/Reviews/{id}")]
        public IQueryable<Review> GetReviews(int id)
        {
            return db.Reviews.Where(q => q.RepoId == id);
        }


        [HttpGet]
        [Route("~/api/MetaRating/{owner}/{name}")]
        public double GetMetaRating(string owner, string name)
        {
            var client = new HttpClient() { BaseAddress = gitHubBaseUri };
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Fiddler");
            client.DefaultRequestHeaders.Remove("Connection");

            //get the repo json and parse it
            var repoContent = client.GetStringAsync(string.Format(repoFmt, owner, name) + auth);
            var repoResult = repoContent.Result;
            JToken repoToken = JObject.Parse(repoResult);
            //populate the repo object
            var id = (int)repoToken.SelectToken("id");
            var forks = (int)repoToken.SelectToken("forks");
            var stargazers= (int)repoToken.SelectToken("stargazers_count");
            var subscribers = (int)repoToken.SelectToken("subscribers_count");
            var wiki = (bool)repoToken.SelectToken("has_wiki");

            var total = 0.0;

            var forkStarRatio = forks / (double)stargazers;
            var forksSubRatio = forks / (double)subscribers;
            var hasWiki = wiki ? 1 : 0;
            var sum = (forks + stargazers + subscribers)/100000.0 * 2.0;

            if (forkStarRatio > .25)
                total += 4;
            else if (forkStarRatio >= .20)
                total += 3;
            else if (forkStarRatio >= .1)
                total += 2;
            else
                total += 1;

            if (forksSubRatio > 1.5)
                total += 3;
            else if (forksSubRatio >= 1)
                total += 2;
            else
                total += 1;

            total += hasWiki;
            total += sum;

            return total;
        }



        //// GET: api/Reviews/5
        //[ResponseType(typeof(Review))]
        //public IHttpActionResult GetReview(int id)
        //{
        //    Review review = db.Reviews.Find(id);

        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(review);
        //}

        // PUT: api/Reviews/5
        [ResponseType(typeof(void))]
        [Route("~/api/reviews/{id}")]
        public IHttpActionResult PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.Id)
            {
                return BadRequest();
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reviews
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = review.Id }, review);
        }

        // DELETE: api/Reviews/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return db.Reviews.Count(e => e.Id == id) > 0;
        }
    }
}