using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;
using ToDoItemApi.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ToDoItemApi.Controllers
{
    [Authorize] // Requires authentication for all endpoints in this controller
    [ApiController]
    [Route("api/[controller]")] // Route: api/ToDoItems
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoRepository toDoRepository;
        private readonly IMapper mapper;

        // Constructor injecting repository and mapper
        public ToDoItemsController(IToDoRepository toDoRepository, IMapper mapper)
        {
            this.toDoRepository = toDoRepository;
            this.mapper = mapper;
        }

        // POST: api/ToDoItems
        // Creates a new ToDo item for the authenticated user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoItemRequestDto request)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var toDoItem = mapper.Map<ToDoItem>(request);
            toDoItem.UserId = userId;

            // Check if a task with the same title already exists for the user
            var exists = await toDoRepository.ExistsByTitleAsync(toDoItem.Title, userId);

            if (exists)
                return BadRequest("A task with the same title already exists.");

            try
            {
                var createdItem = await toDoRepository.CreateAsync(toDoItem, userId);
                var dto = mapper.Map<ToDoItemDto>(createdItem);

                // Return 201 Created with the location of the new resource
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/ToDoItems
        // Returns all ToDo items for the authenticated user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var items = await toDoRepository.GetAllAsync(userId);
            var dtoItems = mapper.Map<List<ToDoItemDto>>(items);

            return Ok(dtoItems);
        }

        // GET: api/ToDoItems/{id}
        // Returns a specific ToDo item by ID if it belongs to the user
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var item = await toDoRepository.GetByIdAsync(id, userId);

            if (item == null)
                return NotFound();

            var dto = mapper.Map<ToDoItemDto>(item);

            return Ok(dto);
        }

        // GET: api/ToDoItems/search?title=...&description=...
        // Searches ToDo items by title and/or description (case-insensitive)
        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitleAndDescription([FromQuery] ToDoSearchRequestDto request)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var items = await toDoRepository.SearchByTitleAndDescriptionAsync(
                request.Title?.Trim().ToLower(),
                request.Description?.Trim().ToLower(),
                userId
            );

            if (items == null || !items.Any())
                return NotFound("No matching items found.");

            var dtos = mapper.Map<List<ToDoItemDto>>(items);

            return Ok(dtos);
        }

        // PUT: api/ToDoItems/{id}
        // Updates a ToDo item by ID if it belongs to the user
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateToDoItemRequestDto request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null");

            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var toDoItem = mapper.Map<ToDoItem>(request);
            toDoItem.Id = id; // Set ID from route parameter

            var updatedItem = await toDoRepository.UpdateAsync(toDoItem, userId);

            if (updatedItem == null)
                return NotFound($"ToDo item with ID {id} not found or access denied.");

            return Ok(mapper.Map<ToDoItemDto>(updatedItem));
        }

        // DELETE: api/ToDoItems/{id}
        // Deletes a ToDo item if it belongs to the user
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("Invalid user ID.");

            var deletedItem = await toDoRepository.DeleteAsync(id, userId);

            if (deletedItem == null)
                return NotFound("Item not found or you don't have permission to delete it.");

            var dto = mapper.Map<ToDoItemDto>(deletedItem);

            return Ok(dto);
        }

        // Extracts the user ID from the JWT token claims
        // Returns true if the user ID was successfully parsed
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdString, out userId);
        }
    }
}
