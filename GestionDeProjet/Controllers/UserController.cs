using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using GestionDeProjet.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly DbConfig _context;

        private readonly ILogger<UserController> _logger;

        private UserRepository RoleRepository;

        public UserController(ILogger<UserController> logger, DbConfig context)
        {
            _logger = logger;
            _context = context;
            RoleRepository = new UserRepository(_context);
        }

       
    }
}
