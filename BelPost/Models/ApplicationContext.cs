using BelPost.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace BelPost.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Dialog> Dialogs { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<PostageStamp> PostageStamps { get; set; }

        public virtual DbSet<PostalLetterCard> PostalLetterCards { get; set; }

        public virtual DbSet<StampsUser> StampsUsers { get; set; }

        public virtual DbSet<UserCard> UserCards { get; set; }
        public virtual DbSet<ProfileView> ProfileView { get; set; }
        public virtual DbSet<BlockRecord> BlockRecord { get; set; }
        public virtual DbSet<CategoryForUserViewModel> CategoryView { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<ProfileView>((pc =>
            //{
            //    pc.HasNoKey();
            //    pc.ToView("ProfileView");
            //    pc.Property(e => e.Email).HasMaxLength(256);
            //    pc.Property(e => e.Id).HasMaxLength(450);
            //    pc.Property(e => e.UserName).HasMaxLength(256);
            //}));
        }
    }
}
