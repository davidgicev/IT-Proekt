using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proekt.Models;
using System.Data.Entity;

namespace Proekt.Controllers
{
    public class ListController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: List
        public ActionResult Index()
        {
            return HttpNotFound();
        }

        public ActionResult Details(int id)
        {
            var target = context.Lists.Include(m => m.Movies).
                SingleOrDefault(m => m.Id == id);
            if (target == null)
                return HttpNotFound();

            return View(target);
        }
    }
}