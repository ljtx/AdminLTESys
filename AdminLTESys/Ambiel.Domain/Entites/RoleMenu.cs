using System;
using System.ComponentModel.DataAnnotations;

namespace Ambiel.Domain.Entites
{
    public class RoleMenu
    {
        [MaxLength(15)]
        public string RoleId { get; set; }
        public Role Role { get; set; }
        [MaxLength(15)]
        public string MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}