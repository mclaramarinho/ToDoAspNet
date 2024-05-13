﻿using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> o) : base(o)
        {
            
        }

        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.StartDate)
                .HasColumnName("StartDate");
            
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.EndDate)
                .HasColumnName("EndDate");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasColumnName("Status");

            base.OnModelCreating(modelBuilder);
        }


    }
}
