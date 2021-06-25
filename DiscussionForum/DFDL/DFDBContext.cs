using System;
using Microsoft.EntityFrameworkCore;
using DFModels;

namespace DFDL
{
    public class DFDBContext : DbContext
    {
        public DFDBContext() : base()
        {

        }
        // Constructer needed to pass in connection string
        public DFDBContext(DbContextOptions options) : base(options)
        {

        }
        // Insert DBSets here for data models
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Posts> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Comments>()
                .Property(comm => comm.CommentID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Forum>()
                .Property(forum => forum.ForumID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Posts>()
                .Property(post => post.PostID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comments>()
                .HasKey(comm => comm.CommentID);
            modelBuilder.Entity<Forum>()
                .HasKey(forum => forum.ForumID);
            modelBuilder.Entity<Posts>()
                .HasKey(post => post.PostID);
        }
    }
}
