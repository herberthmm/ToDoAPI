using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ToDoModel> ToDos { get; set; }
    }
}