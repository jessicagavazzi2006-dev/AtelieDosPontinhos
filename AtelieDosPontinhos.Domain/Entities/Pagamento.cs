using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int CartaoDebito { get; set; }  
        public int CartaoCredito { get; set; } 
        public string Boleto { get; set; } = string.Empty;
        public string Pix {  get; set; } = string.Empty;
    }
}
