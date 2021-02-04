using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoFileService : ITodoFileService
    {
        private const string TodoFileName = "todo.json";

        public async Task<List<TodoItem>> LoadDataAsync()
        {
            try
            {
                var content = await File.ReadAllTextAsync(TodoFileName);
                return JsonConvert.DeserializeObject<List<TodoItem>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<TodoItem>();
            }
        }

        public async Task SaveDataAsync(List<TodoItem> todoItems)
        {
            try
            {
                var content = JsonConvert.SerializeObject(todoItems);
                await File.WriteAllTextAsync(TodoFileName, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
