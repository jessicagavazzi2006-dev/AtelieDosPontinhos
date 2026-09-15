using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AtelieDosPontinhos.UI.Models
{
    public class ProductViewModel
    {
        private bool _destaque = false;

        public int Id { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
        public string Nome { get => Name; set => Name = value; }

        public decimal Price { get; set; } = 0m;
        public decimal Preco { get => Price; set => Price = value; }

        public string CoverImageUrl { get; set; } = string.Empty;
        public string ImagemUrl { get => CoverImageUrl; set => CoverImageUrl = value; }

        public string Description { get; set; } = string.Empty;
        public string Descricao { get => Description; set => Description = value; }

        public bool IsFavorited { get; set; } = false;

        // Propriedades espelhadas com backing field privado para garantir persistência no formulário e na API
        public bool Destaque
        {
            get => _destaque;
            set => _destaque = value;
        }

        public bool IsFeatured
        {
            get => _destaque;
            set => _destaque = value;
        }

        public int Stock { get; set; } = 0;
        public int CategoryId { get; set; } = 0;

        // Lista para carregar o dropdown de categorias no formulário
        public List<SelectListItem>? Categories { get; set; }

        public string Categoria { get; set; }
    }
}