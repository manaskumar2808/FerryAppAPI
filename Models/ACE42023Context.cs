using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FerryAppApi.Models
{
    public partial class ACE42023Context : DbContext
    {
        public ACE42023Context()
        {
        }

        public ACE42023Context(DbContextOptions<ACE42023Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ManasFerry> ManasFerries { get; set; }
        public virtual DbSet<ManasPort> ManasPorts { get; set; }
        public virtual DbSet<ManasTicket> ManasTickets { get; set; }
        public virtual DbSet<ManasUser> ManasUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DEVSQL.corp.local;Database=ACE 4- 2023;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ManasFerry>(entity =>
            {
                entity.ToTable("ManasFerry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Charge).HasColumnName("charge");

                entity.Property(e => e.Departure)
                    .HasColumnType("datetime")
                    .HasColumnName("departure");

                entity.Property(e => e.DestinationId).HasColumnName("destinationId");

                entity.Property(e => e.Image)
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.OriginId).HasColumnName("originId");

                entity.Property(e => e.RoomsLeft).HasColumnName("roomsLeft");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.ManasFerryDestinations)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManasFerry_ManasPort1");

                entity.HasOne(d => d.Origin)
                    .WithMany(p => p.ManasFerryOrigins)
                    .HasForeignKey(d => d.OriginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManasFerry_ManasPort");
            });

            modelBuilder.Entity<ManasPort>(entity =>
            {
                entity.ToTable("ManasPort");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Image)
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ManasTicket>(entity =>
            {
                entity.ToTable("ManasTicket");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdultCount).HasColumnName("adultCount");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.FerryId).HasColumnName("ferryId");

                entity.Property(e => e.RoomNo).HasColumnName("roomNo");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Ferry)
                    .WithMany(p => p.ManasTickets)
                    .HasForeignKey(d => d.FerryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManasTicket_ManasFerry");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ManasTickets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManasTicket_ManasUser");
            });

            modelBuilder.Entity<ManasUser>(entity =>
            {
                entity.ToTable("ManasUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.Property(e => e.Wallet).HasColumnName("wallet");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
