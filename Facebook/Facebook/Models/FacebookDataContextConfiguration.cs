using FaceBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookDbContext
{
    internal class FacebookDataContextConfiguration
    {
        internal class UsersConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("Users","dbo").HasKey(x => x.Id).IsClustered();

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.BirthDate).IsRequired().HasColumnType("datetime");
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.CreatedBy).HasMaxLength(255);
                builder.Property(e => e.DeletedAt).HasColumnType("datetime");
                builder.Property(e => e.DeletedBy).HasMaxLength(255);
                builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
                builder.Property(e => e.FirstName).IsRequired().HasMaxLength(255);
                builder.Property(e => e.GenderId).IsRequired().HasColumnName("Gender_Id");
                builder.Property(e => e.LastName).IsRequired().HasMaxLength(255);
                builder.Property(e => e.Password).IsRequired().HasMaxLength(255);
                builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(255);
                builder.Property(e => e.RoleId).IsRequired().HasColumnName("Role_Id");
                builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
                builder.Property(e => e.UpdatedBy).HasMaxLength(255);
                builder.Property(e => e.Bio).HasMaxLength(255);

                builder.HasOne(d => d.Gender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Gender");

                builder.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            }

        }

        internal class GenderConfiguration : IEntityTypeConfiguration<Gender>
        {
            public void Configure(EntityTypeBuilder<Gender> builder)
            {
     
                builder.ToTable("Gender", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.GenderName)
                    .HasColumnName("GenderName")
                    .HasMaxLength(255);
            }
        }

        internal class LikeConfiguration : IEntityTypeConfiguration<Like>
        {
            public void Configure(EntityTypeBuilder<Like> builder)
            {
                builder.ToTable("Likes", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.PostId).HasColumnName("Post_Id");
                builder.Property(e => e.ReactionStatusId).HasColumnName("Reaction_Status_Id");
                builder.Property(e => e.UserId).HasColumnName("User_Id");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.HasOne(d => d.ReactionStatus)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.ReactionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Reaction_Status1");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Users");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Posts");
            }
        }


        internal class PostConfiguration : IEntityTypeConfiguration<Post>
        {
            public void Configure(EntityTypeBuilder<Post> builder)
            {
                builder.ToTable("Posts", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.Property(e => e.PostContent)
                    .IsRequired()
                    .HasColumnName("Post_Content")
                    .HasMaxLength(400);
            }
        }

        internal class SocialStatusConfiguration : IEntityTypeConfiguration<SocialStatus>
        {
            public void Configure(EntityTypeBuilder<SocialStatus> builder)
            {
                builder.ToTable("Social_Status", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            }
        }

        internal class RoleConfiguration : IEntityTypeConfiguration<Role>
        {
            public void Configure(EntityTypeBuilder<Role> builder)
            {
                builder.ToTable("Roles", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(255);
                builder.Property(e => e.Description).HasMaxLength(255);
                builder.Property(e => e.Title).IsRequired().HasMaxLength(255);
                builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
                builder.Property(e => e.UpdatedBy).HasMaxLength(255);
            }
        }


        internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
        {
            public void Configure(EntityTypeBuilder<Comment> builder)
            {
                builder.ToTable("Comments", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CommentContent).IsRequired().HasColumnName("Comment_Content").HasMaxLength(400);
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.PostId).HasColumnName("Post_Id");
                builder.Property(e => e.UserId).HasColumnName("User_Id");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Posts1");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            }
        }

        internal class PostPhotoConfiguration : IEntityTypeConfiguration<PostPhoto>
        {
            public void Configure(EntityTypeBuilder<PostPhoto> builder)
            {
                builder.ToTable("Post_Photos", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.PostId).HasColumnName("Post_Id");
                builder.Property(e => e.Url).IsRequired().HasMaxLength(50);
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.PostPhotos)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Photos_Posts1");

            }
        }

        internal class ProfilePhotoConfiguration : IEntityTypeConfiguration<ProfilePhoto>
        {
            public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
            {
                builder.ToTable("Profile_Photos", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.Url).IsRequired().HasMaxLength(50);
                builder.Property(e => e.UserId).HasColumnName("User_Id");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.HasOne(d => d.User)
                    .WithMany(p => p.ProfilePhotos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_Photos_Users");
            }
        }

        internal class UserRelationConfiguration : IEntityTypeConfiguration<UserRelation>
        {
            public void Configure(EntityTypeBuilder<UserRelation> builder)
            {
                builder.ToTable("User_Relations", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.DesiderId).HasColumnName("Desider_Id");
                builder.Property(e => e.InitiatorId).HasColumnName("Initiator_Id");
                builder.Property(e => e.SocialStatusId).HasColumnName("Social_Status_Id");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.HasOne(d => d.Desider)
                    .WithMany(p => p.UserRelationsDesider)
                    .HasForeignKey(d => d.DesiderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Relations_Users1");
                builder.HasOne(d => d.Initiator)
                    .WithMany(p => p.UserRelationsInitiator)
                    .HasForeignKey(d => d.InitiatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Relations_Users");
                builder.HasOne(d => d.SocialStatus)
                    .WithMany(p => p.UserRelations)
                    .HasForeignKey(d => d.SocialStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Relations_Social_Status1");
            }
        }


        internal class UsersPostConfiguration : IEntityTypeConfiguration<UsersPost>
        {
            public void Configure(EntityTypeBuilder<UsersPost> builder)
            {
                builder.ToTable("Users_Posts", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.PostId).HasColumnName("Post_Id");
                builder.Property(e => e.UserId).HasColumnName("User_Id");

                builder.HasOne(d => d.Post)
                    .WithMany(p => p.UsersPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Posts_Posts");
                builder.HasOne(d => d.User)
                    .WithMany(p => p.UsersPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Posts_Users1");
            }
        }


        internal class RoleActionConfiguration : IEntityTypeConfiguration<RoleAction>
        {
            public void Configure(EntityTypeBuilder<RoleAction> builder)
            {
                builder.ToTable("Role_Actions", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.ActionId).HasColumnName("Action_Id");
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(255);
                builder.Property(e => e.RoleId).HasColumnName("Role_Id");

                builder.HasOne(d => d.Action)
                    .WithMany(p => p.RoleActions)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Actions_Actions");
                builder.HasOne(d => d.Role)
                    .WithMany(p => p.RoleActions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Actions_Roles");
            }
        }


        internal class ActionConfiguration : IEntityTypeConfiguration<Actions>
        {
            public void Configure(EntityTypeBuilder<Actions> builder)
            {
                builder.ToTable("Actions", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.Icon).HasMaxLength(255).HasColumnName("Icon");
                builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
                builder.Property(e => e.ParentId).HasColumnName("Parent_Id");
                builder.Property(e => e.Url).HasMaxLength(255).HasColumnName("Url");
            }
        }


        internal class ReactionStatusConfiguration : IEntityTypeConfiguration<ReactionStatus>
        {
            public void Configure(EntityTypeBuilder<ReactionStatus> builder)
            {
                builder.ToTable("Reaction_Status", "dbo").HasKey(x => x.Id);

                builder.Property(e => e.Id).ValueGeneratedOnAdd();
                builder.Property(e => e.CreatedAt).HasColumnType("datetime");
                builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

                builder.Property(e => e.IconUrl)
                    .IsRequired()
                    .HasColumnName("Icon_Url")
                    .HasMaxLength(255);
                builder.Property(e => e.ReactionName)
                    .IsRequired()
                    .HasColumnName("Reaction_Name")
                    .HasMaxLength(50);
            }
        }
    }
}
