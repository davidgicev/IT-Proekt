using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    public class ListInserter
    {
        public static void Insert(string ListName, IEnumerable data, ApplicationDbContext context, bool hidden=false)
        {
            ListModel listInDb = null;
            if(ListName != null)
            {
                context.Lists.AddOrUpdate(l => l.Name, new ListModel(ListName, hidden));
                context.SaveChanges();
                listInDb = context.Lists.Include("Movies").Single(l => l.Name == ListName);
            }

            int count = 1000;

            foreach (dynamic d in data)
            {
                count--;
                if (count < 0)
                {
                    break;
                }

                MovieModel movie = new MovieModel(d, true);

                var movieInDb = MovieInserter.Insert(movie, context);
                if (movieInDb.Collection != null)
                    throw new Exception("omggg");
                if(ListName != null)
                {
                    var id = movieInDb.TmdbID;
                    if(!listInDb.Movies.Any(m => m.TmdbID == id))
                        listInDb.Movies.Add(movieInDb);

                    if (!movieInDb.Lists.Any(l => l.Name == ListName))
                        movieInDb.Lists.Add(listInDb);
                }
            }

            context.SaveChanges();
        }
    }
}