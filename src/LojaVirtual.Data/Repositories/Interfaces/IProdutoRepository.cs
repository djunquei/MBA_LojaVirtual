using LojaVirtual.Data.Model;

namespace LojaVirtual.Data.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAll();
        Task<IEnumerable<Produto>> GetAllByCategoria(int idCategoria);
        Task<IEnumerable<Produto>> GetAllByVendedor(string idVendedor);
        Task<Produto> GetById(int id);
        Task AddProduct(Produto produto);
        Task UpdateProduct(Produto produto);
        Task DeleteProduct(int id);
    }
}
