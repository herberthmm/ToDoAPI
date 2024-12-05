using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Username required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password required.")]
        public string password { get; set; }
    }
}
