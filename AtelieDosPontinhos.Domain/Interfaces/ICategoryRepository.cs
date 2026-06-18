using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// retorna todos as categorias exitentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAllAsync();


        /// <summary>
        /// busca uma categoria pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category?> GetByIdAsync(int id);


        //atualiza uma categoria
        Task UpdateAsync(Category category);

        
        

        //Deleta um categoria
        Task DeleteAsync(int id);

        //retorna todos os catergoria cadastrados
        Task<int> CountAsync();

        //adiciona uma categoria
        Task AddAsync(Category category);
    }
}
