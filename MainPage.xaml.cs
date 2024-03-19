using MauiAppTravel.Classes;
using System.Globalization;
using System.Text.Json;


namespace MauiAppTravel
{
    public partial class MainPage : ContentPage
    {
        // ReservationManager handles the logic related to reservations.
        private readonly ReservationManager _reservationManager = new ReservationManager();
        private static readonly string flightsFilePath = "C:\\Users\\thedu\\source\\repos\\MauiAppTravel\\MauiAppTravel\\Resources\\Files\\flights.csv";
        private static readonly string airportsFilePath = "C:\\Users\\thedu\\source\\repos\\MauiAppTravel\\MauiAppTravel\\Resources\\Files\\airports.csv";
        public bool _isUpdatingReservation = false;
        // bool to indicate if the current action is updating an existing reservation

        public MainPage()
        {
            InitializeComponent();
            PopulateAirportLists();
            PopulateDayPicker();

        }
        // PopulateAirportLists loads airport data to populate the departure and arrival pickers 

        public void PopulateAirportLists()
        {
            var flightsList = Flights.LoadFlights(flightsFilePath);

            // Generate lists for departure and arrival airports
            var departureAirports = flightsList.Select(flight => flight.DepartureAirport).Distinct().ToList();
            var arrivalAirports = flightsList.Select(flight => flight.ArrivalAirport).Distinct().ToList();

            // Update Pickers  
            fromPicker.ItemsSource = departureAirports;
            toPicker.ItemsSource = arrivalAirports;
        }
        // Holds the reservation currently being updated
        private Reservation _selectedReservationForUpdate;

        private void PopulateDayPicker()
        {
            var daysOfWeek = new List<string>
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday"
            };

            dayPicker.ItemsSource = daysOfWeek;
        }
        //// FindFlightsClicked searches for flights based on the selected criteria
        private void FindFlightsClicked(object sender, EventArgs e)
        {
            var fromAirport = fromPicker.SelectedItem?.ToString().Split('-')[0].Trim();
            var toAirport = toPicker.SelectedItem?.ToString().Split('-')[0].Trim();
            var day = dayPicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(fromAirport) || string.IsNullOrEmpty(toAirport) || string.IsNullOrEmpty(day))
            {
                DisplayAlert("Error", "Please select From, To, and Day fields.", "OK");
                return;
            }

            var allFlights = Flights.LoadFlights(flightsFilePath);
            var matchingFlights = allFlights.Where(flight =>
                flight.DepartureAirport.Equals(fromAirport, StringComparison.OrdinalIgnoreCase) &&
                flight.ArrivalAirport.Equals(toAirport, StringComparison.OrdinalIgnoreCase) &&
                flight.DayOfWeek.Equals(day, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!matchingFlights.Any())
            {
                noFlightsFoundLabel.IsVisible = true; // Show the "No flights found" message
                
            }
            else
            {
                noFlightsFoundLabel.IsVisible = false;
                flightsListView.ItemsSource = matchingFlights;
            }
        }
        // OnFlightSelected handles the event when a flight is selected from the list, populating the reservation fields with flight data
        private void OnFlightSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedFlight = e.SelectedItem as Flights;
            if (selectedFlight == null)
            {
                DisplayAlert("Selection Error", "No flight selected or incorrect object type.", "OK");
                return;
            }

            reserveFlightCodeEntry.Text = selectedFlight.FlightNumber;
            reserveAirlineEntry.Text = selectedFlight.Airline;
            reserveDayEntry.Text = selectedFlight.DayOfWeek;
            reserveTimeEntry.Text = selectedFlight.DepartureTime;
            reserveCostEntry.Text = selectedFlight.Price.ToString(CultureInfo.InvariantCulture);
            _isUpdatingReservation = false;
        }
        // ReserveFlightClicked handles the click event of the Reserve button
        private void ReserveFlightClicked(object sender, EventArgs e)
        {
            if (_isUpdatingReservation)
            {
                // Update logic for an existing reservation
                if (_selectedReservationForUpdate != null)
                {
                    _selectedReservationForUpdate.TravelerName = nameEntry.Text;
                    _selectedReservationForUpdate.Citizenship = citizenshipEntry.Text;
                    // Update other fields as necessary

                    // Save changes to JSON
                    _reservationManager.SaveReservationsToJson();
                    DisplayAlert("Success", "Reservation updated successfully.", "OK");
                }
            }
            else { 
                var selectedFlight = flightsListView.SelectedItem as Flights;
                if (selectedFlight == null)
                {
                    DisplayAlert("Error", "No flight selected.", "OK");
                    return;
                }

                string name = nameEntry.Text;
                string citizenship = citizenshipEntry.Text;

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(citizenship))
                    {
                    DisplayAlert("Error", "Please enter your name and citizenship.", "OK");
                    return;
                }

                var reservation = _reservationManager.MakeReservation(selectedFlight, name, citizenship);
                reservationCodeLabel.Text = $"Reservation Code: {reservation.ReservationCode}";


            }
        }
        // FindReservationsClicked handles the event for the Find Reservations button, searching for reservations
        private void FindReservationsClicked(object sender, EventArgs e)
        {
            var code = searchCodeEntry.Text;
            var airline = searchAirlineEntry.Text;
            var name = searchNameEntry.Text;

            var matchingReservations = _reservationManager.SearchReservations(code, airline, name);

            // Binding the search results to the reservationsListView
            reservationsListView.ItemsSource = matchingReservations;
        }
        // OnReservationSelected handles the event when a reservation is selected from the list, populating the reservation section 
        private void OnReservationSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedReservation = e.SelectedItem as Reservation;
            if (selectedReservation == null)
            {
                DisplayAlert("Selection Error", "No reservation selected or incorrect object type.", "OK");
                return;
            }

            //populate the reservation section with the selected reservation details
            PopulateReservationSection(selectedReservation);
            _selectedReservationForUpdate = selectedReservation;
            _isUpdatingReservation = true;
        }
        // PopulateReservationSection fills in the reservation details in the UI 
        private void PopulateReservationSection(Reservation reservation)
        {
            reserveFlightCodeEntry.Text = reservation.FlightNumber;
            reserveAirlineEntry.Text = reservation.Airline;
            reserveDayEntry.Text = reservation.DayOfWeek;
            reserveTimeEntry.Text = reservation.DepartureTime;
            reserveCostEntry.Text = reservation.Price.ToString(CultureInfo.InvariantCulture);
            nameEntry.Text = reservation.TravelerName;
            citizenshipEntry.Text = reservation.Citizenship;
        }
    }
}
