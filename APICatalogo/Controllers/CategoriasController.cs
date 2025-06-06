﻿using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.Filters;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("fixedwindow")]
    [Produces("application/json")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriasController(IUnitOfWork unitOfWork)
        {
          _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Obtem uma lista de objetos Categoria
        /// </summary>
        /// <returns>Uma lista objetos Categoria</returns>
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {

            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);

        }
        
        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasParameters categoriasParameters)
        {

            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            return ObterCategorias(categorias);

        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {

            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltroNome);
            return ObterCategorias(categorias);

        }

        /// <summary>
        /// Obtem uma Categoria pelo seu Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDto = categoria.ToCategoriaDTO();
            return categoriaDto!;

        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/categorias
        ///     {
        ///         "categoriaId": 1,
        ///         "nome": "categoria1",
        ///         "imagemUrl": "http://teste.net/1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaDTO>> Post([FromBody] CategoriaDTO categoriaDto)
        {

            if (categoriaDto is null) return BadRequest("Dados inválidos");
            var categoria = categoriaDto.ToCategoria();
            var categoriaPost = _unitOfWork.CategoriaRepository.Add(categoria!);
            await _unitOfWork.CommitAsync();
            var categoriaDtoPost = categoriaPost.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDtoPost!.Id });

        }

        [HttpPut("{id:int:min(1)}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {

            if (id != categoriaDto.Id) return BadRequest("Dados inválidos");
            var categoria = categoriaDto.ToCategoria();
            var categoriaUpdate = _unitOfWork.CategoriaRepository.Update(categoria!);
            await _unitOfWork.CommitAsync();
            var categoriaDtoUpdate = categoriaUpdate.ToCategoriaDTO();
            return Ok(categoriaDtoUpdate);

        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoriaDTO>> DeleteAsync(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c =>c.Id == id);
            if (categoria is null) return NotFound($"Categoria {id} não encontrada");
            var categoriaDeletada = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();
            var categoriaDto = categoriaDeletada.ToCategoriaDTO();
            return Ok(categoriaDto);

        }

    }
}
