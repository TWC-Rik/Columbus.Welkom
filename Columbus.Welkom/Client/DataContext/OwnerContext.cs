﻿using Columbus.Welkom.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Columbus.Welkom.Client.DataContext
{
    public class OwnerContext : DbContext
    {
        public OwnerContext(DbContextOptions<OwnerContext> opts) : base(opts) { }

        public DbSet<OwnerEntity> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OwnerEntity>().ToTable("Owner");
            modelBuilder.Entity<OwnerEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<OwnerEntity>().HasIndex(e => e.Year);
            modelBuilder.Entity<OwnerEntity>().HasMany(o => o.Pigeons);
        }
    }
}
