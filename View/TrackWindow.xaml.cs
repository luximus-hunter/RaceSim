using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Controller;
using Model;

namespace View
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class TrackWindow : Window
    {
        private const string WindowTitle = "Windesheim RaceSim";
        
        public TrackWindow()
        {
            InitializeComponent();
            Data.Initialize();
            StartTrack();
        }

        /// <summary> Render the track to the screen </summary>
        private void Render()
        {
            TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    TrackImage.Source = null;
                    TrackImage.Source = Renderer.DrawTrack(Data.CurrentRace.Track);
                }));
        }

        /// <summary> Set the background color to the color of the track </summary>
        /// <param name="sender"> Called object </param>
        /// <param name="eventArgs"> Event arguments containing the color of the track </param>
        private void RaceStartedEventHandler(object sender, RaceStartedEventArgs eventArgs)
        {
            Container.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Color color = Color.FromArgb(eventArgs.Color.A, eventArgs.Color.R, eventArgs.Color.G,
                        eventArgs.Color.B);
                    Container.Background = new SolidColorBrush(color);
                }));
            
            Window.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Window.Title = $"{WindowTitle} - {eventArgs.Title}";
                }));
        }

        /// <summary> Render the screen when the drivers change position </summary>
        /// <param name="sender"> Called object </param>
        /// <param name="eventArgs"> Event arguments </param>
        private void DriversChangedEventHandler(object sender, DriversChangedEventArgs eventArgs)
        {
            Render();
        }

        /// <summary> Start up the new race when the old one ends </summary>
        /// <param name="sender"> Called object </param>
        /// <param name="eventArgs"> Event arguments </param>
        private void RaceEndedEventHandler(object sender, EventArgs eventArgs)
        {
            StartTrack();
        }

        /// <summary> Gets, sets, and starts the next track </summary>
        private void StartTrack()
        {
            ImageLoader.ClearImages();
            Data.NextRace();
            Render();
            Data.CurrentRace.RaceStarted += RaceStartedEventHandler!;
            Data.CurrentRace.DriversChanged += DriversChangedEventHandler!;
            Data.CurrentRace.UpdateScoreboard += UpdateScoreboardEventHandler!;
            Data.CurrentRace.RaceEnded += RaceEndedEventHandler!;
            Data.CurrentRace.Start();
        }

        /// <summary> Update the scoreboard when a player finishes </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="eventArgs">Event arguments containing the scoreboard string</param>
        private void UpdateScoreboardEventHandler(object sender, UpdateScoreboardEventArgs eventArgs)
        {
            Scoreboard.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Scoreboard.Text = eventArgs.ScoreboardString;
                }));
        }
    }
}