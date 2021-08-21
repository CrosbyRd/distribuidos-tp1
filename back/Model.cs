using System;
using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back
{
    public class UtiContext : DbContext
    {
        public DbSet<Cama> Camas { get; set; }
        public DbSet<Hospital> Hospitales { get; set; }
        public DbSet<Log> Logs { get; set; }
        public string DbPath { get; private set; }

        public UtiContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}uti-distribuidos.db";
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
        
    }
}