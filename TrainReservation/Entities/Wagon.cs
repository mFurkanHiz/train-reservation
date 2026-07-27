namespace TrainReservation.Entities
{
    public class Wagon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int OccupiedSeats { get; set; }
        public int TrainId { get; set; }
        public Train Train { get; set; }
    }
}
