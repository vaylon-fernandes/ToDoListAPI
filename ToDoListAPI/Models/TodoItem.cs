namespace ToDoListAPI.Models;

    

    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string status { get; set; } = "To Do";
        
        public DateTime createdAt { get; set; } = DateTime.Now;
    }

