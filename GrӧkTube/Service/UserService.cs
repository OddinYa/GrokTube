using GrӧkTube.Entities;
using Microsoft.AspNetCore.Identity;

namespace GrӧkTube.Service
{
    public class UserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public string GetHashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool IsPasswordCorrect(string password, User user)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
