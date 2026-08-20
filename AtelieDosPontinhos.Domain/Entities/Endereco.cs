using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; set; }
        public string CEP {  get; set; } = string.Empty;
        public int NUMERO { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Referencial { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
    }
}
