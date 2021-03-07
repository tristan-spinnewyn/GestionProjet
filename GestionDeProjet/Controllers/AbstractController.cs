using GestionDeProjet.DbContextImplementation.DataContext;
using GestionDeProjet.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GestionDeProjet.DbContextImplementation.Model;
using Task = GestionDeProjet.DbContextImplementation.Model.Task;

namespace GestionDeProjet.Controllers
{
    public class AbstractController<Controller> : ControllerBase
    {
        protected readonly DbConfig _context;

        protected readonly ILogger<Controller> _logger;

        protected ProjectRepository ProjectRepository;

        protected ExigenceRepository ExigenceRepository;

        protected JalonRepository JalonRepository;

        protected TaskRepository TaskRepository;


        public AbstractController(ILogger<Controller> logger, DbConfig context)
        {
            _logger = logger;
            _context = context;
            ProjectRepository = new ProjectRepository(_context);
            ExigenceRepository = new ExigenceRepository(_context);
            JalonRepository = new JalonRepository(_context);
            TaskRepository = new TaskRepository(_context);
        }
        protected bool IsChief()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity.FindFirst("roleId").Value == "1")
            {
                return true;
            }
            return false;
        }

        protected int GetId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                return Int32.Parse(identity.FindFirst("id").Value);
            }

            throw new Exception();
        }

        protected bool Access(int idProject)
        {
            Project Project = ProjectRepository.GetById(idProject);
            int userId = this.GetId();
            bool result = true;

            if(Project == null)
            {
                result = false;
            }
            else
            {
                if (Project.Id != userId && !this.IsChief())
                {
                    Jalon Jalon = JalonRepository.getForUser(idProject, userId);
                    if (Jalon == null)
                    {
                        bool trouver = false;
                        List<Jalon> Jalons = JalonRepository.GetJalonForProject(idProject);
                        foreach(Jalon aJalon in Jalons)
                        {
                            Task task = TaskRepository.getTaskForUser(aJalon.Id, userId);
                            if(task != null)
                            {
                                trouver = true;
                            }

                            TaskRepository.Detach(task);
                            JalonRepository.Detach(aJalon);
                        }

                        if (!trouver)
                        {
                            result = false;
                        }
                    }
                }

                ProjectRepository.Detach(Project);
            }
            
            return result;

        }
    }
}
