// =============================================================================
// AtelieDosPontinhos.Application - Services/UsuariosService.cs
// =============================================================================

using Microsoft.AspNetCore.Identity;
using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;

namespace AtelieDosPontinhos.Application.Services
{
    /// <summary>
    /// Implementação do serviço de gerenciamento de Usuários.
    /// Utiliza UserManager e RoleManager do ASP.NET Core Identity.
    /// </summary>
    public class UsuariosService : IUsuariosService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UsuarioDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UsuarioDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty, // UserName é usado como Nome no projeto atual
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "Usuario"
                });
            }

            return result;
        }

        public async Task<UsuarioDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UsuarioDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "Usuario"
            };
        }

        public async Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> CreateAsync(CreateUsuarioDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return (false, null, "As senhas não coincidem.");

            // Verifica se e-mail já existe
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return (false, null, "E-mail já cadastrado.");

            var user = new IdentityUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, null, $"Erro ao criar usuário: {errors}");
            }

            // Adicionar ao Role
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
            }
            else
            {
                // Role padrão
                await _userManager.AddToRoleAsync(user, "Usuario");
            }

            var createdUser = await GetByIdAsync(user.Id);
            return (true, createdUser, string.Empty);
        }

        public async Task<(bool Success, UsuarioDto? Usuario, string ErrorMessage)> UpdateAsync(string id, UpdateUsuarioDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Password) && dto.Password != dto.ConfirmPassword)
                return (false, null, "As senhas não coincidem.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, null, "Usuário não encontrado.");

            // Verifica se o novo e-mail já pertence a outro usuário
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null && existingUser.Id != user.Id)
                return (false, null, "E-mail já cadastrado por outro usuário.");

            user.UserName = dto.UserName;
            user.Email = dto.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return (false, null, $"Erro ao atualizar usuário: {errors}");
            }

            // Atualiza senha se informada
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passResult.Succeeded)
                {
                    var errors = string.Join(", ", passResult.Errors.Select(e => e.Description));
                    return (false, null, $"Erro ao atualizar senha: {errors}");
                }
            }

            // Atualiza Role
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(dto.Role) && !currentRoles.Contains(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
            }

            var updatedUser = await GetByIdAsync(user.Id);
            return (true, updatedUser, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return (false, "Usuário não encontrado.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Erro ao excluir usuário: {errors}");
            }

            return (true, string.Empty);
        }

        public async Task<IEnumerable<string>> GetPerfisAsync()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return await Task.FromResult(roles!);
        }
    }
}
