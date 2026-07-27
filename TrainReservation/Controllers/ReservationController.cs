using Microsoft.AspNetCore.Mvc;
using TrainReservation.Data;
using TrainReservation.Entities;
using TrainReservation.Models.ViewModel;
using TrainReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace TrainReservation.Controllers
{
    public class ReservationController : Controller
    {

        Repos<Train> _repos;
        ApplicationDbContext _context;
        public ReservationController(Repos<Train> trainRepos, ApplicationDbContext context)
        {
            _repos = trainRepos;
            _context = context;
        }

        public IActionResult List()
        {
            return View(_repos.GetAll());
        }

        [HttpPost]
        public IActionResult Add(ReservationRequest request, int Id)
        {
            // The demo database holds a single train, so the id is optional here:
            // when the form does not send one, take the first train there is.
            var train = Id > 0
                ? _context.Trains.Include(t => t.Wagons).FirstOrDefault(t => t.Id == Id)
                : _context.Trains.Include(t => t.Wagons).FirstOrDefault();

            if (train == null)
            {
                return NotFound();
            }

            var response = new ReservationResponse { CanReserve = true, Placements = new List<ReservationDetail>() };

            int remainingPassengers = request.PassengerCount;
            bool canSplit = request.CanSplitAcrossWagons;

            foreach (var wagon in train.Wagons)
            {
                // The rule of the exercise: a wagon may never go past 70% of its seats,
                // so what is left here is 70% of capacity minus whoever already sits there.
                int freeSeats = (int)Math.Floor((wagon.Capacity * 0.7) - wagon.OccupiedSeats);

                if (freeSeats <= 0)
                {
                    continue;
                }

                if (remainingPassengers >= freeSeats)
                {
                    // The group does not fit in this wagon. Filling it only helps if the
                    // group agreed to be split, otherwise skip and look for a bigger one.
                    if (!canSplit)
                    {
                        continue;
                    }

                    response.Placements.Add(new ReservationDetail { WagonName = wagon.Name, PassengerCount = freeSeats });
                    wagon.OccupiedSeats += freeSeats;
                    remainingPassengers -= freeSeats;
                }
                else
                {
                    response.Placements.Add(new ReservationDetail { WagonName = wagon.Name, PassengerCount = remainingPassengers });
                    wagon.OccupiedSeats += remainingPassengers;
                    remainingPassengers = 0;
                    break;
                }
            }

            if (remainingPassengers > 0)
            {
                // Nobody is seated unless everybody is, so the partial placements are
                // dropped and nothing is written to the database.
                response.CanReserve = false;
                response.Placements.Clear();
            }
            else
            {
                _context.SaveChanges();
            }

            return RedirectToAction("List", response);
        }
    }
}
