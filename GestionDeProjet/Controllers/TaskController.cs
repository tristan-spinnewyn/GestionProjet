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
using Task = GestionDeProjet.DbContextImplementation.Model.Task;

namespace GestionDeProjet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : AbstractController<TaskController>
    {
        private TaskExigenceRepository TaskExigenceRepository;

        public TaskController(ILogger<TaskController> logger, DbConfig context) : base(logger,context)
        {
            this.TaskExigenceRepository = new TaskExigenceRepository(context);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            Task Task = TaskRepository.GetById(id);
            if (Task != null)
            {
                var projectId = this.GetProjectId(Task.JalonId);
                if(projectId != 0)
                {
                    if (!this.Access(projectId))
                    {
                        result = Unauthorized(new { Message = "Vous n'avez pas le droit d'accéder à la ressource demander!" });
                    }
                    else
                    {
                        result = Ok(Task);
                    }
                }
                else
                {
                    result = Unauthorized();
                }
                
            }
            else
            {
                result = NotFound(new { Message = "Tache inexistante!" });
            }
            return result;
        }

        [HttpPost]

        public IActionResult Add([FromBody] Task Task)
        {
            IActionResult result;
            var projectId = this.GetProjectId(Task.JalonId);

            if (projectId != 0)
            {
                if (this.Access(projectId))
                {
                    try
                    {
                        this.TaskRepository.Add(Task);
                        this.TaskRepository.SaveChanges();
                        result = Ok(new { Message = "Insertion effectué" });
                    }
                    catch
                    {
                        result = StatusCode(500);
                    }
                }
                else
                {
                    result = Unauthorized();
                }
                
                
            }
            else
            {
                result = Unauthorized(new { Message = "Vous n'avez pas les droits" });
            }

            return result;
        }

        [HttpPut]
        public IActionResult Update([FromBody] Task Task)
        {
            IActionResult result;
            var projectId = this.GetProjectId(Task.JalonId);

            if(projectId != 0)
            {
                if (this.Access(projectId))
                {
                    Task UpdateTask = this.TaskRepository.GetById(Task.Id);
                    if (UpdateTask == null)
                    {
                        result = NotFound(new { Message = "La tache n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.TaskRepository.Detach(UpdateTask);
                            this.TaskRepository.Update(Task);
                            this.TaskRepository.SaveChanges();
                            result = Ok(new { Message = "Modification effectué" });
                        }
                        catch
                        {
                            result = StatusCode(500);
                        }
                    }
                }
                else
                {
                    result = Unauthorized();
                }
       
            }
            else
            {
                result = Unauthorized(new { Message = "Le projet n'éxiste pas" });
            }
            return result;
        }

        [HttpPut("start")]
        public IActionResult Start([FromBody] Task Task)
        {
            IActionResult result;
            var projectId = this.GetProjectId(Task.JalonId);

            if (projectId != 0)
            {
                if (this.Access(projectId))
                {
                    Task UpdateTask = this.TaskRepository.GetById(Task.Id);
                    if (UpdateTask == null)
                    {
                        result = NotFound(new { Message = "La tache n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.TaskRepository.Detach(UpdateTask);
                            if (Task.TaskIdDepend != null)
                            {
                                Task DependTask = this.TaskRepository.GetById(Task.TaskIdDepend.GetValueOrDefault());
                                if(DependTask.DateEndTask != null)
                                {
                                    Task.DateStartTaskReal = DateTime.Now;
                                    this.TaskRepository.Update(Task);
                                    this.TaskRepository.SaveChanges();
                                    result = Ok(new { Message = "Tache commencé" });
                                }
                                else
                                {
                                    result = StatusCode(401);
                                }
                            }
                            else
                            {
                                Task.DateStartTaskReal = DateTime.Now;
                                this.TaskRepository.Update(Task);
                                this.TaskRepository.SaveChanges();
                                result = Ok(new { Message = "Tache commencé" });
                            }
                            
                        }
                        catch
                        {
                            result = StatusCode(500);
                        }
                    }
                }
                else
                {
                    result = Unauthorized();
                }

            }
            else
            {
                result = Unauthorized(new { Message = "Le projet n'éxiste pas" });
            }
            return result;
        }

        [HttpPut("end")]
        public IActionResult End([FromBody] Task Task)
        {
            IActionResult result;
            var projectId = this.GetProjectId(Task.JalonId);

            if (projectId != 0)
            {
                if (this.Access(projectId))
                {
                    Task UpdateTask = this.TaskRepository.GetById(Task.Id);
                    if (UpdateTask == null)
                    {
                        result = NotFound(new { Message = "La tache n'existe pas" });
                    }
                    else
                    {
                        try
                        {
                            this.TaskRepository.Detach(UpdateTask);
                            if(Task.DateStartTaskReal != null)
                            {
                                Task.DateEndTask = DateTime.Now;
                                this.TaskRepository.Update(Task);
                                this.TaskRepository.SaveChanges();
                                result = Ok(new { Message = "Tache fini" });
                            }
                            else
                            {
                                result = Unauthorized();
                            }
                            
                        }
                        catch
                        {
                            result = StatusCode(500);
                        }
                    }
                }
                else
                {
                    result = Unauthorized();
                }

            }
            else
            {
                result = Unauthorized(new { Message = "Le projet n'éxiste pas" });
            }
            return result;
        }

        [HttpGet("{id}/exigence")]
        public List<TaskExigence> getExigenceLst(int id)
        {
            return this.TaskExigenceRepository.GetByTask(id);
        }
        private int GetProjectId(int idJalon)
        {
            Jalon Jalon = JalonRepository.GetById(idJalon);
            int result = 0;
            if(Jalon != null)
            {
                Project Project = ProjectRepository.GetById(Jalon.ProjectId);
                if(Project != null)
                {
                    result = Project.Id;
                    ProjectRepository.Detach(Project);
                }
            }
            JalonRepository.Detach(Jalon);

            return result;
        }
    }
}
