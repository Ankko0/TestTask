using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestTask.Models;

namespace TestTask.Controllers
{
    
    [ApiController]
    public class AccountController : Controller
    {

        UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
            if (!db.Users.Any())
            {
                db.Users.Add(new User { Email = "admin@gmail.com", PasswordHash = "12345", GivenName = "admin" });
                db.Users.Add(new User { Email = "qwerty@gmail.com", PasswordHash = "55555", GivenName = "user" });
                db.SaveChanges();
            }
        }


        [HttpPost("/token")]
        public IActionResult Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = db.Users.FirstOrDefault(x => x.Email == username && x.PasswordHash == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.GivenName),
                    new Claim(ClaimTypes.NameIdentifier, person.Id.ToString())
                    

                };
                
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                
                
                return claimsIdentity;
            }

            return null;
        }
        [HttpPost("/register")]
        public IActionResult Register(User user)

        {
            using (db)
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Ok();
                }
                else return BadRequest();

            }
        }
    }
}
