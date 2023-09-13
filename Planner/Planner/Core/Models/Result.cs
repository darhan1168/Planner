namespace Planner.Core.Models;

public class Result<T>
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<T> DataList { get; set; }

    public Result(bool isSuccessful)
    {
        IsSuccessful = isSuccessful;
    }

    public Result(bool isSuccessful, T data)
    {
        IsSuccessful = isSuccessful;
        Data = data;
    }
    
    public Result(bool isSuccessful, List<T> dataList)
    {
        IsSuccessful = isSuccessful;
        DataList = dataList;
    }
    
    public Result(bool isSuccessful, string message)
    {
        IsSuccessful = isSuccessful;
        Message = message;
    }
}