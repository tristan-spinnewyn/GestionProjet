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
using Task = GestionDeProjet.DbContextImplementation.Model.Task;

namespace GestionDeProjet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : AbstractController<ProjectController>
    {

        public ProjectController(ILogger<ProjectController> logger, DbConfig context) :base(logger,context)
        {

        }

        [HttpGet]
        public List<Project> Index()
        {

            List<Project> Projects = this.ProjectRepository.GetAll();
            List<Project> ProjectsAccess = new List<Project>();
            if (this.IsChief())
            {
                return Projects;
            }
            else
            {
                
                foreach(Project Project in Projects)
                {
                    if (this.Access(Project.Id))
                    {
                        ProjectsAccess.Add(Project);
                    }
                }

                return ProjectsAccess;
            }

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
                    result = Ok(new { Message = "Insertion effectué" });
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
                        result = Ok(new { Message = "Modification effectué" });
                    }
                    catch
                    {
                        result = StatusCode(500);
                    }
                }
            }
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(id);
            //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné
            if((!this.Access(Project.Id)))
            {
                Project = null;
            }
            result = Project == null ?
            (IActionResult)NotFound(new { Message = "Project inexistant ou vous n'avez pas le droit d'y accéder!" }) :
            (IActionResult)Ok(Project);

            return result;
        }

        [HttpGet("{id}/exigence")]
        public IActionResult getExigence(int id)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(id);
            //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné
            if ((Project == null) && (!this.Access(Project.Id)))
            {
                result = NotFound(new { Message = "Project inexistant ou vous n'avez pas le droit d'y accéder!" });
            }
            else
            {
                result = Ok(this.ExigenceRepository.GetExigencesForProject(id));
            }

            return result;
        }

        [HttpGet("{id}/jalon")]
        public IActionResult getJalon(int id)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(id);
            //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné
            if ((Project == null) && (!this.Access(Project.Id)))
            {
                result = NotFound(new { Message = "Project inexistant ou vous n'avez pas le droit d'y accéder!" });
            }
            else
            {
                List<Jalon> Jalons = new List<Jalon>();
                foreach(Jalon Jalon in this.JalonRepository.GetJalonForProject(id))
                {
                    int Pourcentage = 0;
                    int Nb = 0;
                    foreach(Task Task in this.TaskRepository.GetTaskForJalon(Jalon.Id))
                    {
                        if(Task.DateStartTaskReal != null)
                        {
                            if(Task.DateEndTask != null)
                            {
                                Pourcentage = Pourcentage + 100;
                            }
                            else
                            {
                                Pourcentage = Pourcentage + 50;
                            }
                            
                        }
                        Nb = Nb + 1;
                    }
                    if(Nb != 0)
                    {
                        Pourcentage = Pourcentage / Nb;
                    }
                    Jalon.PourcentageFinish = Pourcentage;

                    Jalons.Add(Jalon);
                }

                result = Ok(Jalons);
            }

            return result;
        }
    }
}
