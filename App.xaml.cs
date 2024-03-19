using MauiAppTravel.Classes;

namespace MauiAppTravel
{
    public partial class App : Application
    {
        public static List<Flights>? Flights { get; set; }
        public static List<Airports>? Airports { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}