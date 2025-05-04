using LojaVirtual.Data.Model;

namespace LojaVirtual.Data.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetAll();
        Task<Categoria> GetById(int id);
        Task AddCategory(Categoria produto);
        Task UpdateCategory(Categoria produto);
        Task DeleteCategory(int id);
    }
}
