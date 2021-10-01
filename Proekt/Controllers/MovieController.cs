using Proekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Proekt.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Movie
        public ActionResult Index(int id)
        {

            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Favorite(int id)
        {
            var UserId = User.Identity.GetUserId();
            var userInDb = context.UserDetails.Include("Favorites")
                .SingleOrDefault(u => u.UserId == UserId);

            bool favorited = false;
            if(userInDb != null)
            {
                var movieInDb = context.Movies.Find(id);
                if (movieInDb == null)
                    return HttpNotFound();
                if(userInDb.Favorites.Any(m => m.Id == id))
                {
                    userInDb.Favorites.Remove(movieInDb);
                }
                else
                {
                    userInDb.Favorites.Add(movieInDb);
                    favorited = true;
                }
            }

            context.SaveChanges();

            return Content(favorited ? "True" : "False");
        }

        public ActionResult Details(int id)
        {
            var target = context.Movies.
                            Include(m => m.Genres).
                            Include("Cast.Actor").
                            Include("Collection.Movies").
                            Include(m => m.Companies).
                            Include(m => m.Lists).
                            SingleOrDefault(m => m.Id == id);
            
            if (target == null)
                return HttpNotFound();

            if (target.Cast.Any(c => c.Actor == null))
                throw new Exception("fali actor reference");

            if(User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var userInDb = context.UserDetails.Include("Favorites")
                    .SingleOrDefault(u => u.UserId == UserId);

                bool favorited = userInDb.Favorites.Any(m => m.Id == id);

                ViewBag.favorited = favorited;
            }

            return View(target);
        }
        [Authorize(Roles = "Administrator")]
        public void Delete(int id)
        {
            var target = context.Movies.
                            Include(m => m.Genres).
                            Include("Cast.Actor.Movies").
                            Include("Collection.Movies").
                            Include(m => m.Companies).
                            Include(m => m.Lists).
                            SingleOrDefault(m => m.Id == id);

            target.Genres.ForEach(g => g.Movies.Remove(target));
            target.Cast.ToList().ForEach(c => context.ActorRoles.Remove(c));
            target.Cast.ForEach(c => c.Actor.Movies.Remove(target));
            target.Lists.ForEach(l => l.Movies.Remove(target));
            if(target.Collection != null)
                target.Collection.Movies.Remove(target);
            context.Movies.Remove(target);
            context.SaveChanges();

            Response.Redirect("/Home");
        }
    }
}