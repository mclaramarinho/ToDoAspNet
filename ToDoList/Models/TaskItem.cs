using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class TaskItem
    {
        [Key]
        [Display(Name = "Task ID")]
        public Guid TaskID { get; set; }

        [Required]
        [Display(Name = "Task Title")]
        [MaxLength(50)]
        public string TaskTitle { get; set; }

        [Display(Name = "Task Description")]
        [MaxLength(500)]
        public string TaskDescription { get; set; }

        private DateOnly _startDate;
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateOnly StartDate { get => _startDate; }

        private DateOnly? _endDate;
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateOnly? EndDate { get => _endDate; }

        private string _status;
        [MaxLength(2)]
        [RegularExpression(@"/(TD|DO|FI)/")]
        public string Status { get => _status; }

        
        public void EndTask()
        {
            this._endDate = DateOnly.FromDateTime(DateTime.Now);
            this._status = "FI";
        }

        public void ReopenTask()
        {
            this._endDate = null;
            this._status = "DO";
        }

        public void CreateTask()
        {
            this.TaskID = Guid.NewGuid();
            this._status = "TD";
            this._startDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public void DoTask()
        {
            this._status = "DO";
        }
    }
}
