using Planner.Core.Models;

namespace Planner.BLL.Interfaces;

public interface IDateService
{
   Result<DateTime> GetDateTime(string date);
}