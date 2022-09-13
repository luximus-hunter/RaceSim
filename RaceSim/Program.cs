using Controller;
using RaceSim;

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

Console.ReadLine();

Data.Initialize();
Data.NextRace();
Visualisation.Initialize(Data.CurrentRace);
Visualisation.DrawTrack();
Visualisation.PlaceParticipants();

for (; ; )
{
    Thread.Sleep(100);
}