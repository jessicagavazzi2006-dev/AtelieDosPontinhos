using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Amount { get; set; }

        public string Unit { get; set; } = string.Empty;
    }
}
