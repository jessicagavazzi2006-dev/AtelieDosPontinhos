using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string Email {  set; get; } = string.Empty;
        public string Password { set; get; } = string.Empty;
        public string ConfirmPassword { set; get; } = string.Empty;
    }

    public class UserDto
    {
        public string Id { set; get; } = string.Empty;
        public string Email { set; get; } = string.Empty;
        public IList<string> Roles { set; get; } = new List<string>();
        public bool IsAdmin => Roles.Contains("Admin");
    }

}
