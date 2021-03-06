using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CodeNut247.DotNetCore.ApiAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportFriends.API.Data;
using SportFriends.API.Dto;
using SportFriends.API.Models;

namespace SportFriends.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto User)
        {
            User.Username = User.Username.ToLower();

            if (await _repo.UserExists(User.Username))
                return BadRequest("Username already exists.");

            var userToCreate = new User
            {
                UserName = User.Username
            };

            var createdUser = await _repo.Register(userToCreate, User.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto User)
        {
            var userFromRepo = await _repo.Login(User.Username.ToLower(), User.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };
            
            JwtBuilder tokenbBuilder = 
                new JwtBuilder(
                    claims, 
                    _config.GetSection("AppSettings:Token").Value,
                    DateTime.Now.AddHours(1));

            return Ok(new {
                token = tokenbBuilder.BuildToken()
            });
        }

    }
}