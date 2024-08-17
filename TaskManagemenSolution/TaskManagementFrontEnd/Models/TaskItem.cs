namespace TaskManagementFrontend.Models
{
    public class TaskItem
    {
        public string Id { get; set; } // Change to string
        public string Title { get; set; } // Rename to match backend
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; } // Add if needed
        public bool IsCompleted { get; set; } // Add if needed
    }
}
