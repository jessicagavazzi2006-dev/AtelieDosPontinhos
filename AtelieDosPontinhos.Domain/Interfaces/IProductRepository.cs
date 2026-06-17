using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// retorna todos os produtos exitentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllAsync();


        /// <summary>
        /// busca um produto pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// retorna todos os produtos de uma categoria
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(int  categoryId);

        //atualiza um produto
        Task UpdateAsync(Product product);

        //Deleta um produto
        Task DeleteAsync(int id);
        
        //retorna todos os produtos cadastrados
        Task<int>CountAsync();

        //adiciona produto
        Task AddAsync(Product product);



    }
}
