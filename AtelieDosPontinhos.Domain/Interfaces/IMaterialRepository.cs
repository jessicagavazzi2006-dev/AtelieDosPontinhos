using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        /// <summary>
        /// retorna todos as categorias exitentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Material>> GetAllAsync();


        /// <summary>
        /// busca um material pelo id
        /// </summary>
        /// 
        /// <returns></returns>
        Task<Material?> GetByIdAsync(int id);


        //atualiza um material
        Task UpdateAsync(Material material);

        //Deleta um material
        Task DeleteAsync(int id);

        //retorna todos os material cadastrados
        Task<int> CountAsync();

        //adiciona um material
        Task AddAsync(Material material);
    }
}
