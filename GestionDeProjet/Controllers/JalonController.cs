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
    public class JalonController : AbstractController<JalonController>
    {

        public JalonController(ILogger<JalonController> logger, DbConfig context) : base(logger,context)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            Jalon Jalon = JalonRepository.GetById(id);
            if (Jalon != null)
            {
                Project Project = ProjectRepository.GetById(Jalon.ProjectId);
                //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné avant de lui transmettre l'exigence
                if (!this.Access(Project.Id))
                {
                    result = NotFound(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    result = Ok(Jalon);
                }
            }
            else
            {
                result = NotFound(new { Message = "Exigence inexistante!" });
            }


            return result;
        }

        [HttpPost]

        public IActionResult Add([FromBody] Jalon Jalon)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(Jalon.ProjectId);
            if (Project != null)
            {
                //on vérifie que l'utilisateur n'est pas un chef et que le projet lui est bien assigné avant de lui transmettre l'exigence
                if (this.GetId() != Project.UserId && !this.IsChief())
                {
                    result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    try
                    {
                        Jalon.DateLivPrev = DateTime.Now;
                        this.JalonRepository.Add(Jalon);
                        this.JalonRepository.SaveChanges();
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
        public IActionResult UpdateJalon([FromBody] Jalon Jalon)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(Jalon.ProjectId);
            if (Project != null)
            {
                if (this.GetId() != Project.UserId && !this.IsChief())
                {
                    result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    Jalon UpdateJalon = this.JalonRepository.GetById(Jalon.Id);
                    if (UpdateJalon == null)
                    {
                        result = NotFound(new { Message = "Le jalon n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.JalonRepository.Detach(UpdateJalon);
                            this.ProjectRepository.Detach(Project);
                            this.JalonRepository.Update(Jalon);
                            this.JalonRepository.SaveChanges();
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

        [HttpPut("livraison")]
        public IActionResult JalonLivraison([FromBody]Jalon Jalon)
        {
            IActionResult result;
            Project Project = ProjectRepository.GetById(Jalon.ProjectId);
            if (Project != null)
            {
                if (!this.Access(Project.Id))
                {
                    result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                }
                else
                {
                    Jalon UpdateJalon = this.JalonRepository.GetById(Jalon.Id);
                    if (UpdateJalon == null)
                    {
                        result = NotFound(new { Message = "Le jalon n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.JalonRepository.Detach(UpdateJalon);
                            this.ProjectRepository.Detach(Project);
                            Jalon.DateLivReel = DateTime.Now;
                            this.JalonRepository.Update(Jalon);
                            this.JalonRepository.SaveChanges();
                            result = Ok(new { Message = "Livraison effectué" });
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

        [HttpGet("{id}/task")]
        public IActionResult GetTask(int id)
        {
            IActionResult result;
            Jalon Jalon = JalonRepository.GetById(id);
            if(Jalon == null)
            {
                result = NotFound(new { Message = "Project inexistant ou vous n'avez pas le droit d'y accéder!" });
            }
            else
            {
                Project Project = ProjectRepository.GetById(Jalon.ProjectId);
                //on vérifie que l'utilisateur peut effectuvé l'action

                if (!this.Access(Project.Id))
                {
                    result = Unauthorized(new { Message = "Project inexistant ou vous n'avez pas le droit d'y accéder!" });
                }
                else
                {
                    result = Ok(this.TaskRepository.GetTaskForJalon(id));
                }
            }
            

            return result;
        }
    }
}
