using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proekt.Models
{
    [Table("ActorRoles")]
    public class ActorRoleModel
    {
        [Key]
        public int? Id { get; set; }
        public string TmdbID { get; set; }
        public string Role { get; set; }
        public ActorModel Actor { get; set; }
        public ActorRoleModel() { }
        public ActorRoleModel(dynamic d)
        {
            this.TmdbID = (string)d.credit_id;
            this.Role = (string)d.character;
            this.Actor = new ActorModel(d);
        }
    }
}