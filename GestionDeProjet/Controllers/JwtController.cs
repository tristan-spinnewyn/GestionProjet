using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.DbContextImplementation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private IUserService UserService { get; }

        private readonly DbConfig _context;

        private readonly ILogger<JwtController> _logger;


        public JwtController(ILogger<JwtController> logger,IUserService UserService, DbConfig context)
        {
            this.UserService = UserService;
            this._context = context;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult Auth([FromBody]User AnUser)
        {
            IActionResult result;

            User user = this.UserService.Auth(AnUser.Email, AnUser.Password, _context);

            result = user == null ?
            (IActionResult)Unauthorized(new { Message = "Nom d'utilisateur ou mot de passe incorrect." }) :
            (IActionResult)Ok(user);

            // Retour.
            return result;
        }
    }
}
