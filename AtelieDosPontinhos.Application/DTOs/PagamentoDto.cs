using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class PagamentoDto
    {
        public int Id { get; set; }
        public int FormadePagamento { get; set; }
        public int ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
    }

    public class CreatePagamentoDto
    {
        
        public int FormadePagamento { get; set; }
        public int ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
    }

    public class UpdatePagamentoDto
    {

        public int FormadePagamento { get; set; }
        public int ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
    } 
}
