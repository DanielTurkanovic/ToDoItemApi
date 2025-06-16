using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;
using ToDoItemApi.Repositories;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoRepository toDoRepository;
        private readonly IMapper mapper;
       
        public ToDoItemsController(IToDoRepository toDoRepository, IMapper mapper)
        {
            this.toDoRepository = toDoRepository;
            this.mapper = mapper;
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoItemRequestDto request)
        {
            // Map DTO to Daomain Model
            var toDoItem = mapper.Map<ToDoItems>(request);

            // CHECK: does a task with the same title already exist
            var exists = await toDoRepository.ExistsByTitleAsync(toDoItem.Title);
            if (exists)
            {
                return BadRequest("A task with the same title already exists.");
            }

            try
            {
                // Trying to write to the database
                var createdItem = await toDoRepository.CreateAsync(toDoItem);

                // Map Domain Model to DTO
                var dto = mapper.Map<ToDoItemDto>(createdItem);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }


        // GET: api/ToDoItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await toDoRepository.GetAllAsync();

            var dtoItems = mapper.Map<List<ToDoItemDto>>(items);

            return Ok(dtoItems);
        }


        // GET: api/ToDoItems/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await toDoRepository.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            var dto = mapper.Map<ToDoItemDto>(item);

            return Ok(dto);
        }

        // GET: api/ToDoItems/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitleAndDescription([FromQuery] ToDoSearchRequestDto request)
        {
            var items = await toDoRepository.SearchByTitleAndDescriptionAsync(
                request.Title?.Trim().ToLower(),
                request.Description?.Trim().ToLower()
            );

            if (items == null || !items.Any())
            {
                return NotFound("No matching items found.");
            }

            var dtos = mapper.Map<List<ToDoItemDto>>(items);

            return Ok(dtos);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ToDoItemRequestDto request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null");

            // Map the DTO to ToDoItem and set the ID from the route.
            var toDoItem = mapper.Map<ToDoItems>(request);

            // Ensures that we update the correct item.
            toDoItem.Id = id;

            // Call UpdateAsync (which internally performs all checks and updates)
            var updatedItem = await toDoRepository.UpdateAsync(toDoItem);

            if (updatedItem == null)
                return NotFound($"ToDo item with ID {id} not found");

            // Return the updated DTO
            return Ok(mapper.Map<ToDoItemDto>(updatedItem));
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var existingItem = await toDoRepository.GetByIdAsync(id);

            if (existingItem == null)
                return NotFound("Item is not found");

            await toDoRepository.DeleteAsync(id);

            var dto = mapper.Map<ToDoItemDto>(existingItem);

            return Ok(dto);
        }
    }
}
