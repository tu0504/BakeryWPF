using Bakery.DAL.Entities;
using Bakery.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Bakery.BLLL.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;

        public AuthService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        private string SimpleHash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashedBytes);
        }

        public (bool Success, string? ErrorMessage) Register(string userName, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(userName)) return (false, "User name is required.");
            if (string.IsNullOrWhiteSpace(email)) return (false, "Email is required.");
            if (string.IsNullOrWhiteSpace(password)) return (false, "Password is required.");
            if (password != confirmPassword) return (false, "Passwords do not match.");
            if (password.Length < 6) return (false, "Password must be at least 6 characters.");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return (false, "Invalid email format.");

            var exists = _userRepo.GetByEmail(email);
            if (exists != null) return (false, "Email already in use.");

            var user = new User
            {
                UserName = userName,
                Email = email,
                Role = "U",
                Password = SimpleHash(password),
                CreatedAt = DateTime.UtcNow
            };

            _userRepo.Add(user);
            _userRepo.SaveChanges();

            return (true, null);
        }

        public (bool Success, string? ErrorMessage, User? User) Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "Email and password are required.", null);

            var user = _userRepo.GetByEmail(email);
            if (user == null) return (false, "Invalid credentials.", null);

            if (user.Password != SimpleHash(password)) return (false, "Invalid credentials.", null);

            return (true, null, user);
        }
    }
}