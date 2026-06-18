using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IProductMaterialRepository
    {
        /// <summary>
        /// retorna todos as categorias exitentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product_Material>> GetAllAsync();


        /// <summary>
        /// busca uma categoria pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product_Material?> GetByIdAsync(int id);


        //atualiza um material
        Task UpdateAsync(Product_Material product_Material);

        //Deleta um material
        Task DeleteAsync(int id);

        //retorna todos os material cadastrados
        Task<int> CountAsync();

        //adiciona uma categoria
        Task AddAsync(Product_Material product_Material);
    }
}
