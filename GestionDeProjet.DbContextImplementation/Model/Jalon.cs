using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class Jalon
    {
        public int Id { get; set; }

        public DateTime DateLivPrev { get; set; }

        public DateTime? DateLivReel { get; set; }

        public string JalonName { get; set; }

        public Project Project { get; set; }

        public int ProjectId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();

    }
}
