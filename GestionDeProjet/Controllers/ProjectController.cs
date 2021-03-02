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

        [HttpPost]
        public IActionResult AddProject([FromBody]Project Project)
        {
            IActionResult result;
            if (!this.IsChief())
            {
                result = Unauthorized(new { Message = "Vous n'avez pas les droits." });
            }
            else if (Project.NameProject == null || Project.NameProject == "")
            {
                result = Unauthorized(new { Message = "Le nom du projet est obligatoire" });
            }
            else
            {
                try
                {
                    this.ProjectRepository.Add(Project);
                    this.ProjectRepository.SaveChanges();
                    result = Ok("Insertion effectué");
                }
                catch
                {
                    result = StatusCode(500);
                }
            }

            return result;
        }

        [HttpPut]
        public IActionResult UpdateProject([FromBody]Project Project)
        {
            IActionResult result;
            if (!this.IsChief())
            {
                result = Unauthorized(new { Message = "Vous n'avez pas les droits." });
            }
            else if (Project.NameProject == null || Project.NameProject == "")
            {
                result = Unauthorized(new { Message = "Le nom du projet est obligatoire" });
            }
            else
            {
                Project UpdateProject = this.ProjectRepository.GetById(Project.Id);
                if(UpdateProject == null)
                {
                    result = NotFound(new { Message = "Le projet n'existe pas" });
                }
                else
                {
                    try
                    {
                        this.ProjectRepository.Detach(UpdateProject);
                        this.ProjectRepository.Update(Project);
                        this.ProjectRepository.SaveChanges();
                        result = Ok("Modification effectué");
                    }
                    catch
                    {
                        result = StatusCode(500);
                    }
                }
            }
            return result;
        }
    }
}
