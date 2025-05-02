using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        public ProdutosController(IProdutoRepository respository)
        {
            _repository = respository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {

           
            var produtos = _repository.GetProdutos();
            if (!produtos.Any()) return NotFound("Produtos não encontrados...");
            return Ok(produtos);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {

           
            var produto = _repository.GetProduto(id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            return Ok(produto);

        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {

            if (produto is null) return BadRequest();
            var produtoCriado = _repository.Create(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.Id }, produtoCriado);

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {

            if (id != produto.Id) return BadRequest();
            var produtoAlterado = _repository.Update(produto);
            return Ok(produtoAlterado);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.Delete(id);
            return Ok(produto);

        }
    }
}
