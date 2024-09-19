using ProxyResource.Interfaces;
using ProxyResource.Models;
using ProxyResource.Services;
using ProxyResource.Controllers;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestResource
{
    public class Test
    {
        public class ResourceControllerTests
        {
            private readonly Mock<IResourceService> _resourceServiceMock;
            private readonly ResourceController _controller;

            public ResourceControllerTests()
            {
                _resourceServiceMock = new Mock<IResourceService>();
                _controller = new ResourceController(_resourceServiceMock.Object);

                // Set up HttpContext for the controller
                var httpContext = new DefaultHttpContext();
                _controller.ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                };
            }

            [Fact]
            public async Task GetResource_ReturnsNotFound_WhenResourceDoesNotExist()
            {
                // Arrange
                int resourceId = 1;
                _resourceServiceMock.Setup(s => s.GetResourceById(resourceId))
                    .ReturnsAsync((Resource)null);

                // Act
                var result = await _controller.GetResource(resourceId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal($"Resource with id {resourceId} not found.", notFoundResult.Value);
            }

            [Fact]
            public async Task GetResource_ReturnsFullResource_WhenHeaderPresent()
            {
                // Arrange
                int resourceId = 1;
                var resource = new Resource { Id = resourceId, Name = "Test", Color = "Red" };
                _resourceServiceMock.Setup(s => s.GetResourceById(resourceId))
                    .ReturnsAsync(resource);

                _controller.HttpContext.Items["FullHeaderPresent"] = true;

                // Act
                var result = await _controller.GetResource(resourceId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedResource = Assert.IsType<Resource>(okResult.Value);
                Assert.Equal(resource.Id, returnedResource.Id);
                Assert.Equal(resource.Name, returnedResource.Name);
                Assert.Equal(resource.Color, returnedResource.Color);
            }

           
        }
    }
}
