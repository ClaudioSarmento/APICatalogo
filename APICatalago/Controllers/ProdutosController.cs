using APICatalago.Domain.Entities;
using APICatalago.DTOs;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;   
            _mapper = mapper;
        }

        [HttpGet("produtos/{categoriaId}")]
        public ActionResult <IEnumerable<ProdutoDTO>> GetProdutosCategoria(int categoriaId)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(categoriaId);
            if (produtos is null) return NotFound();
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {

           
            var produtos = _unitOfWork.ProdutoRepository.GetAll();
            if (!produtos.Any()) return NotFound("Produtos não encontrados...");
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {

           
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrado...");
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);

        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {

            if (produtoDto is null) return BadRequest();
            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoCriado = _unitOfWork.ProdutoRepository.Add(produto);
            _unitOfWork.Commit();
            var produtoDtoCriado = _mapper.Map<ProdutoDTO>(produtoCriado);
            return new CreatedAtRouteResult("ObterProduto", new { id = produtoDtoCriado.Id }, produtoDtoCriado);

        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {

            if (id != produtoDto.Id) return BadRequest();
            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAlterado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();
            var produtoAlteradoDto = _mapper.Map<ProdutoDTO>(produtoAlterado);
            return Ok(produtoAlteradoDto);

        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto is null || id <= 0) return BadRequest();
            var produto = _unitOfWork.ProdutoRepository.Get(p =>  p.Id == id);

            if (produto is null) return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);
            if(!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest)) 
                return BadRequest(ModelState);
            _mapper.Map(produtoUpdateRequest, produto);
            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();
            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }


        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null) return NotFound($"Produto {id} não encontrada");
            var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();
            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);
            return Ok(produtoDeletadoDto);

        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        
    }
}
