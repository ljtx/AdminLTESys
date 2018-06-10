using System;
using System.ComponentModel.DataAnnotations;

namespace Ambiel.AppService.RoleApp.Dtos
{
        public class RoleDto
        {
            public string Id { get; set; }

            public string Code { get; set; }

            [Required(ErrorMessage = "角色名称不能为空。")]
            public string Name { get; set; }

            public string CreateUserId { get; set; }

            public DateTime? CreateTime { get; set; }

            public string Remarks { get; set; }
        }
}