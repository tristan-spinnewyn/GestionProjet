using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class Exigence
    {
        public int Id { get; set; }

        public string DescExigence { get; set; }

        public bool IsFonctionnel { get; set; }

        public Project Project { get; set; }

        public int ProjectId { get; set; }

        public TypeExigence TypeExigence { get; set; }

        public int TypeExigenceId { get; set; }

        public ICollection<TaskExigence> TaskExigence { get; set; } = new HashSet<TaskExigence>();

    }
}
