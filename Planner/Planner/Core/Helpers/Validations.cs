namespace Planner.Core.Helpers;

public static class Validations
{
    public static bool IsUsernameValid(string username)
    {
        return !string.IsNullOrEmpty(username) && username.Length >= 5;
    }

    public static bool IsPasswordValid(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= 8;
    }
}