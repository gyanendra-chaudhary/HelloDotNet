namespace AllSecurity.Models;

public class DataStore
{
    public static List<SecurityTypes> securityTypesList = new List<SecurityTypes>()
    {
        new SecurityTypes()
        {
            Id = 1, Title = "Basic Authentication",
            Description =
                @"This a simple authentication using hashing or encryption depending upon users choice. It took user email and password to perform encryption or hashing on it.",
            Image =
                "https://static.vecteezy.com/system/resources/previews/005/069/331/non_2x/login-page-icon-flat-on-white-background-free-vector.jpg"
        },
        new SecurityTypes()
        {
            Id = 2,
            Title = "Cookie Based Authentication",
            Description = "",
            Image = ""
        }
    };
}