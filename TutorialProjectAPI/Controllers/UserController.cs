using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialProjectAPI.Contexts;
using TutorialProjectAPI.Models;
using TutorialProjectAPI.Repositories;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IdentifiableRepository<UserDB> _userRepository;
        private readonly MainContext _context;

        public UserController(IdentifiableRepository<UserDB> userRepository, MainContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDB user)
        {
            if (user == null)
                return BadRequest();

            _userRepository.Insert(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditUser(Guid id, [FromBody] UserDB user)
        {
            if (user == null || user.Id != id)
                return BadRequest();

            _userRepository.Update(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            _userRepository.Delete(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllAsReadOnly();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
