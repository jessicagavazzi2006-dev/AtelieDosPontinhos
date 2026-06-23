namespace SeuProjeto.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public string Categoria { get; set; }
    }
}