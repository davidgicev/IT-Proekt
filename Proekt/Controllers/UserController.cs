using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Proekt.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private UserDetailsModel getDetails()
        {
            var UserId = User.Identity.GetUserId();
            var userInDb = context.UserDetails
                .Include("Favorites")
                .Include("User")
                .SingleOrDefault(u => u.UserId == UserId);

            return userInDb;
        }
        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            return View(getDetails());
        }

        public ActionResult LoginPartial()
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            return View(getDetails());
        }
        [Authorize]
        [HttpPost]
        public void ChangePicture(string url)
        {
            var userInDb = getDetails();

            userInDb.Picture = url;
            context.SaveChanges();
        }
        [Authorize]
        [HttpGet]
        public ActionResult ChangeDetails()
        {
            var user = getDetails();
            var editModel = new EditDetailedViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Picture = user.Picture
            };
            return View(editModel);
        }

        [HttpPost]
        [Authorize]
        public void ChangeDetails(EditDetailedViewModel model)
        {
            var user = getDetails();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Picture = model.Picture;
            context.SaveChanges();

            Response.Redirect("/User");
        }
        [HttpPost]
        [Authorize]
        public void ToggleAdmin()
        {
            var UserId = User.Identity.GetUserId();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            if(User.IsInRole("Administrator"))
                UserManager.RemoveFromRole(UserId, "Administrator");
            else
                UserManager.AddToRole(UserId, "Administrator");

            FormsAuthentication.SignOut();
            Response.Redirect("/Account/Login");
        }
    }
}