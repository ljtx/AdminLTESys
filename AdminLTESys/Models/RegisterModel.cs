using System.ComponentModel.DataAnnotations;

namespace AdminLTESys.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码不能为空。")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "两次输入的密码不同")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        public string DepartmentId { get; set; }
    }
}