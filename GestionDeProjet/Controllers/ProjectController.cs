using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GestionDeProjet.DbContextImplementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDeProjet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : AbstractController
    {

        private readonly DbConfig _context;

        private readonly ILogger<ProjectController> _logger;

        private ProjectRepository ProjectRepository;

        public ProjectController(ILogger<ProjectController> logger, DbConfig context)
        {
            _logger = logger;
            _context = context;
            ProjectRepository = new ProjectRepository(_context);
        }
        [HttpGet]
        public List<Project> Index()
        {

            List<Project> Projects;
            if (this.IsChief())
            {
                Projects = this.ProjectRepository.GetAll();
            }
            else
            {
                Projects = this.ProjectRepository.GetProjectAssign(this.GetId());
            }


            return Projects;
        }

        
    }
}
