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
    public class TaskExigenceController : AbstractController<TaskExigenceController>
    {
        private TaskExigenceRepository TaskExigenceRepository;
        public TaskExigenceController(ILogger<TaskExigenceController> logger, DbConfig context) : base(logger, context)
        {
            this.TaskExigenceRepository = new TaskExigenceRepository(context);
        }

        [HttpPost]
        public IActionResult Insert([FromBody]TaskExigence TaskExigence)
        {
            IActionResult result;
            try
            {
                this.TaskExigenceRepository.Add(TaskExigence);
                this.TaskRepository.SaveChanges();
                result = Ok(new { Message = "Insertion effectué" });
            }
            catch
            {
                result = StatusCode(500);
            }
            return result;
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]TaskExigence TaskExigence)
        {
            IActionResult result;
            try
            {
                this.TaskExigenceRepository.Delete(TaskExigence);
                this.TaskRepository.SaveChanges();
                result = Ok(new { Message = "Suppression effectué" });
            }
            catch
            {
                result = StatusCode(500);
            }
            return result;
        }

    }
}
