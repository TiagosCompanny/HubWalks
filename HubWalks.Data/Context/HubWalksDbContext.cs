using HubWalks.Bussines.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubWalks.Data.Context
{
    public class HubWalksDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public HubWalksDbContext(DbContextOptions<HubWalksDbContext> options)
            : base(options)
        {
        }


        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<NotaFiscal>  NotasFicais { get; set; }
        public DbSet<Sdr_Bdr> Sdr_Bdrs { get; set; }
        public DbSet<JobOrder> OrdensDeServico { get; set; }


        //Adiciona as roles do sistema  no migraiton 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // usa um schema diferente do "public"
            builder.HasDefaultSchema("app");

            // seu seeding de roles continua aqui…
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "role-admin", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = "role-user", Name = "User", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() }
            );
        }
    }
}