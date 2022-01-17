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
        log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// You can see ale Comments
        /// </summary>
        /// <param name="id">id of Post</param>
        /// <returns></returns>
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

        /// <summary>
        /// Site for makeing a new Comment
        /// </summary>
        /// <param name="id">Id of Post</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewComment(int id)
        {
            
            Comment comment = new Comment();
            comment.PostId = id;
            return View(comment);
        }


        /// <summary>
        /// Saves Comment in db
        /// </summary>
        /// <param name="comment">comment which will be saved</param>
        /// <returns></returns>
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
                    _log.Error(ex.Message);
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
                _log.Info($"Post with Id {commentForDb.Id} created form user {User.Identity.GetUserId()}");
                return RedirectToAction("Index", new { id = commentForDb.PostId });
            }
            else
            {
                return View();
            }

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
