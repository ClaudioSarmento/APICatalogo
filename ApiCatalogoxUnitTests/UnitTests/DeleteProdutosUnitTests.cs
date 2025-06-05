using APICatalago.Controllers;
using ApiCatalagoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class DeleteProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public DeleteProdutosUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProduto_Return_OkResult()
        {
            // Act
            var data = await _controller.DeleteAsync(683);

            // Assert
            var okResult = data.Should().BeOfType<OkObjectResult>();
            okResult.Subject.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task PutProduto_Return_NotFound()
        {

            // Act
            var data = await _controller.DeleteAsync(683);

            // Assert
            var notFoundResult = data.Should().BeOfType<NotFoundObjectResult>();
            notFoundResult.Subject.StatusCode.Should().Be(404);

        }

    }
}
