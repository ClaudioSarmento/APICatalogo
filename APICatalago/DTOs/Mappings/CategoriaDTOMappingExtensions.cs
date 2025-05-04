using APICatalago.Domain.Entities;

namespace APICatalago.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if(categoria == null) return null;
            var categoriaDto = new CategoriaDTO()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl

            };
            return categoriaDto;
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
        {
            if (categoriaDto == null) return null;
            var categoria = new Categoria()
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };
            return categoria;
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias) 
        {

            var categoriasDto = categorias?
                .Select(
                    c => new CategoriaDTO
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        ImagemUrl = c.ImagemUrl,
                    }
                ) ?? [];
            return categoriasDto;
        }

    }
}
