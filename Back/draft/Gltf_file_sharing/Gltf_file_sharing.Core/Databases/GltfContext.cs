﻿using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;


namespace Gltf_file_sharing.Core.EF
{
    public class GltfContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<GltfFile> GltfFiles { get; set; }

        public GltfContext(DbContextOptions<GltfContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>[]
                {
                    new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = "admin",
                        NormalizedName = "ADMIN"
                    }
                });

            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>[]
                {
                     new IdentityRole<Guid>
                     {
                        Id = Guid.NewGuid(),
                        Name = "superadmin",
                        NormalizedName = "SUPERADMIN"
                     }
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
