//DIRS21 DATA-MODEL

namespace DataModel
{
    public class Reservation
    {
        public string Id { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int RoomNumber { get; set; }
        public decimal TotalPrice { get; set; }
    }
}