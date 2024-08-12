using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GradinataNaBabaRatka.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GradinataNaBabaRatka.Areas.Identity.Data;

namespace GradinataNaBabaRatka.Data
{
    public class GradinataNaBabaRatkaContext : IdentityDbContext<GradinataNaBabaRatkaUser>
    {
        public GradinataNaBabaRatkaContext (DbContextOptions<GradinataNaBabaRatkaContext> options)
            : base(options)
        {
        }

        public DbSet<GradinataNaBabaRatka.Models.Prodavac> Prodavac { get; set; } = default!;

        public DbSet<GradinataNaBabaRatka.Models.Proizvod>? Proizvod { get; set; }

        public DbSet<GradinataNaBabaRatka.Models.Grad>? Grad { get; set; }

        public DbSet<GradinataNaBabaRatka.Models.Review>? Review { get; set; }

        public DbSet<GradinataNaBabaRatka.Models.Kupuvac>? Kupuvac { get; set; }

        public DbSet<ProizvodGrad>? ProizvodGrad { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
