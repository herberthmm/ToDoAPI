using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Database { 
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relacionamento com ToDos
        public virtual List<ToDo> ToDo { get; set; }
    }
}
