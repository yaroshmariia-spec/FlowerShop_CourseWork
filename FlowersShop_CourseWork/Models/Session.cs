namespace FlowersShop_CourseWork.Models;

public class Session
{
    public static User CurrentUser { get; set; }

    public static bool IsAdmin => CurrentUser?.Role == "Admin";
    public static bool IsLoggedIn => CurrentUser != null;
        
    public static void Logout()
    {
        CurrentUser = null;
    }
}