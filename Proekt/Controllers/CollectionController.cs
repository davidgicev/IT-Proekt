using Proekt.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proekt.Controllers
{
    public class CollectionController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Collection
        public ActionResult Index()
        {
            return HttpNotFound();
        }

        public ActionResult Details(int id)
        {
            var target = context.Collections.
                Include("Movies.Genres").
                Include("Movies.Cast.Actor").
                SingleOrDefault(m => m.Id == id);
            if (target == null)
                return HttpNotFound();

            return View(target);
        }
    }
}