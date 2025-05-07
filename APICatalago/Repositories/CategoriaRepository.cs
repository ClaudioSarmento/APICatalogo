using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }

    public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
    {
        var categorias = await GetAllAsync();
        var categoriasOrdenadas = categorias
        .OrderBy(c => c.Id)
        .AsQueryable();
        return PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);
    }

    public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = await GetAllAsync();
        var categoriasAsQueryable = categorias.AsQueryable();
        if (!string.IsNullOrWhiteSpace(categoriasFiltroNome.Nome))
        {
            categorias = categorias.Where(c => c.Nome!.ToLowerInvariant().Contains(categoriasFiltroNome.Nome.ToLowerInvariant()));
        }
        categorias = categorias.OrderBy(p => p.Nome);
        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categoriasAsQueryable, categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        return categoriasFiltradas;
    }
}
