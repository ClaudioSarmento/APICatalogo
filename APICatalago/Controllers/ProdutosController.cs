using APICatalago.Domain.Entities;
using APICatalago.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context?.Produtos?.AsNoTracking().ToList();
            if (produtos is null) return NotFound("Produtos não encontrados...");
            return produtos;
        }

        [HttpGet("{id:int}",Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context?.Produtos?.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            if (produto is null) return BadRequest();
            _context?.Produtos?.Add(produto);
            _context?.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new {id = produto.Id}, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) 
        {
            if(id != produto.Id || _context == null) return BadRequest();
            _context.Entry(produto).State = EntityState.Modified;
            _context?.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context?.Produtos?.FirstOrDefault(p => p.Id==id);
            if (produto is null || _context == null) return NotFound("Produto não localizado...");
            _context.Produtos?.Remove(produto);
            _context?.SaveChanges();
            return Ok(produto);
        }
    }
}
