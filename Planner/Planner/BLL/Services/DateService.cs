using Planner.BLL.Interfaces;
using Planner.Core.Models;

namespace Planner.BLL.Services;

public class DateService : IDateService
{
    public Result<DateTime> GetDateTime(string date)
    {
        var currentDate = DateTime.Today;

        if (!int.TryParse(date, out var day))
        {
            return new Result<DateTime>(false, $"Invalid format for date: {date}");
        }
        
        var selectedDate = new DateTime(currentDate.Year, currentDate.Month, day);

        return new Result<DateTime>(true, selectedDate);;
    }
}