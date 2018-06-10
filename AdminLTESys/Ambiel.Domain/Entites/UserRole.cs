using System;
using System.ComponentModel.DataAnnotations;

namespace Ambiel.Domain.Entites
{
    public class UserRole
    {
        [MaxLength(15)]
        public string UserId { get; set; }
        public User User { get; set; }
        [MaxLength(15)]
        public string RoleId { get; set; }
        public Role Role { get; set; }

    }
}