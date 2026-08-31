using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.DTOs
{
    public class UsuarioResponseDto
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Roles { get; set; } = string.Empty;
    }
    /// <summary>
    /// crianção do DTO para o usuário
    /// <summary>

    public class CreateUsuarioDto
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }


    public class UpdateUsuarioDto
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    /// <summary>
    /// dto para redefinir senha
    /// </summary>

    public class ResetPasswordDto
    {
        public string UserId { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    //atribuição o remoção  de role

    public class AssignRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
