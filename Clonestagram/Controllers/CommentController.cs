using Clonestagram.Models;
using Microsoft.AspNet.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clonestagram.Controllers
{
    public class CommentController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Comment
        public ActionResult Index(int id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IEnumerable<Comment> allComments = context.Comments.Where(p => p.PostId == id).OrderByDescending(p => p.date).ToList();
                return View(allComments);
            }

        } 

        [HttpGet]
        public ActionResult NewComment(int id)
        {
            
            Comment comment = new Comment();
            comment.PostId = id;
            return View(comment);
        }

        [HttpPost]
        public ActionResult NewComment(Comment comment)
        {
            Comment commentForDb = new Comment();
            commentForDb.ApplicationUserId = User.Identity.GetUserId();
            commentForDb.ApplicationUserName = User.Identity.GetUserName();
            commentForDb.CommentContent = comment.CommentContent;
            commentForDb.PostId = comment.PostId;
            commentForDb.date = DateTime.Now;


            try
            {
                db.Comments.Add(commentForDb);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.WasPostingSuccess = false;
                if (ModelState.IsValid)
                {
                    return View(comment);
                }
                else
                {
                    return View();
                }


            }
            ViewBag.WasPostingSuccess = true;
            if (ModelState.IsValid)
            {
                return View(comment);
            }
            else
            {
                return View();
            }

        }


    }
}
