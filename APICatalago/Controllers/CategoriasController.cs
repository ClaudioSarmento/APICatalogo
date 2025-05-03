using APICatalago.Domain.Entities;
using APICatalago.Filters;
using APICatalago.Infrastructure.Data.Context;
using APICatalago.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriasController(IUnitOfWork unitOfWork)
        {
          _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            var categorias = _unitOfWork.CategoriaRepository.GetAll(); 
            return Ok(categorias);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            return categoria;

        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {

            if (categoria is null) return BadRequest("Dados inválidos");
            var categoriaPost = _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaPost.Id });

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {

            if (id != categoria.Id) return BadRequest("Dados inválidos");
            var categoriaUpdate = _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();
            return Ok(categoriaUpdate);

        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c =>c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDeletada = _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();
            return Ok(categoria);

        }

    }
}
