//External Data Model

namespace Google
{
	public class Room
	{
		public string RoomIdentifier { get; set; }
		public string Type { get; set; }
		public string NightlyRate { get; set; }      // string, not decimal
		public string Availability { get; set; }     // "available" or "unavailable"
	}
}