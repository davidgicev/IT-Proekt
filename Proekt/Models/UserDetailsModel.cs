using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("UserDetails")]
    public class UserDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Name {
            get {
                return FirstName + " " + LastName;
            }
        }
        public List<MovieModel> Favorites { get; set; }
        public UserDetailsModel() { }
        public UserDetailsModel(string FirstName, string LastName, string Picture, ApplicationUser user)
        {
            this.User = user;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Picture = Picture;
            this.Favorites = new List<MovieModel>();
        }
    }
}