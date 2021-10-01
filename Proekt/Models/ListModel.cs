using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Lists")]
    public class ListModel
    {
        [Key]
        public int Id { get; set; }
        public List<MovieModel> Movies { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public string Backdrop { get; set; }
        public bool Hidden { get; set; }

        public ListModel()
        {
            this.Hidden = false;
            this.Movies = new List<MovieModel>();
        }
        public ListModel(string ListName, bool hidden)
        {
            this.Movies = new List<MovieModel>();
            this.Name = ListName;
            this.Hidden = hidden;
        }
    }
}