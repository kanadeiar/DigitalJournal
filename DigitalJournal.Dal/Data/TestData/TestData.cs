namespace DigitalJournal.Dal.Data;

public static class TestData
{
    public static class AdminRole
    {
        public static string Name = "admins";
        public static string Description = "Администраторы";
    }
    public static class UserRole
    {
        public static string Name = "users";
        public static string Description = "Пользователи";
    }
    public static class MasterRole
    {
        public static string Name = "masters";
        public static string Description = "Мастера";
    }
    public static class OperatorRole
    {
        public static string Name = "operators";
        public static string Description = "Операторы";
    }


    public static class Admin
    {
        public static string Username = "admin";
        public static string Email = "admin@example.com";
        public static string Password = "secret";
        public static string Rolename = "admins";
    }
    public static class User
    {
        public static string Username = "user";
        public static string Email = "user@example.com";
        public static string Password = "123";
        public static string Rolename = "users";
    }
    public static class Master
    {
        public static string Username = "master";
        public static string Email = "master@example.com";
        public static string Password = "123";
        public static string Rolename = "masters";
    }
    public static class Operator
    {
        public static string Username = "operator";
        public static string Email = "operator@example.com";
        public static string Password = "123";
        public static string Rolename = "operators";
    }
}

