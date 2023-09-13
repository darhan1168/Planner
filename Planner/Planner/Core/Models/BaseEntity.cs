using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Core.Models;

public abstract class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}