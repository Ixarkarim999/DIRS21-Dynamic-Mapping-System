//Converts DIRS21 Reservation to Google Reservation


using DIRS21.Mapping.Core;

namespace DIRS21.Mapping.Core.Mappers
{
    public class ReservationToGoogleMapper : IMapper
    {
        public string SourceType => "Model.Reservation";
        public string TargetType => "Google.Reservation";

        public object Map(object data)
        {
            // Make sure the input is actually a DIRS21 Reservation
            if (data is not DataModel.Reservation source)
                throw new MappingException(
                    $"Expected type '{nameof(DataModel.Reservation)}' but got '{data.GetType().Name}'"
                );

            // Field by field conversion
            return new Google.Reservation
            {
                ReservationId = source.Id,
                GuestFullName = source.GuestName,
                ArrivalDate = source.CheckIn.ToString("yyyy-MM-dd"),
                DepartureDate = source.CheckOut.ToString("yyyy-MM-dd"),
                RoomId = source.RoomNumber.ToString(),
                TotalAmount = source.TotalPrice.ToString("F2")
            };
        }
    }
}