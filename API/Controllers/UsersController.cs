using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Post(UserDto user)
        {
            var existingUser = await UserExists(user);

            if (existingUser != null)
            {
                existingUser.AmountRequired = user.AmountRequired;
                _context.Entry(existingUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return $"http://localhost:4200/{existingUser.Id}";
            }

            var newUSer = new AppUser
            {
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                AmountRequired = user.AmountRequired
            };

            await _context.Users.AddAsync(newUSer);
            var result = await _context.SaveChangesAsync();

            if (result != 0)
                return $"http://localhost:4200/{newUSer.Id}";

            return BadRequest();
        }

        private async Task<AppUser> UserExists(UserDto user)
        {
            return await _context.Users
            .FirstOrDefaultAsync(x =>
            x.FirstName.ToLower() == user.FirstName.ToLower() &&
            x.LastName.ToLower() == user.LastName.ToLower() &&
            DateTime.Compare(x.DateOfBirth, user.DateOfBirth) == 0);
        }
    }
}