//Converts Google Room to DIRS21 Room

using DIRS21.Mapping.Core;

namespace DIRS21.Mapping.Core.Mappers
{
    public class GoogleToReservationMapper : IMapper
    {
        public string SourceType => "Google.Reservation";
        public string TargetType => "Model.Reservation";

        public object Map(object data)
        {
            if (data is not Google.Reservation source)
                throw new MappingException(
                    $"Expected type '{nameof(Google.Reservation)}' but got '{data.GetType().Name}'"
                );

            // Parse strings back into proper types
            return new DataModel.Reservation
            {
                Id = source.ReservationId,
                GuestName = source.GuestFullName,
                CheckIn = DateTime.Parse(source.ArrivalDate),
                CheckOut = DateTime.Parse(source.DepartureDate),
                RoomNumber = int.Parse(source.RoomId),
                TotalPrice = decimal.Parse(source.TotalAmount)
            };
        }
    }
}