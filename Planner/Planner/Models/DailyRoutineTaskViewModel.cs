using System.ComponentModel.DataAnnotations;
using Planner.Core.Enums;

namespace Planner.Models;

public class DailyRoutineTaskViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    public bool IsDone { get; set; }
    
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
    
    [Required(ErrorMessage = "Priority is required")]
    public DailyTaskPriority Priority { get; set; }
}