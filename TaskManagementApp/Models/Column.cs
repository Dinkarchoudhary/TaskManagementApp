using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class Column
    {
        public int ColumnId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
