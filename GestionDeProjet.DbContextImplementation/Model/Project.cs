using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class Project
    {
        public int Id { get; set; }

        public string NameProject { get; set; }

        public User? User { get; set; }

        public int? UserId { get; set; }

        public ICollection<Exigence> Exigences { get; set; } = new HashSet<Exigence>();

        public ICollection<Jalon> Jalons { get; set; } = new HashSet<Jalon>();

    }
}
