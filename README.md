# DIRS21 Dynamic Mapping System

## About

This project implements a dynamic mapping system using .NET 10, designed to handle 
bidirectional data conversion between DIRS21 internal C# data models and external 
partner-specific models.

The system is built around a **Registry Pattern**, where a central `MapHandler` delegates 
mapping responsibilities to partner-specific mappers registered in a `MapperRegistry`. 
The architecture is fully extensible — adding a new partner requires only implementing 
a new mapper class and registering it, with zero changes to the core engine.

### Key Features

- Bidirectional mapping between DIRS21 and Google data models
- Extensible architecture — add new partners with minimal effort
- Robust error handling with custom `MappingException`
- Input validation at every layer
- 7 unit tests covering all mappers and edge cases
- Clean separation of concerns across multiple projects

---

## Project Structure

```
DIRS21.Mapping/
├── DIRS21.Mapping.Models/          # Data classes for DIRS21 and partners
│   ├── DataModel/                  # Internal DIRS21 models
│   │   ├── Reservation.cs
│   │   └── Room.cs
│   └── Google/                     # Google partner models
│       ├── Reservation.cs
│       └── Room.cs
├── DIRS21.Mapping.Core/            # Mapping engine
│   ├── IMapper.cs                  # Interface all mappers implement
│   ├── MapHandler.cs               # Main public API
│   ├── MapperRegistry.cs           # Stores and looks up mappers
│   ├── MappingException.cs         # Custom error class
│   └── Mappers/                    # Concrete mapper implementations
│       ├── ReservationToGoogleMapper.cs
│       ├── GoogleToReservationMapper.cs
│       ├── RoomToGoogleMapper.cs
│       └── GoogleToRoomMapper.cs
├── DIRS21.Mapping.Demo/            # Console app demonstrating usage
│   └── Program.cs
└── DIRS21.Mapping.Tests/           # Unit tests (xUnit)
    └── MapHandlerTests.cs
```

---

## Architecture

The system uses a **Registry Pattern**:

```
MapHandler
    └── delegates to → MapperRegistry
                            └── looks up → IMapper
                                              └── converts the data
```

- **MapHandler** — the single entry point. Accepts data, sourceType, targetType.
- **MapperRegistry** — stores all mappers in a dictionary keyed by (sourceType, targetType).
- **IMapper** — interface every mapper implements.
- **MappingException** — thrown when no mapper is found or input type is wrong.

---

## Usage

```csharp
// 1. Setup
var registry = new MapperRegistry();
registry.Register(new ReservationToGoogleMapper());
registry.Register(new GoogleToReservationMapper());

var mapHandler = new MapHandler(registry);

// 2. Map DIRS21 → Google
var googleReservation = (Google.Reservation)mapHandler.Map(
    dirs21Reservation,
    "Model.Reservation",
    "Google.Reservation"
);

// 3. Map Google → DIRS21
var dirs21Reservation = (DataModel.Reservation)mapHandler.Map(
    googleReservation,
    "Google.Reservation",
    "Model.Reservation"
);
```

---

## How to Add a New Partner (e.g. Booking.com)

**Step 1:** Add the partner models in `DIRS21.Mapping.Models`:

```
DIRS21.Mapping.Models/
└── Booking/
    └── Reservation.cs
```

**Step 2:** Create a mapper in `DIRS21.Mapping.Core/Mappers/`:

```csharp
public class ReservationToBookingMapper : IMapper
{
    public string SourceType => "Model.Reservation";
    public string TargetType => "Booking.Reservation";

    public object Map(object data)
    {
        if (data is not DataModel.Reservation source)
            throw new MappingException($"Invalid source type");

        return new Booking.Reservation
        {
            // map fields here
        };
    }
}
```

**Step 3:** Register it in your setup:

```csharp
registry.Register(new ReservationToBookingMapper());
```

That's it. No changes to `MapHandler` or `MapperRegistry` needed.

---

## Running the Demo

```bash
cd DIRS21.Mapping.Demo
dotnet run
```

## Running the Tests

```bash
cd DIRS21.Mapping.Tests
dotnet test
```

> [!IMPORTANT]
> Run the tests and demo using **Windows CMD or PowerShell terminal only**.
> Do not use the Visual Studio Test Explorer as it may throw a
> `FileNotFoundException` due to a known compatibility issue between
> Visual Studio's built-in test runner and .NET 10.
> Running via terminal works perfectly and shows all 7 tests passing.

---

## The Big Picture

```
You call MapHandler.Map(data, "Model.Reservation", "Google.Reservation")
                    ↓
         MapHandler validates inputs
                    ↓
         MapperRegistry finds the right mapper
                    ↓
         ReservationToGoogleMapper.Map() runs
                    ↓
         Returns Google.Reservation object
```

---

## Assumptions

- Date fields use `yyyy-MM-dd` format for partner models.
- Numeric fields (RoomNumber, Price) are stored as strings in partner models.
- Room availability is stored as `"available"` or `"unavailable"` in Google model.

---

## Limitations

- Currently only Google is supported as a partner.
- No support for partial/nested object mapping.
- Adding a new partner requires manual mapper registration in setup code.