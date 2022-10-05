using Controller;
using RaceSim;

// Console.WriteLine("Maximize the console. Press Enter when done.");
// Console.ReadLine();
// Console.Clear();

Data.Initialize();
Data.NextRace();

Data.CurrentRace.DriversChanged += Visualisation.DriversChangedEventHandler;
Data.CurrentRace.RaceEnded += delegate
{
    Data.NextRace();
    
    Visualisation.Initialize();
    Visualisation.PlaceParticipants();
    Visualisation.DrawTrack();

    Data.CurrentRace.RandomizeEquipment();
    // Data.CurrentRace.Start();
};

Visualisation.Initialize();
Visualisation.PlaceParticipants();
Visualisation.DrawTrack();

Data.CurrentRace.RandomizeEquipment();
Data.CurrentRace.Start();

for (;;)
{
    // Visualisation.DrawTrack();
    // Thread.Sleep(500);
}