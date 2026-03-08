using Microsoft.EntityFrameworkCore;
using sistema_de_conta_bancaria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema_de_conta_bancaria
{
    public class BancoContext : DbContext
    {
        public DbSet<ContaBancaria> contass { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BancoDB;Trusted_Connection=true;");
        }
    }
}
