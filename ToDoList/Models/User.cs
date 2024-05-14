using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(80)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8), MaxLength(20)]

        public string Password { get; set; }

        public ICollection<TaskItem> tasks = new List<TaskItem>();
    }
}
