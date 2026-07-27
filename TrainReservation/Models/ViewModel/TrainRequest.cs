namespace TrainReservation.Models.ViewModel
{
    public class TrainRequest
    {
        public string Name { get; set; }
        public List<WagonRequest> Wagons { get; set; }
    }
}
