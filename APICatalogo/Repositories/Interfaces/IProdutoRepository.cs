using APICatalago.Domain.Entities;
using APICatalago.Pagination;

namespace APICatalago.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int idCategoria);
        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
    }
}
