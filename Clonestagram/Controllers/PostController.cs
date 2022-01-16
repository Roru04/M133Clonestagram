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

        [Authorize]
        // GET: Post
        public ActionResult Index(bool? hasPosted = null)
        {
            ViewBag.WasPostingSuccess = hasPosted;
            Post post = new Post();
            return View(post);

        }
        [HttpPost]
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
        public ActionResult ShowPosts() 
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Post> allPosts = context.Posts.OrderByDescending(p => p.date).ToList();
                return View(allPosts);
            }
        }

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

        [HttpGet]
        public ActionResult SortLikes()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Post> allPosts = context.Posts.OrderByDescending(p => p.Likes).ToList();
                return View("SortedPosts",allPosts);
            }
        }


        [Authorize(Roles = nameof(Role.Administrator))]
        public ActionResult DeletePost(int id)
        {
            Post deletable = db.Posts.First(p => p.Id == id);

            db.Posts.Remove(deletable);



            db.SaveChanges();

            return RedirectToAction("ShowPosts");
        }

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