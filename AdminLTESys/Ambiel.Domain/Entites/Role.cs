using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ambiel.Domain.Entites
{
    public class Role : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
        [MaxLength(15)]
        public string CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        
    }
}