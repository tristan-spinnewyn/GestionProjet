using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.Repository;
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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TypeExigenceController : AbstractController
    {

        private readonly DbConfig _context;

        private readonly ILogger<TypeExigenceController> _logger;

        private TypeExigenceRepository TypeExigenceRepository;

        public TypeExigenceController(ILogger<TypeExigenceController> logger, DbConfig context)
        {
            _logger = logger;
            _context = context;
            TypeExigenceRepository = new TypeExigenceRepository(_context);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            TypeExigence TypeExigence = TypeExigenceRepository.GetById(id);
            if (TypeExigence != null)
            {
                result = Ok(TypeExigence);
            }
            else
            {
                result = NotFound(new { Message = "TypeExigence inexistante!" });
            }


            return result;
        }

        [HttpGet]
        public List<TypeExigence> GetAll()
        {
            return this.TypeExigenceRepository.GetAll();
        }
    }
}
