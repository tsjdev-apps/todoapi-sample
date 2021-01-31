using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> _todoItems = new List<TodoItem>();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TodoItem>> Get()
        {
            return Ok(_todoItems);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoItem> GetById(string id)
        {
            var todoItem = GetTodoItem(id);

            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TodoItem> Add(TodoItemRequest todoItemRequest)
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

            _todoItems.Add(todoItem);

            return Ok(todoItem);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(string id)
        {
            var todoItem = GetTodoItem(id);

            if (todoItem == null)
                return NotFound();

            _todoItems.Remove(todoItem);

            return Ok();
        }

        [HttpPut("status/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoItem> Update(string id)
        {
            var todoItem = GetTodoItem(id);

            if (todoItem == null)
                return NotFound();

            todoItem.IsDone = !todoItem.IsDone;
            todoItem.TimeStamp = DateTime.Now;

            return Ok(todoItem);
        }

        private TodoItem GetTodoItem(string id)
        {
            return _todoItems.FirstOrDefault(t => t.Id == id);
        }
    }
}
