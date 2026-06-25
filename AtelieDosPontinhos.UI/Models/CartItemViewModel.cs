using System;

namespace AtelieDosPontinhos.UI.Models
{
    public class CartItemViewModel
    {
        public ProductViewModel Produto { get; set; } = new ProductViewModel();
        public int Quantidade { get; set; }

        public decimal Total
        {
            get
            {
                var preco = Convert.ToDecimal(Produto.GetType().GetProperty("Preco")?.GetValue(Produto)
                            ?? Produto.GetType().GetProperty("Price")?.GetValue(Produto) ?? 0m);
                return preco * Quantidade;
            }
        }
    }
}