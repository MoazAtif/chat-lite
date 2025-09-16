using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using tring_to_be_better.DTOS;
using tring_to_be_better.Model;

namespace tring_to_be_better.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class UseresController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbcontext _appDbcontext;
        public UseresController(AppDbcontext appDbcontext, IConfiguration configuration)
        {
            _appDbcontext = appDbcontext;
            _configuration = configuration;

        }
        [HttpPost("register")]
        public IActionResult register(DTORegester dTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = new User
            {
                UserName = dTO.username,
                PasswordHash = dTO.password

            };
            _appDbcontext.users.Add(user);
            _appDbcontext.SaveChanges();
            return Ok(user);

        }
[HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            var user = _appDbcontext.users.FirstOrDefault(a => a.UserName == username && a.PasswordHash == password);
        if (user == null)
            {
                return Unauthorized("invaled username or password");

            }
            var token = GenerateJwtToken(user);

            return Ok(new { token });


        }
        private string GenerateJwtToken(User user) {
            var claims = new[]
                { new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("id", user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cards = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cards




                );
        return new JwtSecurityTokenHandler().WriteToken(token);
        } 
    }
}
