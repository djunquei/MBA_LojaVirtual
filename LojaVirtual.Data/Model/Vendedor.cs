using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Data.Model
{
    public class Vendedor
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        public IEnumerable<Produto> Produtos { get; set; }

    }
}
