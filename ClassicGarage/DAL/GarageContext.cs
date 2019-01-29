using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ClassicGarage.Models;

namespace ClassicGarage.DAL
{
    public class GarageContext:DbContext
    {

        public GarageContext():base("name=BazaGaraz")
        {

        }

        public DbSet<CarModels> Car { get; set; }
  
        public DbSet<OwnerModels> Owner { get; set; }

        public DbSet<PartModels> Parts { get; set; }

        public DbSet<RepairModels> Reapirs { get; set; }

        public DbSet<AdModels> Ads { get; set; }

    }
}