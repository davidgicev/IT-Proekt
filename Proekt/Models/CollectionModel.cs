using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Collections")]
    public class CollectionModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TmdbID { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }
        public string Backdrop { get; set; }
        public List<MovieModel> Movies { get; set; }

        public CollectionModel()
        {
            this.Movies = new List<MovieModel>();
        }

        public CollectionModel(dynamic d)
        {
            this.TmdbID = d.id;
            if (d.parts == null)
                return;
            this.Name = d.name;
            this.Overview = d.overview;
            this.Poster = d.poster_path;
            this.Backdrop = d.backdrop_path;
            this.Movies = new List<MovieModel>();
            foreach(dynamic m in (IEnumerable)d.parts)
            {
                this.Movies.Add(new MovieModel(m, false));
            }
        }
    }
}