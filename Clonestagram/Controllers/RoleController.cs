using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clonestagram.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Clonestagram.Controllers
{
    [Authorize(Roles = nameof(Role.Administrator))]
    public class RoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Role
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
                        userFromDb.Roles.Add(userRole);
                        userFromDb.Roles.Add(userRole);
                        userFromDb.Roles.Add(userRole);
                        
                    }
                    

                }

                db.SaveChanges();
            }


            return RedirectToAction("Index", "Home");
        }
    }
}