using LojaVirtual.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LojaVirtual.Web.Models
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [DisplayName("Título")]
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Titulo { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O estoque é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser maior que zero")]
        public int Estoque { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória")]
        public IFormFile Imagem { get; set; }

        [DisplayName("Categoria")]
        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }

    }
}
