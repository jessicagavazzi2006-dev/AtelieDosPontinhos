using System;
using System.Collections.Generic;
using System.Text;

// 🚨🚨🚨🚨necessario mudar caso ocorra alguma falta pelo codigo

namespace AtelieDosPontinhos.Application.DTOs
{
    public class Product_MaterialDto
    {
        public int ProductId { get; set; }

        public int MaterialId { get; set; }

        public int UnitUsed { get; set; }
    }
    public class CreateProduct_MaterialDto 
    {
        public int UnitUsed { get; set; }
    }
    public class UpdateProduct_MaterialDto
    {
        public int UnitUsed { get; set; }
    }
}
