using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.DTOs
{
    public class PagamentoDto
    {
        public int Id { get; set; }
        public int CartaoDebito { get; set; }
        public int CartaoCredito { get; set; }
        public string Boleto { get; set; } = string.Empty;
        public string Pix { get; set; } = string.Empty;
    }

    public class CreatePagamentoDto
    {

        public int CartaoDebito { get; set; }
        public int CartaoCredito { get; set; }
        public string Boleto { get; set; } = string.Empty;
        public string Pix { get; set; } = string.Empty;
    }

    public class UpdatePagamentoDto
    {

        public int CartaoDebito { get; set; }
        public int CartaoCredito { get; set; }
        public string Boleto { get; set; } = string.Empty;
        public string Pix { get; set; } = string.Empty;
    }
}
