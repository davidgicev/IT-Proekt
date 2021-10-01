using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Proekt.Models;

namespace Proekt.Controllers
{
    public class ActorController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Actor
        public ActionResult Index()
        {
            return HttpNotFound();
        }

        public ActionResult Details(int id)
        {
            var target = context.Actors.Include(m => m.Movies).
                SingleOrDefault(m => m.Id == id);
            if (target == null)
                return HttpNotFound();

            return View(target);
        }
    }
}