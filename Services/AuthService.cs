using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TicketApi.Models;
using TicketApi.Models.Auth;

namespace TicketApi.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(User user, string password);
        string GenerateJwtToken(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Hashes the specified plaintext password using a secure hashing algorithm.
        /// </summary>
        /// <param name="password">The plaintext password to hash. Cannot be null or empty.</param>
        /// <returns>A hashed representation of the password, suitable for secure storage.</returns>
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        /// <summary>
        /// Verifies whether the provided password matches the hashed password of the specified user.
        /// </summary>
        /// <param name="user">The user whose password is being verified. Must not be <see langword="null"/>.</param>
        /// <param name="password">The plain-text password to verify. Must not be <see langword="null"/> or empty.</param>
        /// <returns><see langword="true"/> if the provided password matches the user's hashed password; otherwise, <see
        /// langword="false"/>.</returns>
        public bool VerifyPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the JWT is being generated. The user's ID, email, name, and role are included as claims in
        /// the token.</param>
        /// <returns>A string representation of the generated JWT.</returns>
        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
