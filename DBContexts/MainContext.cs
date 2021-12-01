using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MainServer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MainServer.DBContexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DB.db;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().OwnsMany(p => p.Ratings, r => r.Property(e => e.Marks)
            //    .HasConversion(
            //        v => String.Join(" ", v.Select(p => p.ToString()).ToArray()),
            //        v => Array.ConvertAll(v.Split(), Double.Parse)
            //    ));
            //modelBuilder.Entity<Mark>();
            //modelBuilder.Entity<RatingList>().ToTable("Ratings");
            modelBuilder.Entity<Student>().Property(e => e.Marks)
            .IsRequired()
            .HasConversion(
                d => JsonConvert.SerializeObject(d, Formatting.None),
                s => JsonConvert.DeserializeObject<Dictionary<string, List<Mark>>>(s)
            );
            modelBuilder.Entity<Student>().HasKey(p => p.Username);
            //modelBuilder.Entity<Student>().HasMany(x => x.Lecturers).WithMany(x => x.Students);
            modelBuilder.Entity<Student>().ToTable("Students");


            modelBuilder.Entity<Lecturer>().HasKey(p => p.Username);
            modelBuilder.Entity<Lecturer>().HasMany(x => x.Students).WithMany(x => x.Lecturers);
            modelBuilder.Entity<Lecturer>().ToTable("Lecturers");
        }
    }
}
