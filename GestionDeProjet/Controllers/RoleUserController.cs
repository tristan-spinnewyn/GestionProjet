using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using GestionDeProjet.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GestionDeProjet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleUserController : AbstractController<RoleUserController>
    {

        private RoleRepository RoleRepository;

        public RoleUserController(ILogger<RoleUserController> logger,DbConfig context) : base (logger,context)
        {
            RoleRepository = new RoleRepository(_context);
        }

        [HttpGet]
        public List<RoleUsers> Get()
        {
            _logger.LogInformation("Get All RoleUser");
            _logger.LogInformation(this.IsChief().ToString());
            return this.RoleRepository.GetAll();
        }

        [HttpGet("user/{id}")]
        public RoleUsers GetRole(int id)
        {
            _logger.LogInformation("Get role for user "+id);
            return _context.User.Where(u => u.Id == id).Include(u => u.RoleUsers).First().RoleUsers;
            
        }

        [HttpGet("{id}/getUsers")]
        public List<User> GetUsers(int id)
        {
            return _context.RoleUsers.Where(r => r.Id == id).Include(r => r.Users).First().Users.ToList<User>();
            

        }
    }
}
