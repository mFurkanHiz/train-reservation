namespace TrainReservation.Models.ViewModel
{
    public class ReservationResponse
    {
        public bool CanReserve { get; set; }
        public List<ReservationDetail> Placements { get; set; }
    }
}
