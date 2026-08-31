using AtelieDosPontinhos.Application.DTOs;

namespace AtelieDosPontinhos.Application.Interfaces
{
    /// <summary>
    /// Contrato para o serviço de gerenciamento de Usuários.
    /// Define as operações de CRUD e obtenção de perfis.
    /// </summary>
    public interface IUsuariosService
    {
        Task<IEnumerable<UsuarioDto>> GetAllAsync();
        Task<UsuarioDto?> GetByIdAsync(string id);
        Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> CreateAsync(CreateUsuarioDto dto);
        Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> UpdateAsync(string id, UpdateUsuarioDto dto);
        Task<(bool Success, string ErrorMessage)> DeleteAsync(string id);
        Task<IEnumerable<string>> GetPerfisAsync();
    }
}
