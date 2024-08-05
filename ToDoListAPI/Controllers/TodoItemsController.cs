using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<TodoItem>>>> GetTodoItems()
        {
            var todoItem = await _context.TodoItems.ToListAsync();

            return Ok(new ApiResponse<List<TodoItem>>
            {
                Success = true,
                Message = "Item retrieved successfully",
                Data = todoItem
            });
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound(value: new ApiResponse<TodoItem>
                {
                    Success = false,
                    Message = "Item not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<TodoItem>
            {
                Success = true,
                Message = "Item retrieved successfully",
                Data = todoItem
            });
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest(new ApiResponse<TodoItem>
                {
                    Data = null,
                    Message = "ID Mismatch",
                    Success = false
                });
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound(new ApiResponse<TodoItem>
                    {
                        Success = false,
                        Message = "Item not found",
                        Data = null
                    });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new ApiResponse<TodoItem>
            {
                Success = true,
                Message = "Item updated successfully",
            });
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<TodoItem>
                {
                    Success = false,
                    Message = "Invalid model",
                    Data = null
                });
            }
            try
            {

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
                return Ok(new ApiResponse<TodoItem>
                {
                    Success = true,
                    Message = "Task created successfully",
                    Data = todoItem
                });
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<TodoItem>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                });
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound(new ApiResponse<TodoItem>
                {
                    Success = false,
                    Message = "Task not found",
                    Data = null
                }); 
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TodoItem>
            {
                Success = true,
                Message = "Task deleted successfully",
            });
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
