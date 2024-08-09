using SkillProfiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SkillProfiApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<MenuTitles> MenuTitles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}