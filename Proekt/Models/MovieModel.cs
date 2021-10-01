using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Movies")]
    public class MovieModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ImdbID { get; set; }
        public int? TmdbID { get; set; }
        public string Overview { get; set; }
        public string Language { get; set; }
        public string Poster{ get; set; }
        public string Backdrop{ get; set; }
        public int? Runtime { get; set; }
        public string Trailer { get; set; }
        public string Status { get; set; }
        public string ImdbRating { get; set; }
        public string RottenTomatoesRating { get; set; }
        public string MetacriticRating { get; set; }
        public List<GenreModel> Genres { get; set; }
        public List<ActorRoleModel> Cast { get; set; }
        public List<ListModel> Lists { get; set; }
        public CollectionModel Collection { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public MovieModel() {}

        public MovieModel(dynamic d, bool full)
        {
            this.TmdbID = (int?)d.id;
            if(!full)
                return;
            this.Title = (string)d.title;
            this.ImdbID = (string)d.imdb_id;
            
            DateTime result;
            if (DateTime.TryParseExact(
                (string)d.release_date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result))
                this.ReleaseDate = result;
            else
                this.ReleaseDate = null;

            this.Overview = (string)d.overview;
            this.Poster = (string)d.poster_path;
            this.Backdrop = (string)d.backdrop_path;
            this.Runtime = (int?)d.runtime;
            this.Trailer = (string)d.trailer;
            this.Status = (string)d.status;
            string lang = (string)d.original_language;
            this.Language = CultureInfo.GetCultureInfo(lang).EnglishName;
            if (d.belongs_to_collection != null)
            {
                var col = new CollectionModel(d.belongs_to_collection);
                if (col.Name != null)
                {
                    this.Collection = col;
                }
            }
            this.ImdbRating = (string)d.ratings[0];
            this.RottenTomatoesRating = (string)d.ratings[1];
            this.MetacriticRating = (string)d.ratings[2];
            this.Lists = new List<ListModel>();
            populateGenres(d);
            populateCast(d);
            populateCompanies(d);
        }

        private void populateGenres(dynamic d)
        {
            if (d.genres == null)
                return;
            IEnumerable genres = (IEnumerable)d.genres;
            List<GenreModel> objs = new List<GenreModel>();
            foreach (dynamic genre in genres)
            {
                GenreModel obj = new GenreModel(genre);
                if (obj.TmdbID == null)
                    continue;
                objs.Add(obj);
            }
            this.Genres = objs;
        }

        private void populateCast(dynamic d)
        {
            if (d.cast == null)
                return;
            IEnumerable people = (IEnumerable)d.cast;
            List<ActorRoleModel> objs = new List<ActorRoleModel>();
            foreach (dynamic person in people)
            {
                ActorRoleModel obj = new ActorRoleModel(person);
                if (obj.TmdbID == null)
                    continue;
                objs.Add(obj);
            }
            this.Cast = objs;
        }

        private void populateCompanies(dynamic d)
        {
            if (d.production_companies == null)
                return;
            IEnumerable companies = (IEnumerable)d.production_companies;
            List<CompanyModel> objs = new List<CompanyModel>();
            foreach (dynamic company in companies)
            {
                CompanyModel obj = new CompanyModel(company);
                if (obj.TmdbID == null)
                    continue;
                objs.Add(obj);
            }
            this.Companies = objs;
        }
    }
}