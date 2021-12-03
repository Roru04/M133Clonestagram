using Clonestagram.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Clonestagram.Controllers
{
    public class PostController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

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
                ViewBag.WasPostingSuccess = false;
                return RedirectToAction("Index", new { hasPosted = false });
            }
            return RedirectToAction("Index", new { hasPosted = true });
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