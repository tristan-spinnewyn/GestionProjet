using System;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using GestionDeProjet.DbContextImplementation.Model;


namespace GestionDeProjet.DbContextImplementation.DataContext
{
    public class DbConfig : DbContext
    {
        public Guid Guid = Guid.NewGuid();

        public DbSet<RoleUsers> RoleUsers { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<TypeExigence> TypeExigences { get; set; }

        public DbSet<Exigence> Exigence { get; set; }

        public DbSet<Jalon> Jalon { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<TaskExigence> TaskExigences { get;set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=gestionprojet;user=root;password=qyh5cbet9xf8;port=3306");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleUsers>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameRole).HasMaxLength(50).IsRequired();
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Firstname).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Lastname).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Trigramme).HasMaxLength(3).IsRequired();
                entity.HasOne(d => d.RoleUsers).WithMany(p => p.Users).HasForeignKey(x => x.RoleUserId);
            });
            
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameProject).HasMaxLength(50).IsRequired();
                entity.HasOne(d => d.User).WithMany(p => p.Projects).HasForeignKey(x => x.UserId);
            });
            
            modelBuilder.Entity<TypeExigence>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameExigence).HasMaxLength(50).IsRequired();
            });
            
            modelBuilder.Entity<Exigence>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DescExigence);
                entity.Property(e => e.IsFonctionnel);
                entity.HasOne(d => d.Project).WithMany(p => p.Exigences).HasForeignKey(x => x.ProjectId);
                entity.HasOne(d => d.TypeExigence).WithMany(p => p.Exigences).HasForeignKey(x => x.TypeExigenceId);
            });
            
            modelBuilder.Entity<Jalon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JalonName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DateLivPrev);
                entity.Property(e => e.DateLivReel);
                entity.HasOne(d => d.Project).WithMany(p => p.Jalons).HasForeignKey(x => x.ProjectId);
                entity.HasOne(d => d.User).WithMany(p => p.Jalons).HasForeignKey(x => x.UserId);
            });
            
            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NameTask).HasMaxLength(50);
                entity.Property(e => e.DescTask).HasMaxLength(50);
                entity.Property(e => e.NbDays);
                entity.Property(e => e.DateStartTaskPrev);
                entity.Property(e => e.DateStartTaskReal);
                entity.Property(e => e.DateEndTask);
                entity.HasOne(d => d.Jalon).WithMany(p => p.Tasks).HasForeignKey(x => x.JalonId);
                entity.HasOne(d => d.User).WithMany(p => p.Tasks).HasForeignKey(x => x.UserId);
                entity.HasOne(d => d.TaskDepend).WithMany(p => p.Tasks).HasForeignKey(x => x.TaskIdDepend);
            });
            
            modelBuilder.Entity<TaskExigence>(entity =>
            {
                entity.HasOne(te => te.Exigence).WithMany(e => e.TaskExigence).HasForeignKey(te => te.ExigenceId).IsRequired();
                entity.HasOne(te => te.Task).WithMany(e => e.TaskExigence).HasForeignKey(te => te.TaskId).IsRequired();
                entity.HasKey(e => new { e.ExigenceId, e.TaskId });
            });
        }
    }
}
