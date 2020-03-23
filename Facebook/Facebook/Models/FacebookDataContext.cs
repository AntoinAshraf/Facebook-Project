using System;
using System.Collections.Generic;
using System.Text;
using FaceBook.Models;
using Microsoft.EntityFrameworkCore;
using Facebook.Models.ViewModels;

namespace FacebookDbContext
{
    public class FacebookDataContext : DbContext
    {
        public FacebookDataContext(DbContextOptions<FacebookDataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.UsersConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.ActionConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.CommentConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.GenderConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.LikeConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.PostConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.PostPhotoConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.ProfilePhotoConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.ReactionStatusConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.RoleActionConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.SocialStatusConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.UserRelationConfiguration());
            modelBuilder.ApplyConfiguration(new FacebookDataContextConfiguration.UsersPostConfiguration());
        }

        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<PostPhoto> PostPhotos { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<ProfilePhoto> ProfilePhotos { get; set; }
        public virtual DbSet<ReactionStatus> ReactionStatus { get; set; }
        public virtual DbSet<RoleAction> RoleActions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SocialStatus> SocialStatus { get; set; }
        public virtual DbSet<UserRelation> UserRelations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersPost> UsersPosts { get; set; }
    }
}
