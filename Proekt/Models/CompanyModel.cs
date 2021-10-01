using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("Companies")]
    public class CompanyModel
    {
        [Key]
        public int Id { get; set; }
        public int? TmdbID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        
        public CompanyModel() { }
        public CompanyModel(dynamic d)
        {
            this.TmdbID = d.id;
            this.Name = d.name;
            this.Country = d.origin_country;
            this.Logo = d.logo_path;
        }
    }
}