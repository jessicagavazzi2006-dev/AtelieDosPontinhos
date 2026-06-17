using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Product_Material
    {
        public int ProductId { get; set; }

        public int MaterialId { get; set; }

        public int UnitUsed { get; set; }

        public virtual Product? Products { get; set; }

        public virtual Material? Materials { get; set; }
    }
}
