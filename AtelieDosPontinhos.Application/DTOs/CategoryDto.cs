using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ProductCount { get; set; }

        public string ImageLocal { get; set; } = string.Empty;
    }
    //criar categoria
    public class CreateCategoryDto 
    {
        public string Name { get; set; } = string.Empty;

        public string ImageLocal { get; set; } = string.Empty;
    }
    //atualiza a categoria
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public string ImageLocal { get; set; } = string.Empty;
    }

}
