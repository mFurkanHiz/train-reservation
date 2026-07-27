# Train Reservation

A seat reservation exercise: given a train, a number of passengers, and one rule about how
full a wagon may get, decide whether the group can be seated and where.

I built this in 2023 as a case study. The web part is thin on purpose. The whole exercise
lives in one method, and the interesting work is getting the rules right rather than making
it look good.

## The rules

A train has wagons, and every wagon has a capacity and a number of seats already taken.

1. **No wagon may go past 70% of its capacity.** A wagon with 100 seats stops accepting
   people at 70, even though 30 seats are physically free. That is the rule the exercise is
   built around.
2. **The group either fits or it does not.** If the last passenger cannot be seated, nobody
   is seated. No half reservations, and nothing is written to the database.
3. **Splitting is the caller's choice.** A group of 40 might fit as 25 in one wagon and 15 in
   another. If the group said no to splitting, only a wagon that can take all 40 at once
   counts, and the algorithm keeps looking.

## How it works

`ReservationController.Add` walks the wagons in order. For each one it works out how many
seats are still usable under the 70% rule:

```csharp
int freeSeats = (int)Math.Floor((wagon.Capacity * 0.7) - wagon.OccupiedSeats);
```

If the remaining group is smaller than that, everyone sits there and the loop is done. If it
is larger, the wagon is filled to its limit and the rest moves on, but only when splitting
was allowed. Otherwise that wagon is skipped entirely.

After the loop, anyone left over means the whole reservation fails. The placements collected
so far are thrown away and `SaveChanges` is never called, so a failed attempt leaves the
database exactly as it was.

## Running it

You need the .NET 7 SDK and SQL Server (Express is fine).

Set your server in `TrainReservation/appsettings.json`:

```json
"ConnectionStrings": {
  "TR_Database": "Server=<YOUR_SQL_SERVER>;Database=TrainReservationDataBase;Trusted_Connection=True;TrustServerCertificate=Yes"
}
```

Then:

```
dotnet ef database update --project TrainReservation
dotnet run --project TrainReservation
```

The reservation screen is at `/Reservation/List`. The database starts empty, so insert a
train and a few wagons first, otherwise there is nothing to reserve.

## Known limitations

There is no seeding and no screen for creating trains or wagons. Rows have to go in by hand
before the reservation form does anything useful.

The result page is not built. The controller works out the placements correctly but then
redirects back to the list, so you see the updated occupancy numbers rather than a "3 in
Wagon A, 2 in Wagon B" summary.

`Repos<T>` is generic in its signature only. `GetAll()` returns wagons no matter what `T`
is, which was a shortcut I would not repeat.

The reservation logic sits in a controller rather than a service, and nothing stops two
people reserving the last seats at the same time.

No authentication, no tests.

## License

MIT — see [LICENSE](LICENSE).
