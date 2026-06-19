using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Type_Product { get; set; } = string.Empty;
    }
    //criar produto
    public class CreateProductDto
    {
        public string name { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
    //atulizar produto
    public class UpdateProductDto
    {
        public string name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}
