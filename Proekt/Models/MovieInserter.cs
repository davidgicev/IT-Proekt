using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;

namespace Proekt.Models
{
    public class MovieInserter
    {
        public static MovieModel Insert(MovieModel movie, ApplicationDbContext context)
        {
            var genres = movie.Genres;
            movie.Genres = null;
            var cast = movie.Cast;
            movie.Cast = null;
            var collection = movie.Collection;
            movie.Collection = null;
            var companies = movie.Companies;
            movie.Companies = null;

            if(!context.Movies.Any(m => m.TmdbID == movie.TmdbID))
            {
                context.Movies.Add(movie);
                context.SaveChanges();
            }
            
            var movieInDb = context.Movies.Include("Collection")
                                          .Include("Genres")
                                          .Include("Cast.Actor")
                                          .Include("Companies")
                                          .Single(m => m.TmdbID == movie.TmdbID);

            int len = genres.Count();

            for (int i = 0; i < len; i++)
            {
                GenreModel genre = genres[i];
                GenreModel found = context.Genres.Include(g => g.Movies).
                    SingleOrDefault(g => g.TmdbID == genre.TmdbID);
                if (found != null)
                {
                    genres[i] = found;
                    if (!found.Movies.Any(m => m.TmdbID == movie.TmdbID))
                        found.Movies.Add(movieInDb);
                }
            }


            len = cast.Count();

            for (int i = 0; i < len; i++)
            {
                var person = cast[i];

                var actor = person.Actor;

                var targetRole = context.ActorRoles.Include("Actor").
                    SingleOrDefault(p => p.TmdbID == person.TmdbID);

                var targetActor = context.Actors.Include("Movies").
                    SingleOrDefault(t => t.TmdbID == actor.TmdbID);

                if (targetActor == null && targetRole != null)
                    throw new Exception("ne ochekuvano");

                if (targetRole == null)
                {
                    targetRole = person;
                }

                if(targetActor == null)
                {
                    targetActor = actor;
                }

                targetRole.Actor = targetActor;
                
                if (!targetActor.Movies.Any(m => m.TmdbID == movie.TmdbID))
                    targetActor.Movies.Add(movieInDb);
                
                cast[i] = targetRole;

            }

            len = companies.Count();

            for (int i = 0; i < len; i++)
            {
                CompanyModel company = companies[i];
                CompanyModel target = context.Companies.
                    SingleOrDefault(t => t.TmdbID == company.TmdbID);
                if (target != null)
                {
                    companies[i] = target;
                }
            }

            genres = genres.GroupBy(i => i.TmdbID).Select(o => o.First()).ToList();

            //cast = cast.GroupBy(i => i.TmdbID).Select(o => o.First()).ToList();
            cast = cast.GroupBy(i => i.Actor.TmdbID).Select(o => o.First()).ToList();
            companies = companies.GroupBy(i => i.TmdbID).Select(o => o.First()).ToList();

            var duplicates = cast.Select(i => i.Actor).GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("movie inserter: duplikati vo Actors");

            movieInDb.Genres = genres;
            movieInDb.Cast = cast;
            movieInDb.Collection = collection;
            movieInDb.Companies = companies;
            context.SaveChanges();

            return movieInDb;
        }
    }
}