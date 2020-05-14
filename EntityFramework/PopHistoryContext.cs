using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PopHistoryFunction.EntityFramework
{
    public partial class PopHistoryFunctionContext : DbContext
    {
        public PopHistoryFunctionContext()
        {
        }

        public PopHistoryFunctionContext(DbContextOptions<PopHistoryFunctionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PsaCard> PsaCard { get; set; }
        public virtual DbSet<PsaCustomSet> PsaCustomSet { get; set; }
        public virtual DbSet<PsaCustomSetCard> PsaCustomSetCard { get; set; }
        public virtual DbSet<PsaPopHistoryFunction> PsaPopHistoryFunction { get; set; }
        public virtual DbSet<PsaSet> PsaSet { get; set; }
        public virtual DbSet<Series> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PsaCard>(entity =>
            {
                entity.ToTable("psa_card");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardNumber)
                    .HasColumnName("card_number")
                    .IsUnicode(false);

                entity.Property(e => e.NamePrimary)
                    .HasColumnName("name_primary")
                    .IsUnicode(false);

                entity.Property(e => e.NameSecondary)
                    .HasColumnName("name_secondary")
                    .IsUnicode(false);

                entity.Property(e => e.SetId).HasColumnName("set_id");

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.PsaCard)
                    .HasForeignKey(d => d.SetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("card_set_id_fk");
            });

            modelBuilder.Entity<PsaCustomSet>(entity =>
            {
                entity.ToTable("psa_custom_set");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .IsUnicode(false);

                entity.Property(e => e.SeriesId).HasColumnName("series_id");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.PsaCustomSet)
                    .HasForeignKey(d => d.SeriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("custom_set_series_id_fk");
            });

            modelBuilder.Entity<PsaCustomSetCard>(entity =>
            {
                entity.ToTable("psa_custom_set_card");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.CustomSetId).HasColumnName("custom_set_id");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.PsaCustomSetCard)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("custom_set_card_card_id_fk");

                entity.HasOne(d => d.CustomSet)
                    .WithMany(p => p.PsaCustomSetCard)
                    .HasForeignKey(d => d.CustomSetId)
                    .HasConstraintName("custom_set_card_custom_set_id_fk");
            });

            modelBuilder.Entity<PsaPopHistoryFunction>(entity =>
            {
                entity.ToTable("psa_pop_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pop01).HasColumnName("pop_01");

                entity.Property(e => e.Pop02).HasColumnName("pop_02");

                entity.Property(e => e.Pop03).HasColumnName("pop_03");

                entity.Property(e => e.Pop04).HasColumnName("pop_04");

                entity.Property(e => e.Pop05).HasColumnName("pop_05");

                entity.Property(e => e.Pop06).HasColumnName("pop_06");

                entity.Property(e => e.Pop07).HasColumnName("pop_07");

                entity.Property(e => e.Pop08).HasColumnName("pop_08");

                entity.Property(e => e.Pop085).HasColumnName("pop_08_5");

                entity.Property(e => e.Pop09).HasColumnName("pop_09");

                entity.Property(e => e.Pop095).HasColumnName("pop_09_5");

                entity.Property(e => e.Pop10).HasColumnName("pop_10");

                entity.Property(e => e.PopAuth).HasColumnName("pop_auth");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.PsaPopHistoryFunction)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("pop_history_card_id_fk");
            });

            modelBuilder.Entity<PsaSet>(entity =>
            {
                entity.ToTable("psa_set");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.HeadingId).HasColumnName("heading_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsUnicode(false);

                entity.Property(e => e.SeriesId).HasColumnName("series_id");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.PsaSet)
                    .HasForeignKey(d => d.SeriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("set_series_id_fk");
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.ToTable("series");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .IsUnicode(false);
            });
        }
    }
}
