using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoFileService
    {
        Task<List<TodoItem>> LoadDataAsync();
        Task SaveDataAsync(List<TodoItem> todoItems);
    }
}