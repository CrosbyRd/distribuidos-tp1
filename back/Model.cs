using System;
using System.Threading;
using System.Threading.Tasks;
using back.Controllers;
using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back
{
    public class UtiContext : DbContext
    {
        public UtiContext(DbContextOptions<UtiContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}uti-distribuidos.db";
        }
        public DbSet<Cama> Camas { get; set; }
        public DbSet<Hospital> Hospitales { get; set; }
        public DbSet<Log> Logs { get; set; }
        public string DbPath { get; private set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries<Cama>();

            /*foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    var newLog = new Log
                    {
                        CamaObjetivo = entry.Entity.Id.ToString(),
                        CreatedAt = DateTime.Now,
                        Descripcion = "Cama añadida",
                        Estado = ,
                        HospitalObjetivo = hospitalObjetivo,
                        NombreOperacion = nombreOperacion
                    };
                    Logs.Add()
                }

                
            }*/

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath};");
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UtiContext).Assembly);
        }
    }
}