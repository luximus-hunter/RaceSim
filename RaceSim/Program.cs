using Controller;
using RaceSim;

Console.WriteLine("Maximize the console. Press Enter when done.");
Console.ReadLine();
Console.Clear();

Data.Initialize();
Data.NextRace();
// Data.NextRace();

Data.CurrentRace.DriversChanged += Visualisation.DriversChangedEventHandler;

// Aftekenen 2.6 
// Console.WriteLine(Data.CurrentRace.Track.Name);

// Aftekenen 4...
Visualisation.Initialize(Data.CurrentRace);
Visualisation.PlaceParticipants();
Visualisation.DrawTrack();

Data.CurrentRace.Start();

for (;;)
{
    // Visualisation.DrawTrack();
    // Thread.Sleep(500);
}