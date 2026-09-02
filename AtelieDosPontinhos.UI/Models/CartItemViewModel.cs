using System;

namespace AtelieDosPontinhos.UI.Models
{
    public class CartItemViewModel
    {
        public ProductViewModel Produto { get; set; } = new ProductViewModel();
        public int Quantidade { get; set; } = 0;

        public decimal Total
        {
            get
            {
                if (Produto == null) return 0m;
                var preco = Produto.Price > 0 ? Produto.Price : Produto.Preco;
                return preco * Quantidade;
            }
        }
    }
}