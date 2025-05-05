using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ProdutosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        [HttpGet("produtos/{categoriaId}")]
        public ActionResult <IEnumerable<ProdutoDTO>> GetProdutosCategoria(int categoriaId)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(categoriaId);
            if (produtos is null) return NotFound();
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {

           
            var produtos = _unitOfWork.ProdutoRepository.GetAll();
            if (!produtos.Any()) return NotFound("Produtos não encontrados...");
            return Ok(produtos);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {

           
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            return Ok(produto);

        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {

            if (produtoDto is null) return BadRequest();
            var produtoCriado = _unitOfWork.ProdutoRepository.Add(produtoDto);
            _unitOfWork.Commit();
            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.Id }, produtoCriado);

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {

            if (id != produtoDto.Id) return BadRequest();
            var produtoAlterado = _unitOfWork.ProdutoRepository.Update(produtoDto);
            _unitOfWork.Commit();
            return Ok(produtoAlterado);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrada");
            var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();
            return Ok(produtoDeletado);

        }

    }
}
