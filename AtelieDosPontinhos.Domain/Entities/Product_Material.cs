using System;

namespace AtelieDosPontinhos.Domain.Entities
{
    // Entidade de junção entre Product e Material (many-to-many)
    public class Product_Material
    {
        // Chave estrangeira para Product
        public int ProductId { get; set; }

        // Chave estrangeira para Material
        public int MaterialId { get; set; }

        // Quantidade de material usada no produto
        public int UnitUsed { get; set; }

        // Relacionamento com Product
        public virtual Product? Product { get; set; }

        // Relacionamento com Material
        public virtual Material? Material { get; set; }
    }
}