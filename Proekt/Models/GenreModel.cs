using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Genres")]
    public class GenreModel
    {  
        [Key]
        public int? Id { get; set; }
        public int? TmdbID { get; set; }
        public string Name { get; set; }
        public List<MovieModel> Movies { get; set; }

        public GenreModel() { }
        public GenreModel(dynamic obj)
        {
            this.TmdbID = (int?) obj.id;
            this.Name = (string) obj.name;
            this.Movies = new List<MovieModel>();
        }
    }
}