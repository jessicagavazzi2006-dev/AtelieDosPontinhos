using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do produto
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL da imagem do produto
        /// </summary>
        public string CoverImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Preço dos produtos
        /// </summary>
        public decimal Price {  get; set; }

        /// <summary>
        /// Estoque dos produtos
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Tipo do produto
        /// </summary>
        public string Type_Product { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        // NAV: coleções de mapeamento many-to-many com payload
        public virtual ICollection<Product_Material> Product_Materials { get; set; } = new List<Product_Material>();




    }
}
