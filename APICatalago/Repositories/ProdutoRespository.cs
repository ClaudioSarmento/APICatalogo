using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Repositories;

public class ProdutoRespository : Repository<Produto>, IProdutoRepository
{
  
    public ProdutoRespository(AppDbContext context) : base(context) { }

    public async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos
            .OrderBy(p => p.Id)
            .AsQueryable();

        return PagedList<Produto>.ToPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);
    }

    public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = await GetAllAsync();
        if (produtosFiltroPreco.Preco.HasValue &&
            !string.IsNullOrWhiteSpace(produtosFiltroPreco.PrecoCriterio))
        {
            var preco = produtosFiltroPreco.Preco.Value;
            produtos = produtosFiltroPreco.PrecoCriterio.ToLowerInvariant() switch
            {
                "maior" => produtos.Where(p => p.Preco > preco),
                "menor" => produtos.Where(p => p.Preco < preco),
                "igual" => produtos.Where(p => p.Preco == preco),
                _ => produtos
            };
        }
        var produtosAsQueryable = produtos
            .AsQueryable()
            .OrderBy(p => p.Preco);
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtosAsQueryable, produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);
        return produtosFiltrados;

    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int idCategoria)
    {
        var produtos = await GetAllAsync();
        var produtosPorCategoria = produtos
            .Where(c => c.CategoriaId == idCategoria);
        return produtosPorCategoria;
    }
}
