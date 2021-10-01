using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proekt.Models;
using System.Data.Entity;

namespace Proekt.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Search
        public ActionResult Index(bool? type, int? id)
        {
            var genres = context.Genres.ToList();
            if(type != null)
            {
                ViewBag.type = (bool)type;
                ViewBag.id = (int)id;
                if(!(bool)type)
                {
                    var actor = context.Actors.Find((int)id);
                    ViewBag.name = actor.Name;
                    ViewBag.img = actor.Poster;
                }
            }
            return View(genres);
        }

        public ActionResult Filter(int[] genres, int[] actors, string query)
        {
            IEnumerable<MovieModel> Movies = context.Movies.Where(m => m.ReleaseDate != null);
            if (genres != null && genres.Length > 0)
            {
                var Genres = genres.Select(g => context.Genres.Include("Movies").Single(o => o.Id == g)).ToList();
                Movies = Genres.Select(g => g.Movies.AsEnumerable()).
                    Aggregate(Movies, (result, current) => { return result.Intersect(current); });
            }

            if (actors != null && actors.Length > 0)
            {
                var Actors = actors.Select(a => context.Actors.Include("Movies").Single(o => o.Id == a)).ToList();
                Movies = Actors.Select(a => a.Movies.AsEnumerable()).
                    Aggregate(Movies, (result, current) => { return result.Intersect(current); });
            }

            query = query.Trim();
            if(query != "")
            {
                var lower = query.ToLower();
                Movies = Movies.Where(m => m.Title.ToLower().Contains(lower));
            }

            return View(Movies.Take(100).ToList());
        }

        public ActionResult Actors(string query)
        {
            List<ActorModel> Actors = new List<ActorModel>();
            if(query != "")
            {
                var lower = query.ToLower();
                Actors = context.Actors.Where(a => a.Name.ToLower().Contains(lower)).Take(100).ToList();
            }

            return View(Actors);
        }
    }
}