using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InteriorCoffee.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using FirebaseAdmin.Auth;

namespace InteriorCoffee.Application.Utils
{
    public class JwtUtil
    {
        //Jwt token for db account
        public static string GenerateJwtToken(Account account, string roleName)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var key = config["Jwt:Key"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, account._id),
                new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                new Claim(ClaimTypes.Role, roleName)
            };

            //Add expiredTime of token
            var expires = DateTime.Now.AddDays(1);

            //Create token
            var token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.Now, expires, credentials);
            return jwtHandler.WriteToken(token);
        }

        //Jwt for google login account
        public static string GenerateJwtToken(UserRecord account, string roleName)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var key = config["Jwt:Key"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, account.Uid),
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

            //Add expiredTime of token
            var expires = DateTime.Now.AddDays(1);

            //Create token
            var token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.Now, expires, credentials);
            return jwtHandler.WriteToken(token);
        }
    }
}
