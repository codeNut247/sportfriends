using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportFriends.API.Data;
using SportFriends.API.Models;

namespace SportFriends.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController: ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetValues(int id) 
        {
            return Ok(await _context.Values.FirstOrDefaultAsync(_ => _.Id == id));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetValues() 
        {
            return Ok(await _context.Values.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostValue([FromBody]Value entity) 
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutValue(int id, [FromBody]Value entity) 
        {
            var value = await _context.Values.SingleOrDefaultAsync(v => v.Id == id);
            
            if (value == null) {
                entity.Id = id;
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Created("", entity);
            } else {
                value.Name = entity.Name;
                _context.Values.Update(value);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}