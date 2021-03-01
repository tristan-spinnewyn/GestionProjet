using System;
using System.Collections.Generic;
using System.Text;

namespace GestionDeProjet.DbContextImplementation.Model
{
    public class Task
    {
        public int Id { get; set; }

        public string NameTask { get; set; }

        public string DescTask { get; set; }

        public int NbDays { get; set; }

        public DateTime DateStartTaskPrev { get; set; }

        public DateTime DateStartTaskReal { get; set; }

        public DateTime DateEndTask { get; set; }

        public Jalon Jalon { get; set; }

        public int JalonId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public Task TaskDepend { get; set; }

        public int TaskIdDepend { get; set; }

        public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();

        public ICollection<TaskExigence> TaskExigence { get; set; } = new HashSet<TaskExigence>();

    }
}
