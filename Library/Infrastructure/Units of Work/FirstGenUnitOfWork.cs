using System;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;

namespace Music.Infrastructure.UnitsOfWork
{
    public class FirstGenUnitOfWork : DbContext, IUnitOfWork
    {
        public DbSet<Record> Records { get; set; }

        public FirstGenUnitOfWork(DbContextOptions<FirstGenUnitOfWork> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>(record =>
            {
                record.HasKey("Id");
                record.Property("Id").HasColumnName("record_id");
                record.Property(p => p.ConcurrencyStamp).ForNpgsqlHasColumnName("xmin").ForNpgsqlHasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
                record.ToTable("record");
            });

            base.OnModelCreating(builder);
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void Rollback()
        {
            Dispose();
        }
    }
}