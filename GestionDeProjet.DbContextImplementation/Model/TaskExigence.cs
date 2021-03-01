using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class TaskExigence
    {
        public int TaskId { get; set; }

        public Task Task { get; set; }

        public int ExigenceId { get; set; }

        public Exigence Exigence { get; set; }
    }
}
