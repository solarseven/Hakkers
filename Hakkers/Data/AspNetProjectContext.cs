using Hakkers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hakkers.Data
{
    public partial class AspNetProjectContext : IdentityDbContext
    {
        public IConfiguration Configuration { get; }

        public AspNetProjectContext()
        {
        }

        public AspNetProjectContext(DbContextOptions<AspNetProjectContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<Models.AssignmentCategories> AssignmentCategories { get; set; }
        public virtual DbSet<Models.AssignmentPriorities> AssignmentPriorities { get; set; }
        public virtual DbSet<Models.AssignmentStatuses> AssignmentStatuses { get; set; }
        public virtual DbSet<Models.Assignments> Assignments { get; set; }
        public virtual DbSet<Models.Clients> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AssignmentCategories>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AssignmentPriorities>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<AssignmentStatuses>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.Property(e => e.FkCategory).HasColumnName("FK_Category");

                entity.Property(e => e.FkClient).HasColumnName("FK_Client");

                entity.Property(e => e.FkMechanic)
                    .IsRequired()
                    .HasColumnName("FK_Mechanic")
                    .HasMaxLength(450);

                entity.Property(e => e.FkPlanner)
                    .IsRequired()
                    .HasColumnName("FK_Planner")
                    .HasMaxLength(450);

                entity.Property(e => e.FkPriority).HasColumnName("FK_Priority");

                entity.Property(e => e.FkStatus).HasColumnName("FK_Status");

                entity.HasOne(d => d.FkCategoryNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.FkCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_AssignmentCategory");

                entity.HasOne(d => d.FkClientNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.FkClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_Client");

                entity.HasOne(d => d.FkPriorityNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.FkPriority)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_AssignmentPriority");

                entity.HasOne(d => d.FkStatusNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.FkStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_AssignmentStatus");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}