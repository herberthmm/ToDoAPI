using System.ComponentModel.DataAnnotations;
using ToDoList.Enums;

namespace ToDoList.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        
        public bool IsCompleted { get; set; }
        
        public Category Category { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }
    }
}