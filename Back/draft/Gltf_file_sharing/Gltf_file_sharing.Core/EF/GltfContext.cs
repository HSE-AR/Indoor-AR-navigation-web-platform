using Gltf_file_sharing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Core.EF
{
    public class GltfContext : DbContext
    {
        public DbSet<GltfFile> GltfFiles { get; set; }

        public GltfContext(DbContextOptions<GltfContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
