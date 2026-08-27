// =============================================================================
// SenacGames.Application - DTOs de Usuario
// =============================================================================

namespace AtelieDosPontinhos.Application.DTOs
{
    /// <summary>
    /// DTO para transferência de dados de um Usuário.
    /// </summary>
    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role{ get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para criação de um novo Usuário.
    /// </summary>
    public class CreateUsuarioDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de um Usuário existente.
    /// </summary>
    public class UpdateUsuarioDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } // Senha é opcional na edição
        public string? ConfirmPassword { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
