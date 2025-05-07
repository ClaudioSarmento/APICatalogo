using APICatalago.Domain.Entities;
using APICatalago.Pagination;
namespace APICatalago.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
}
