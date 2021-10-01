using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Actors")]
    public class ActorModel
    {
        [Key]
        public int? Id { get; set; }
        public int? TmdbID { get; set; }
        public string Name { get; set; }
        public string Poster { get; set; }
        public List<MovieModel> Movies { get; set; }

        public ActorModel() { }
        public ActorModel(dynamic d)
        {
            this.TmdbID = (int?) d.id;
            this.Name = (string)d.name;
            this.Poster = (string)d.profile_path;
            this.Movies = new List<MovieModel>();
        }
    }
}