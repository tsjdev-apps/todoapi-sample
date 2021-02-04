using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
