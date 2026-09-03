namespace TechStock.Domain.Entities;

public static class LoggedUser
{
    public static User? Current { get; private set; }

    public static void Login(User user)
        => Current = user;

    public static void Logout()
        => Current = null;

    public static User? Get()
        => Current;
}
