//DIRS21 DATA-MODEL

namespace DataModel
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }    // e.g. "Single", "Double"
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }
}