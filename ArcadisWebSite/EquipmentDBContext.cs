using ArcadisWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcadisWebSite
{
    public class EquipmentDBContext : DbContext 
    {
        public EquipmentDBContext(DbContextOptions<EquipmentDBContext> options): base(options)
        {
           // Database.SetInitializer<EquipmentDBContext>(new CreateDatabaseIfNotExists<EquipmentDBContext>());
        }

        public DbSet<Equipment> Equipments { get; set; }
    }
}
