using APICatalago.Domain.Entities;
using APICatalago.Pagination;

namespace APICatalago.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria);
    }
}
