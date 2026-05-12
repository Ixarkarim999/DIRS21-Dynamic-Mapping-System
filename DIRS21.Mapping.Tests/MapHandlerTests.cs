using DIRS21.Mapping.Core;
using DIRS21.Mapping.Core.Mappers;
using Xunit;

namespace DIRS21.Mapping.Tests
{
    public class MapHandlerTests
    {
        private readonly MapHandler _mapHandler;

        // This runs before every single test — sets up a fresh MapHandler
        public MapHandlerTests()
        {
            var registry = new MapperRegistry();
            registry.Register(new ReservationToGoogleMapper());
            registry.Register(new GoogleToReservationMapper());
            registry.Register(new RoomToGoogleMapper());
            registry.Register(new GoogleToRoomMapper());

            _mapHandler = new MapHandler(registry);
        }

        // ─── Reservation Tests ────────────────────────────────────

        [Fact]
        public void Map_Dirs21ReservationToGoogle_ShouldMapCorrectly()
        {
            // Arrange — prepare the input
            var input = new DataModel.Reservation
            {
                Id = "R001",
                GuestName = "Izhar Ahmed",
                CheckIn = new DateTime(2025, 6, 1),
                CheckOut = new DateTime(2025, 6, 5),
                RoomNumber = 42,
                TotalPrice = 320.50m
            };

            // Act — run the mapping
            var result = (Google.Reservation)_mapHandler.Map(
                input,
                "Model.Reservation",
                "Google.Reservation"
            );

            // Assert — check the output fields
            Assert.Equal("R001", result.ReservationId);
            Assert.Equal("Izhar Ahmed", result.GuestFullName);
            Assert.Equal("2025-06-01", result.ArrivalDate);
            Assert.Equal("2025-06-05", result.DepartureDate);
            Assert.Equal("42", result.RoomId);
            Assert.Equal("320.50", result.TotalAmount);
        }

        [Fact]
        public void Map_GoogleReservationToDirs21_ShouldMapCorrectly()
        {
            // Arrange
            var input = new Google.Reservation
            {
                ReservationId = "R002",
                GuestFullName = "John Doe",
                ArrivalDate = "2025-07-10",
                DepartureDate = "2025-07-15",
                RoomId = "101",
                TotalAmount = "500.00"
            };

            // Act
            var result = (DataModel.Reservation)_mapHandler.Map(
                input,
                "Google.Reservation",
                "Model.Reservation"
            );

            // Assert
            Assert.Equal("R002", result.Id);
            Assert.Equal("John Doe", result.GuestName);
            Assert.Equal(new DateTime(2025, 7, 10), result.CheckIn);
            Assert.Equal(new DateTime(2025, 7, 15), result.CheckOut);
            Assert.Equal(101, result.RoomNumber);
            Assert.Equal(500.00m, result.TotalPrice);
        }

        // ─── Room Tests ───────────────────────────────────────────

        [Fact]
        public void Map_Dirs21RoomToGoogle_ShouldMapCorrectly()
        {
            // Arrange
            var input = new DataModel.Room
            {
                RoomNumber = 42,
                RoomType = "Double",
                PricePerNight = 80.00m,
                IsAvailable = true
            };

            // Act
            var result = (Google.Room)_mapHandler.Map(
                input,
                "Model.Room",
                "Google.Room"
            );

            // Assert
            Assert.Equal("42", result.RoomIdentifier);
            Assert.Equal("Double", result.Type);
            Assert.Equal("80.00", result.NightlyRate);
            Assert.Equal("available", result.Availability);
        }

        [Fact]
        public void Map_GoogleRoomToDirs21_ShouldMapCorrectly()
        {
            // Arrange
            var input = new Google.Room
            {
                RoomIdentifier = "10",
                Type = "Single",
                NightlyRate = "60.00",
                Availability = "unavailable"
            };

            // Act
            var result = (DataModel.Room)_mapHandler.Map(
                input,
                "Google.Room",
                "Model.Room"
            );

            // Assert
            Assert.Equal(10, result.RoomNumber);
            Assert.Equal("Single", result.RoomType);
            Assert.Equal(60.00m, result.PricePerNight);
            Assert.False(result.IsAvailable);
        }

        // ─── Error Handling Tests ─────────────────────────────────

        [Fact]
        public void Map_UnknownMapping_ShouldThrowMappingException()
        {
            var input = new DataModel.Reservation { Id = "R001" };

            Assert.Throws<MappingException>(() =>
                _mapHandler.Map(input, "Model.Reservation", "Booking.Reservation")
            );
        }

        [Fact]
        public void Map_NullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _mapHandler.Map(null, "Model.Reservation", "Google.Reservation")
            );
        }

        [Fact]
        public void Map_EmptySourceType_ShouldThrowArgumentNullException()
        {
            var input = new DataModel.Reservation { Id = "R001" };

            Assert.Throws<ArgumentNullException>(() =>
                _mapHandler.Map(input, "", "Google.Reservation")
            );
        }
    }
}