using GrӧkTube.Entities;
using Microsoft.AspNetCore.Identity;

namespace GrӧkTube.Service
{
    public class UserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public string GetHashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool IsPasswordCorrect(string password, User user)
        {
           if(user.HashPassword != GetHashPassword(password))
            {
                return false;
            }
           return true;
        }
    }
}
