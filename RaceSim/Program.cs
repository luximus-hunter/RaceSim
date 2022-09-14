using Controller;
using RaceSim;

Console.WriteLine("Maximize the console. Press Enter when done.");
Console.ReadLine();
Console.Clear();

Data.Initialize();
Data.NextRace();

// Aftekenen 2.6 
// Console.WriteLine(Data.CurrentRace.Track.Name);

// Aftekenen 4...
Visualisation.Initialize(Data.CurrentRace);
Visualisation.DrawTrack();
// Visualisation.PlaceParticipants();

for (;;)
{
    Thread.Sleep(100);
}