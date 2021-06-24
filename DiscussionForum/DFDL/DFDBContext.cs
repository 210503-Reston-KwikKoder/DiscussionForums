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
        public DbSet<DogList> DogLists { get; set; }
        public DbSet<ListedDog> ListedDogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Alert> Alert { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DogList>()
                .Property(list => list.ListID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Comments>()
                .Property(comm => comm.CommentID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Forum>()
                .Property(forum => forum.ForumID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Posts>()
                .Property(post => post.PostID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Tags>()
                .Property(tag => tag.TagID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Alert>()
                .Property(alert => alert.AlertID)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<User>()
                .HasKey(user => user.UserID);
            modelBuilder.Entity<ListedDog>()
                .HasKey(LDog => new { LDog.ListID, LDog.APIID });
            modelBuilder.Entity<DogList>()
                .HasKey(list => list.ListID);
            modelBuilder.Entity<Like>()
                .HasKey(like => new { like.DogID, like.UserName });
            modelBuilder.Entity<Comments>()
                .HasKey(comm => comm.CommentID);
            modelBuilder.Entity<Forum>()
                .HasKey(forum => forum.ForumID);
            modelBuilder.Entity<Posts>()
                .HasKey(post => post.PostID);
            modelBuilder.Entity<Preference>()
                .HasKey(pref => new { pref.TagID, pref.UserName });
            modelBuilder.Entity<Tags>()
                .HasKey(tag => tag.TagID);


        }
    }
}
