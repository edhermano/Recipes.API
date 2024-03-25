using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Recipes.API.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Api to get bearer token to test controller authentication
        /// </summary>
        /// <param name="authenticationRequestBody">User Credentials</param>
        /// <returns>Bearer Token</returns>
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));

            var jwtSecurityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                           .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        //Using dummy data to test authentication
        private RecipeUser ValidateUserCredentials(string? userName, string? password)
        {
            return new RecipeUser(
                1,
                userName ?? "",
                "Eric",
                "Herman");
        }

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        private class RecipeUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public RecipeUser(
                int userId,
                string userName,
                string firstName,
                string lastName)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
            }

        }
    }
}
