using System;
using System.Security.Cryptography;
using System.Text;

// Static helper class for password hashing
public static class PasswordHashHelper
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

// Instantiable class for password hashing
public class PasswordHashService
{
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Using the static helper class
        string password1 = "password123";
        string hashedPassword1 = PasswordHashHelper.HashPassword(password1);
        Console.WriteLine($"Hashed password using static helper class: {hashedPassword1}");

        // Using the instantiable class
        string password2 = "securepassword456";
        var passwordHashService = new PasswordHashService();
        string hashedPassword2 = passwordHashService.HashPassword(password2);
        Console.WriteLine($"Hashed password using instantiable class: {hashedPassword2}");
    }
}
