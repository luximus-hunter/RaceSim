using Controller;

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

Data.Initialize();
Data.NextRace();

Console.WriteLine($"Current Track: {Data.CurrentRace.Track.Name}");
Console.WriteLine($"Current Race: {Data.CurrentRace}");

//for (; ; )
//{
//    Thread.Sleep(100);
//}