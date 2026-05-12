//Converts DIRS21 Room to Google Room

using DIRS21.Mapping.Core;
using System.Text.Json;

namespace DIRS21.Mapping.Core.Mappers
{
    public class RoomToGoogleMapper : IMapper
    {
        public string SourceType => "Model.Room";
        public string TargetType => "Google.Room";

        public object Map(object data)
        {
            if (data is not DataModel.Room source)
                throw new MappingException(
                    $"Expected type '{nameof(DataModel.Room)}' but got '{data.GetType().Name}'"
                );

            Console.WriteLine(JsonSerializer.Serialize(source));

            return new Google.Room
            {
                RoomIdentifier = source.RoomNumber.ToString(),
                Type = source.RoomType,
                NightlyRate = source.PricePerNight.ToString("F2"),
                Availability = source.IsAvailable ? "available" : "unavailable"
            };
        }
    }
}