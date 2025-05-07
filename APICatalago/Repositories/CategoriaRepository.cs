using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    {
        var categorias = GetAll()
               .OrderBy(c => c.Id)
               .AsQueryable();
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);
        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = GetAll().AsQueryable();
        if (!string.IsNullOrWhiteSpace(categoriasFiltroNome.Nome))
        {
            categorias = categorias.Where(c => c.Nome!.ToLowerInvariant().Contains(categoriasFiltroNome.Nome.ToLowerInvariant()));
        }
        categorias = categorias.OrderBy(p => p.Nome);
        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        return categoriasFiltradas;
    }
}
