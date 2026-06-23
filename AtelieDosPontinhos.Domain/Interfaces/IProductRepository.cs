using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// retorna todos os Products exitentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllAsync();


        /// <summary>
        /// busca um Product pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// retorna todos os Products de uma categoria
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(int  categoryId);

        //atualiza um Product
        Task UpdateAsync(Product product);

        //Deleta um Product
        Task DeleteAsync(int id);
        
        //retorna todos os Products cadastrados
        Task<int>CountAsync();

        //adiciona Product
        Task AddAsync(Product product);

        Task<IEnumerable<Product>> SearchAsync(string term);

    }
}
