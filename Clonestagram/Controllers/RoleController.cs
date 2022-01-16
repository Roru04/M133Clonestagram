using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clonestagram.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Clonestagram.Controllers
{
    //only the admin can go on this Controller
    [Authorize(Roles = nameof(Role.Administrator))]
    public class RoleController : Controller
    {
        log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Role
        //displays all users for the admin
        public ActionResult Index()
        {
            List<ApplicationUser> allUsers = db.Users.ToList();
            List<IdentityRole> allRoles = db.Roles.ToList();
            List<UserViewModel> model = new List<UserViewModel>();

            foreach(ApplicationUser user in allUsers)
            {
                List<RoleViewModel> rolls = new List<RoleViewModel>();
                foreach(IdentityRole identityRole in allRoles)
                {
                    bool hasRole = false;
                    foreach (IdentityUserRole userRole in user.Roles)
                    {
                        if (userRole.RoleId == identityRole.Id)
                        {
                            hasRole = true;
                        }
                    }

                    RoleViewModel role = new RoleViewModel
                    {
                        RoleId = identityRole.Id,
                        HasRole = hasRole
                    };

                    rolls.Add(role);
                }

                UserViewModel aUser = new UserViewModel
                {
                    UserId = user.Id,
                    Username = user.Email,
                    Roles = rolls
                };
                model.Add(aUser);
                
            }
            return View(model);
        }

        [HttpPost]
        //save new roles in the db
        public ActionResult Index(List<UserViewModel> userViewModels)
        {
            foreach (UserViewModel userViewModel in userViewModels)
            {
                ApplicationUser userFromDb = db.Users.Find(userViewModel.UserId);

                userFromDb.Roles.Clear();

                foreach (RoleViewModel roleViewModel in userViewModel.Roles)
                {
                    if (roleViewModel.HasRole)
                    {
                        IdentityRole role = db.Roles.Single(x => x.Id == roleViewModel.RoleId);

                        IdentityUserRole userRole = new IdentityUserRole
                        {
                            RoleId = roleViewModel.RoleId,
                            UserId = userFromDb.Id
                        };


                        userFromDb.Roles.Add(userRole);

                        _log.Info($"{userRole.UserId} got the role {userRole.RoleId} added in Roles");


                    }
                    

                }

                db.SaveChanges();
                
                
            }
            


            return RedirectToAction("Index", "Home");
        }

        //close db connection
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