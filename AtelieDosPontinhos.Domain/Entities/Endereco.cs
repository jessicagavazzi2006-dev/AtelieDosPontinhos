using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; set; }
        public string CEP {  get; set; }
        public int NUMERO { get; set; }
        public string Estado { get; set; }
        public string Referencial { get; set; }
        public string Cidade { get; set; }
    }
}
