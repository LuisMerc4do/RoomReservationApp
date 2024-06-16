namespace RoomReservationApp.Models
{
    public class RoomReservationModel
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string ReservedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
    }
}
