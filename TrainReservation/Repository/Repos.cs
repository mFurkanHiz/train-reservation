using System.Collections.Generic;
using TrainReservation.Data;
using TrainReservation.Entities;
using Microsoft.EntityFrameworkCore;

namespace TrainReservation.Repository
{
    public class Repos<T> where T : class 
    {
        
        ApplicationDbContext _context;
        public Repos(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Wagon> GetAll()
        {
            return _context.Wagons.ToList(); 
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
