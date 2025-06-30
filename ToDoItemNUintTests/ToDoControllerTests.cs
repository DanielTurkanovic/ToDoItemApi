using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using ToDoItemApi.Controllers;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;
using ToDoItemApi.Repositories;

namespace ToDoItemNUintTests
{
    public class ToDoControllerTests
    {
        [Test]
        public async Task GetAll_ShouldReturnOkResultWithList()
        {
            // Arrange
            var mockRepo = new Mock<IToDoRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new ToDoItemsController(mockRepo.Object, mockMapper.Object);

            // User simulation
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Dummy data
            var userId = 1;
            var items = new List<ToDoItems> { new ToDoItems { Id = 1, Title = "Test" } };

            mockRepo.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(items);
            mockMapper.Setup(m => m.Map<List<ToDoItemDto>>(items)).Returns(new List<ToDoItemDto>
            {
                 new ToDoItemDto { Title = "Test" }
            });

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var dtoList = okResult.Value as List<ToDoItemDto>;
            Assert.AreEqual(1, dtoList.Count);
        }

        [Test]
        public async Task GetAll_ReturnsOkResultWithMappedDtoList()
        {
            // Arrange
            var mockRepo = new Mock<IToDoRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new ToDoItemsController(mockRepo.Object, mockMapper.Object);

            // Set a fake user in HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var userId = 1;
            var domainItems = new List<ToDoItems>
            {
                new ToDoItems { Id = 1, Title = "Test Task" }
            };
            var dtoItems = new List<ToDoItemDto>
            {
                new ToDoItemDto { Title = "Test Task" }
            };

            mockRepo.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(domainItems);
            mockMapper.Setup(m => m.Map<List<ToDoItemDto>>(domainItems)).Returns(dtoItems);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedDtoList = okResult.Value as List<ToDoItemDto>;
            Assert.IsNotNull(returnedDtoList);
            Assert.AreEqual(1, returnedDtoList.Count);
            Assert.AreEqual("Test Task", returnedDtoList[0].Title);

            // You can check if the repo method was called exactly once
            mockRepo.Verify(r => r.GetAllAsync(userId), Times.Once);
        }
    }
}
