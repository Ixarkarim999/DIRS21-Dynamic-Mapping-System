//EXTERNAL SOURCE DATA-MODEL

namespace Google
{
    public class Reservation
    {
        public string ReservationId { get; set; }
        public string GuestFullName { get; set; }
        public string ArrivalDate { get; set; }      // string, not DateTime
        public string DepartureDate { get; set; }    // string, not DateTime
        public string RoomId { get; set; }           // string, not int
        public string TotalAmount { get; set; }      // string, not decimal
    }
}