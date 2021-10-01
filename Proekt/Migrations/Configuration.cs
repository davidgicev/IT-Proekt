namespace Proekt.Migrations
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using Proekt.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Proekt.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Proekt.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            
            context.Lists.RemoveRange(context.Lists.ToList());
            context.Companies.RemoveRange(context.Companies.ToList());
            context.ActorRoles.RemoveRange(context.ActorRoles.ToList());
            context.Genres.RemoveRange(context.Genres.ToList());
            context.Actors.RemoveRange(context.Actors.ToList());
            context.Collections.RemoveRange(context.Collections.ToList());
            context.Movies.RemoveRange(context.Movies.ToList());
            context.SaveChanges();
            
            using (StreamReader reader = new StreamReader(MapPath("~/App_Data/completeCollections.json")))
            {
                string json = reader.ReadToEnd();

                IEnumerable data = (IEnumerable)JsonConvert.DeserializeObject(json);

                ListInserter.Insert(null, data, context, true);
            }

            using (StreamReader reader = new StreamReader(MapPath("~/App_Data/povekjePopularni.json")))
            {
                string json = reader.ReadToEnd();

                IEnumerable data = (IEnumerable)JsonConvert.DeserializeObject(json);

                ListInserter.Insert(null, data, context, true);

            }

            using (StreamReader reader = new StreamReader(MapPath("~/App_Data/popularMovies.json")))
            {
                string json = reader.ReadToEnd();

                IEnumerable data = (IEnumerable)JsonConvert.DeserializeObject(json);

                ListInserter.Insert("Popular", data, context, true);

            }

            using (StreamReader reader = new StreamReader(MapPath("~/App_Data/cineplexx.json")))
            {
                string json = reader.ReadToEnd();

                IEnumerable data = (IEnumerable)JsonConvert.DeserializeObject(json);

                ListInserter.Insert("Cineplexx", data, context, true);
            }

            using (StreamReader reader = new StreamReader(MapPath("~/App_Data/extractedCollections.json")))
            {
                string json = reader.ReadToEnd();

                IEnumerable data = (IEnumerable)JsonConvert.DeserializeObject(json);

                CollectionsInserter.Insert(data, context);
            }
            

            var duplicates = context.Movies.GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("ima duplikati vo Movies");

            duplicates = context.Genres.GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("ima duplikati vo Genres");

            duplicates = context.Actors.GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("ima duplikati vo Actors "+duplicates.ToList().Count().ToString());

            duplicates = context.Collections.GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("ima duplikati vo Collections");

            duplicates = context.Companies.GroupBy(i => i.TmdbID)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);
            if (duplicates.Count() > 0)
                throw new Exception("ima duplikati vo Companies");

            if (context.ActorRoles.Include("Actor").Any(r => r.Actor == null))
                throw new Exception("prazno");
            
            context.SaveChanges();
        }

        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }
    }
}