using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TrainReservation.Entities;

namespace TrainReservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Wagon> Wagons { get; set; }

    }
}
