using AtelieDosPontinhos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<MaterialDto> Materiais { get; set; } = new List<MaterialDto>();
        public IEnumerable<ProductDto> ProductsRecentes { get; set; } = new List<ProductDto>();
    }
    public class ProductDetailViewModel
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public IEnumerable<ProductDto> ProductsRelated { get; set;} = new List<ProductDto>();
    }
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalMaterials { get; set; }
        public IEnumerable<ProductDto> RecentProduct { get; set; } = new List<ProductDto>();

    }
    public class ProductFormViewModel
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } =string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string Type_Product { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public IEnumerable<CategoryDto> categories { get; set; } = new List<CategoryDto>();
    }
    public class ProductListViewModel
    {
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        
        //public IEnumerable<MaterialDto> materials { get; set; } = new List<MaterialDto>();  
       
        public int? SelectedCategoryId { get; set; }
       
    }
}   

