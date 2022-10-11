using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.Initialize();
            Data.NextRace();

            // Renderer.DrawTrack(Data.CurrentRace.Track);
            Data.CurrentRace.DriversChanged += DriversChangedEventHandler;
            // Data.CurrentRace.RaceEnded += RaceEndedEventHandler;
        }

        public void DriversChangedEventHandler(object sender, DriversChangedEventArgs e)
        {
            TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    TrackImage.Source = null;
                    TrackImage.Source = Renderer.DrawTrack(Data.CurrentRace.Track);
                }));
        }

        private void RaceEndedEventHandler(object sender, EventArgs eventArgs)
        {
            Data.NextRace();
            Data.CurrentRace.DriversChanged += DriversChangedEventHandler;
            Data.CurrentRace.RaceEnded += RaceEndedEventHandler;
        }
    }
}