using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatInc.App.Jwt;
using ChatInc.App.Models;
using ChatInc.Data;
using ChatInc.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatInc.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ChatIncDbContext context;
        private readonly JwtSettings jwtSettings;

        public UsersController(ChatIncDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            this.context = context;
            this.jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]UsersBindingModel model)
        {
            this.context.Users.Add(new User
            {
                Username = model.Username,
                Password = model.Password
            });

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]UsersBindingModel model)
        {
            var userFromDb = await this.context.Users
                .SingleOrDefaultAsync(user => user.Username == model.Username
                && user.Password == model.Password);

            if (userFromDb == null)
            {
                return this.BadRequest("Username or password is invalid");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return this.Ok(token);
        }
    }
}