using Clonestagram.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Clonestagram.Controllers
{
    public class PostController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //Must be logged in
        [Authorize]
        // GET: Post
        public ActionResult Index(bool? hasPosted = null)
        {
            ViewBag.WasPostingSuccess = hasPosted;
            Post post = new Post();
            return View(post);

        }
        [HttpPost]
        //Save a new post in the db
        public ActionResult Index(Post post)
        {
            Post postForDb = new Post();
            postForDb.Title = post.Title;
            postForDb.Content = post.Content;
            postForDb.date = DateTime.Now;
            postForDb.ApplicationUserId = User.Identity.GetUserId();
            postForDb.ApplicationUserName = User.Identity.GetUserName();

            try
            {
                db.Posts.Add(postForDb);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.Error(ex.Message);
                ViewBag.WasPostingSuccess = false;
                if (ModelState.IsValid)
                {
                    return View(post);
                }
                else
                {
                    return View();
                }
               
                
            }
            ViewBag.WasPostingSuccess = true;
            if (ModelState.IsValid)
            {
                _log.Info($"Post with Id {postForDb.Id} created form user {User.Identity.GetUserId()}");
                return View(post);
            }
            else
            {
                return View();
            }
           
        }

        [Authorize]
        [HttpGet]
        //Displays all posts
        public ActionResult ShowPosts() 
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Post> allPosts = context.Posts.OrderByDescending(p => p.date).ToList();
                return View(allPosts);
            }
        }

        /// <summary>
        /// function to like posts
        /// </summary>
        /// <param name="id">Id of Post</param>
        /// <returns></returns>
        public ActionResult Like(int id)
        {
            try
            {
                Post postfromDb = db.Posts.Find(id);

                postfromDb.Likes++;
                db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                DbEntityEntry entity = ex.Entries.Single();

                entity.Reload();
            }

            return RedirectToAction("ShowPosts");
        }

        /// <summary>
        /// Returns all posts sorted by the number of likes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //displays all likes but sorted by the likes
        public ActionResult SortLikes()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Post> allPosts = context.Posts.OrderByDescending(p => p.Likes).ToList();
                return View("SortedPosts",allPosts);
            }
        }
        

        //only Administrators can call this site
        [Authorize(Roles = nameof(Role.Administrator))]
        /// <summary>
        /// Delete a post
        /// </summary>
        /// <param name="id">Id of the post</param>
        /// <returns></returns>
        public ActionResult DeletePost(int id)
        {
            Post deletable = db.Posts.First(p => p.Id == id);

            db.Posts.Remove(deletable);



            db.SaveChanges();

            return RedirectToAction("ShowPosts");
        }

        /// <summary>
        /// Search for a post
        /// </summary>
        /// <param name="querry">input in the sesarch field</param>
        /// <returns></returns>
        [HttpGet]
        //search for posts
        public ActionResult SearchPost(string querry)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Post> postfromdb = db.Posts.ToList();

                List<Post> postList = new List<Post>();

                foreach(Post post in postfromdb)
                {
                    if(post.Title.Contains(querry) == true)
                    {
                        postList.Add(post);
                    }
                }

                IEnumerable<Post> allPosts = postList;
                     

                return View("ShowPosts", allPosts);
            }
        }

        //close dbConnection
        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }

    

}