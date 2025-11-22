using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<FollowRequest> FollowRequests { get; set; }
        public DbSet<UserProfileInfo> UserProfileInfos { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<UserWorkExperience> UserWorkExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ---------- UserFollow (Follow sistemi) ----------
            builder.Entity<UserFollow>(entity =>
            {
                entity.HasKey(uf => uf.Id);

                // Məni follow edənlər (Followers)
                entity.HasOne(uf => uf.Following)        // kimi follow edir
                      .WithMany(u => u.Followers)        // onun follower-ləri
                      .HasForeignKey(uf => uf.FollowingId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Mənim follow etdiklərim (Following)
                entity.HasOne(uf => uf.Follower)         // follow edən
                      .WithMany(u => u.Following)        // onun follow etdikləri
                      .HasForeignKey(uf => uf.FollowerId)
                      .OnDelete(DeleteBehavior.NoAction);

                // İstəsən uniqueness də qoya bilərsən: bir user eyni adamı 2 dəfə follow etməsin
                entity.HasIndex(uf => new { uf.FollowerId, uf.FollowingId }).IsUnique();
            });

            // ---------- Message (private mesajlaşma) ----------
            builder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- FollowRequest (private profil üçün follow istəyi) ----------
            builder.Entity<FollowRequest>(entity =>
            {
                entity.HasKey(fr => fr.Id);

                entity.HasOne(fr => fr.FromUser)
                      .WithMany() // istəsən FromUserRequests kimi collection aça bilərsən
                      .HasForeignKey(fr => fr.FromUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(fr => fr.ToUser)
                      .WithMany() // istəsən ToUserRequests kimi collection aça bilərsən
                      .HasForeignKey(fr => fr.ToUserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- Notification ----------
            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.FromUser)
                      .WithMany()  // İstəsən FromUserNotifications üçün collection aça bilərsən
                      .HasForeignKey(n => n.FromUserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- Post ----------
            builder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.AppUser)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // ---------- PostComment ----------
            builder.Entity<PostComment>(entity =>
            {
                entity.HasKey(pc => pc.Id);

                entity.HasOne(pc => pc.Post)
                      .WithMany(p => p.PostComments)
                      .HasForeignKey(pc => pc.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pc => pc.AppUser)
                      .WithMany() // istəsən UserComments collection əlavə edə bilərsən
                      .HasForeignKey(pc => pc.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ---------- PostLike ----------
            builder.Entity<PostLike>(entity =>
            {
                entity.HasKey(pl => pl.Id);

                entity.HasOne(pl => pl.Post)
                      .WithMany(p => p.PostLikes)
                      .HasForeignKey(pl => pl.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pl => pl.AppUser)
                      .WithMany() // istəsən UserLikes collection əlavə edə bilərsən
                      .HasForeignKey(pl => pl.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(pl => new { pl.PostId, pl.UserId }).IsUnique();
            });
            // ---------- UserProfileInfo (1:1) ----------
            builder.Entity<UserProfileInfo>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.User)
                      .WithOne(u => u.UserProfileInfo)
                      .HasForeignKey<UserProfileInfo>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- UserWorkExperience (1:N) ----------
            builder.Entity<UserWorkExperience>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.HasOne(w => w.User)
                      .WithMany(u => u.WorkExperiences)
                      .HasForeignKey(w => w.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- UserEducation (1:N) ----------
            builder.Entity<UserEducation>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Educations)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- UserSocialLink ----------
            builder.Entity<UserSocialLink>(entity =>
            {
                entity.HasIndex(x => new { x.UserId, x.Platform }).IsUnique();
                entity.Property(x => x.Url)
                      .IsRequired()
                      .HasMaxLength(40);

                entity.Property(x => x.Platform)
                      .HasConversion<int>();

                entity.HasOne(x => x.User)
                      .WithMany(u => u.SocialLinks)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ------------ UserProfileVisibility ----------- >
            builder.Entity<UserProfileVisibility>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Visibility).HasConversion<int>();
                entity.Property(v => v.Field).HasConversion<int>();
                entity.HasOne(v => v.User)
                      .WithMany(u => u.UserProfileVisibilities)
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(v => new { v.UserId, v.Field }).IsUnique();
            });

        }
    }
}
