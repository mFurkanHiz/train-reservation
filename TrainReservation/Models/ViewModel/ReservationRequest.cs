namespace TrainReservation.Models.ViewModel
{
    public class ReservationRequest
    {
        public TrainRequest Train { get; set; }
        public int PassengerCount { get; set; }
        public bool CanSplitAcrossWagons { get; set; }
    }
}
