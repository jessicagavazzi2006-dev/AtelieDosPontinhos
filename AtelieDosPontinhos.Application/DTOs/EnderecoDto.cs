using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class EnderecoDto
    {
        public int Id { get; set; }
        public string CEP { get; set; } = string.Empty;
        public int NUMERO { get; set; }
        public string Estado {  get; set; } = string.Empty;
        public string Referencial {  get; set; } = string.Empty;
        public string Cidade {  get; set; } = string.Empty;
    }

    public class CreateEnderecoDto
    {
        
        public string CEP { get; set; } = string.Empty;
        public int NUMERO { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Referencial { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
    }

    public class UpdateEnderecoDto
    {

        public string CEP { get; set; } = string.Empty;
        public int NUMERO { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Referencial { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
    }
}
