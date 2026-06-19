using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
    //criar material
    public class CreateMaterialDto
    {
        public string Name { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
    //atualiza material
    public class UpdateMaterialDto
    {
        public string Name { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
