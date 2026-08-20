using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int FormadePagamento { get; set; }  
        public int ValorPagamento { get; set; } 
        public DateTime DataPagamento { get; set; } 
    }
}
