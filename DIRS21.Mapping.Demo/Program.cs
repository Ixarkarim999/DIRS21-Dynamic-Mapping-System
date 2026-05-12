using DIRS21.Mapping.Core;
using DIRS21.Mapping.Core.Mappers;

// ─── 1. Setup ───────────────────────────────────────────────
var registry = new MapperRegistry();

// Register all mappers
registry.Register(new ReservationToGoogleMapper());
registry.Register(new GoogleToReservationMapper());
registry.Register(new RoomToGoogleMapper());
registry.Register(new GoogleToRoomMapper());

var mapHandler = new MapHandler(registry);

// ─── 2. Test: DIRS21 Reservation → Google Reservation ───────
Console.WriteLine("=== DIRS21 Reservation → Google Reservation ===\n");

var dirs21Reservation = new DataModel.Reservation
{
    Id = "R001",
    GuestName = "Izhar Karim",
    CheckIn = new DateTime(2026, 6, 1),
    CheckOut = new DateTime(2026, 6, 5),
    RoomNumber = 42,
    TotalPrice = 400m
};

var googleReservation = (Google.Reservation)mapHandler.Map(
    dirs21Reservation,
    "Model.Reservation",
    "Google.Reservation"
);

Console.WriteLine($"ReservationId : {googleReservation.ReservationId}");
Console.WriteLine($"GuestFullName : {googleReservation.GuestFullName}");
Console.WriteLine($"ArrivalDate   : {googleReservation.ArrivalDate}");
Console.WriteLine($"DepartureDate : {googleReservation.DepartureDate}");
Console.WriteLine($"RoomId        : {googleReservation.RoomId}");
Console.WriteLine($"TotalAmount   : {googleReservation.TotalAmount}");

// ─── 3. Test: Google Reservation → DIRS21 Reservation ───────
Console.WriteLine("\n=== Google Reservation → DIRS21 Reservation ===\n");

var googleRes = new Google.Reservation
{
    ReservationId = "R002",
    GuestFullName = "John Doe",
    ArrivalDate = "2026-07-10",
    DepartureDate = "2026-07-15",
    RoomId = "101",
    TotalAmount = "500.00"
};

var dirs21Res = (DataModel.Reservation)mapHandler.Map(
    googleRes,
    "Google.Reservation",
    "Model.Reservation"
);

Console.WriteLine($"Id          : {dirs21Res.Id}");
Console.WriteLine($"GuestName   : {dirs21Res.GuestName}");
Console.WriteLine($"CheckIn     : {dirs21Res.CheckIn.ToShortDateString()}");
Console.WriteLine($"CheckOut    : {dirs21Res.CheckOut.ToShortDateString()}");
Console.WriteLine($"RoomNumber  : {dirs21Res.RoomNumber}");
Console.WriteLine($"TotalPrice  : {dirs21Res.TotalPrice}");

// ─── 4. Test: DIRS21 Room → Google Room ─────────────────────
Console.WriteLine("\n=== DIRS21 Room → Google Room ===\n");

var dirs21Room = new DataModel.Room
{
    RoomNumber = 42,
    RoomType = "Double",
    PricePerNight = 80.00m,
    IsAvailable = true
};

var googleRoom = (Google.Room)mapHandler.Map(
    dirs21Room,
    "Model.Room",
    "Google.Room"
);

Console.WriteLine($"RoomIdentifier : {googleRoom.RoomIdentifier}");
Console.WriteLine($"Type           : {googleRoom.Type}");
Console.WriteLine($"NightlyRate    : {googleRoom.NightlyRate}");
Console.WriteLine($"Availability   : {googleRoom.Availability}");

// ─── 5. Test: Error Handling ─────────────────────────────────
Console.WriteLine("\n=== Error Handling Test ===\n");

try
{
    // Intentionally wrong mapping — no mapper exists for this
    mapHandler.Map(dirs21Reservation, "Model.Reservation", "Booking.Reservation");
}
catch (MappingException ex)
{
    Console.WriteLine($"Caught expected error: {ex.Message}");
}

try
{
    // Intentionally passing null
    mapHandler.Map(null, "Model.Reservation", "Google.Reservation");
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Caught null error: {ex.Message}");
}

Console.WriteLine("\n=== All tests completed ===");