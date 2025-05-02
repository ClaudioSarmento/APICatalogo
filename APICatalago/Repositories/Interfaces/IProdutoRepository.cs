using APICatalago.Domain.Entities;

namespace APICatalago.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int idCategoria);
    }
}
