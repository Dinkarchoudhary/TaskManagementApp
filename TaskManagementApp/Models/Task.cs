using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.Date, ErrorMessage = "Deadline must be a valid date.")]
        public DateTime Deadline { get; set; }
        public bool IsFavorited { get; set; }
        public string ImagePath { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ColumnId must be a positive integer.")]
        public int ColumnId { get; set; }
    }

    public class TaskColumnDTO
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsFavorited { get; set; }
        public string ImagePath { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
    }
}
