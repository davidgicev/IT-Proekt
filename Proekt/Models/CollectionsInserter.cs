using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    public class CollectionsInserter
    {
        public static void Insert(IEnumerable data, ApplicationDbContext context)
        {

            foreach (dynamic d in data)
            {

                CollectionModel collection = new CollectionModel(d);
                var movies = collection.Movies;
                collection.Movies = null;
                context.Collections.AddOrUpdate(c => c.TmdbID, collection);
                context.SaveChanges();
                CollectionModel collectionInDb = context.Collections.Include("Movies").
                    Single(c => c.TmdbID == collection.TmdbID);

                int l = movies.Count();
                
                for (int i=0; i<l; i++)
                {
                    var movie = movies[i];
                    var found = context.Movies.SingleOrDefault(m => m.TmdbID == movie.TmdbID);
                    if (found != null)
                    {
                        found.Collection = collectionInDb;
                        movies[i] = found;
                    }
                    else
                    {
                        movies[i].Collection = collectionInDb;
                    }   
                }

                collectionInDb.Movies = movies;
                context.SaveChanges();
            }
        }
    }
}