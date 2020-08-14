using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public AuthController(IAuthRepository repo)
        {
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto User) 
        {
            User.Username = User.Username.ToLower();

            if(await _repo.UserExists(User.Username ))
                return BadRequest("Username already exists.");

            var userToCreate = new User {
                UserName = User.Username 
            };

            var createdUser = await _repo.Register(userToCreate, User.Password);

            return StatusCode(201);
        }

    }
}