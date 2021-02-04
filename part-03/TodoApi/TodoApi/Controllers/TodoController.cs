using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoFileService _todoFileService;

        public TodoController(ITodoFileService todoFileService)
        {
            _todoFileService = todoFileService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TodoItem>>> GetAsync()
        {
            var todoItems = await _todoFileService.LoadDataAsync();
            return Ok(todoItems);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> GetByIdAsync(string id)
        {
            var todoItem = await GetTodoItemAsync(id);

            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItem>> AddAsync(TodoItemRequest todoItemRequest)
        {
            if (string.IsNullOrEmpty(todoItemRequest.Content))
                return BadRequest("No content provided");

            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid().ToString(),
                Content = todoItemRequest.Content,
                IsDone = todoItemRequest.IsDone,
                TimeStamp = DateTime.Now
            };

            var todoItems = await _todoFileService.LoadDataAsync();
            todoItems.Add(todoItem);
            await _todoFileService.SaveDataAsync(todoItems);

            return Ok(todoItem);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var todoItems = await _todoFileService.LoadDataAsync();
            var todoItem = todoItems.FirstOrDefault(t => t.Id == id);

            if (todoItem == null)
                return NotFound();

            todoItems.Remove(todoItem);
            await _todoFileService.SaveDataAsync(todoItems);

            return Ok();
        }

        [HttpPut("status/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> UpdateAsync(string id)
        {
            var todoItems = await _todoFileService.LoadDataAsync();
            var todoItem = todoItems.FirstOrDefault(t => t.Id == id);

            if (todoItem == null)
                return NotFound();

            todoItem.IsDone = !todoItem.IsDone;
            todoItem.TimeStamp = DateTime.Now;

            await _todoFileService.SaveDataAsync(todoItems);

            return Ok(todoItem);
        }

        private async Task<TodoItem> GetTodoItemAsync(string id)
        {
            var todoItems = await _todoFileService.LoadDataAsync();
            return todoItems.FirstOrDefault(t => t.Id == id);
        }
    }
}
