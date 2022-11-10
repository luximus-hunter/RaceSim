using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Controller;
using Model;

namespace View.Windows
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class TrackWindow
    {
        // private static readonly ScoreboardWindow ScoreboardWindow = new();
        private static readonly TrackStats TrackStatsWindow = new();
        private static readonly CompetitionStats CompetitionStatsWindow = new();

        public TrackWindow()
        {
            Data.Initialize();
            LoadTrack();
            InitializeComponent();
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
                    // Color color = Color.FromArgb(eventArgs.Color.A, eventArgs.Color.R, eventArgs.Color.G,
                    //     eventArgs.Color.B);
                    // Container.Background = new SolidColorBrush(color);
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
            LoadTrack();
            StartTrack();
        }

        private void LoadTrack()
        {
            ImageLoader.ClearImages();
            Data.NextRace();
        }

        /// <summary> Gets, sets, and starts the next track </summary>
        private void StartTrack()
        {
            Render();
            Data.CurrentRace.RaceStarted += RaceStartedEventHandler!;
            Data.CurrentRace.DriversChanged += DriversChangedEventHandler!;
            Data.CurrentRace.RaceEnded += RaceEndedEventHandler!;
            // Data.CurrentRace.UpdateScoreboard += ScoreboardWindow.UpdateScoreboardEventHandler;

            Data.CurrentRace.Start();
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemTrackStatsClick(object sender, RoutedEventArgs e)
        {
            TrackStatsWindow.Show();
        }

        private void MenuItemCompetitionStatsClick(object sender, RoutedEventArgs e)
        {
            CompetitionStatsWindow.Show();
        }

        private void MenuItemExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}