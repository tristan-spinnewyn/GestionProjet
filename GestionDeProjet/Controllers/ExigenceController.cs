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
    public class ExigenceController : AbstractController<ExigenceController>
    {
        public ExigenceController(ILogger<ExigenceController> logger, DbConfig context) : base(logger,context)
        {
            
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            Exigence Exigence = ExigenceRepository.GetById(id);
            if (Exigence != null)
            {
                Project Project = ProjectRepository.GetById(Exigence.ProjectId);
                //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné avant de lui transmettre l'exigence
                if (!this.Access(Project.Id))
                {
                    result = NotFound(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    result = Ok(Exigence);
                }
            }
            else
            {
                result = NotFound(new { Message = "Exigence inexistante!" });
            }


            return result;
        }

        [HttpPost]

        public IActionResult addExigence([FromBody]Exigence Exigence)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(Exigence.ProjectId);
            if(Project != null)
            {
                //on vérifie que l'utilisateur n'est pas un chef et qu'il n'a pas accés a une des ressource du projet
                if (!this.Access(Project.Id))
                {
                    result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    try
                    {
                        this.ExigenceRepository.Add(Exigence);
                        this.ExigenceRepository.SaveChanges();
                        result = Ok(new { Message = "Insertion effectué" });
                    }
                    catch
                    {
                        result = StatusCode(500);
                    }
                }
            }
            else
            {
                result = NotFound(new { Message = "Le projet n'éxiste pas" });
            }

            return result;
        }

        [HttpPut]
        public IActionResult UpdateExigence([FromBody]Exigence Exigence)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(Exigence.ProjectId);
            if (Project != null)
            {
                if (!this.Access(Project.Id))
                {
                    result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    Exigence UpdateExigence = this.ExigenceRepository.GetById(Exigence.Id);
                    if (UpdateExigence == null)
                    {
                        result = NotFound(new { Message = "L'exigence n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.ExigenceRepository.Detach(UpdateExigence);
                            this.ProjectRepository.Detach(Project);
                            this.ExigenceRepository.Update(Exigence);
                            this.ExigenceRepository.SaveChanges();
                            result = Ok(new { Message = "Modification effectué" });
                        }
                        catch
                        {
                            result = StatusCode(500);
                        }
                    }
                }
            }
            else
            {
                result = NotFound(new { Message = "Le projet n'éxiste pas" });
            }
            return result;
        }
    }
}
