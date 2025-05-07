using APICatalago.Domain.Entities;
using APICatalago.Pagination;
namespace APICatalago.Repositories.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);

    Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome CategoriasFiltroNome);
}
