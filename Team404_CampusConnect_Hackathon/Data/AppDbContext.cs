using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Team404_CampusConnect_Hackathon.Models;

namespace Team404_CampusConnect_Hackathon.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets = tables in SQLite. We basically have a table for every model. In our generic repository + interface we will pass the model through
        // and Entity framework will link it to the DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupChatMessage> GroupChatMessages { get; set; }
        public DbSet<GroupEvent> GroupEvents { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }
        public DbSet<GroupAnnouncement> GroupAnnouncements { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<StudyMaterial> StudyMaterials { get; set; }
        public DbSet<MaterialEntry> MaterialEntries { get; set; }
        public DbSet<StudyMaterialReport> StudyMaterialReports { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many for User Skills
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSkills)
                .WithMany(s => s.Users);

            // Configure many-to-many for User Sports
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSports)
                .WithMany(s => s.Users);
        }
    }
}


