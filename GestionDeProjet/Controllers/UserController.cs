using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using GestionDeProjet.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GestionDeProjet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DbConfig _context;

        private readonly ILogger<UserController> _logger;

        private UserRepository UserRepository;

        public UserController(ILogger<UserController> logger, DbConfig context)
        {
            _logger = logger;
            _context = context;
            UserRepository = new UserRepository(_context);
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return UserRepository.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            IActionResult result;
            User user = UserRepository.GetById(id);

            result = user == null ?
            (IActionResult)NotFound(new { Message = "Utilisateur inexistant !" }) :
            (IActionResult)Ok(user);

            return result;
        }
       
    }
}
