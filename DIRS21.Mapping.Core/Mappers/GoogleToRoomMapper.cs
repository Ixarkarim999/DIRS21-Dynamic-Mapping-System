//Converts Google Reservation to DIRS21 Reservation

using DIRS21.Mapping.Core;

namespace DIRS21.Mapping.Core.Mappers
{
    public class GoogleToRoomMapper : IMapper
    {
        public string SourceType => "Google.Room";
        public string TargetType => "Model.Room";

        public object Map(object data)
        {
            if (data is not Google.Room source)
                throw new MappingException(
                    $"Expected type '{nameof(Google.Room)}' but got '{data.GetType().Name}'"
                );

            return new DataModel.Room
            {
                RoomNumber = int.Parse(source.RoomIdentifier),
                RoomType = source.Type,
                PricePerNight = decimal.Parse(source.NightlyRate),
                IsAvailable = source.Availability == "available"
            };
        }
    }
}