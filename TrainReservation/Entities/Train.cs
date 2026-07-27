namespace TrainReservation.Entities
{
    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Wagon> Wagons { get; set; }
    }
}
