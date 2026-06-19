using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Category
    {

        public int Id { get; set; }

        /// <summary>
        /// Nome de categoria
        /// </summary>
        public string Name { get; set; } = string.Empty;


        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// busca a imagem via local 
        /// </summary>
        public string ImageLocal { get; set; } = string.Empty;
    }
}
