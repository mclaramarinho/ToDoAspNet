using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ToDoList.Data;

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
        [DataType(DataType.Text)]
        public string TaskTitle { get; set; }

        [Display(Name = "Task Description")]
        [MaxLength(500)]
        [DataType(DataType.Text)]
        public string? TaskDescription { get; set; }

        private DateTime _startDate;
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime StartDate { get => _startDate; }

        private DateTime? _endDate;
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get => _endDate; }

        private string _status;
        [MaxLength(2)]
        [DataType(DataType.Text)]
        public string Status { get => _status; }

        
        public void EndTask(ApplicationDbContext db)
        {
            this._endDate = DateTime.Now.Date;
            this._status = "FI";
            db.TaskItems.Update(this);
            db.SaveChanges();
        }

        public void ReopenTask(ApplicationDbContext db)
        {
            this._endDate = null;
            this._status = "TD";
            db.TaskItems.Update(this);
            db.SaveChanges();
        }

        public void CreateTask()
        {
            this._status = "TD";
            this._startDate = DateTime.Now.Date;
        }

        public void DoTask(ApplicationDbContext db)
        {
            this._status = "DO";
            db.TaskItems.Update(this);
            db.SaveChanges();
        }
    }
}
