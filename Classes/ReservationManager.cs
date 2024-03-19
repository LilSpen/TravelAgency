using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;



namespace MauiAppTravel.Classes
{
    [Serializable]
    public class ReservationManager
    {
        //Initializing our reservation manager class
        private static int reservationCodeCounter = 1000;
        private List<Reservation> reservations = new List<Reservation>();
        private const string ReservationsFilePath = @"C:\Users\thedu\source\repos\MauiAppTravel\MauiAppTravel\Resources\Files\reservations.json";
        
        public ReservationManager() { }
        //Method to save reservations, calls SaveReservationsToJson to save file
        public Reservation MakeReservation(Flights selectedFlight, string name, string citizenship)
        {
            string reservationCode = $"L{reservationCodeCounter++}";
            var reservation = new Reservation(selectedFlight.FlightNumber, selectedFlight.Airline, selectedFlight.DayOfWeek, selectedFlight.DepartureTime, selectedFlight.Price, name, citizenship, reservationCode);
            selectedFlight.DecrementSeatAvailability(); 
            reservations.Add(reservation);

            SaveReservationsToJson(); 

            return reservation;
        }
        //Method to save our binary file
        public void SaveReservationsToJson()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(reservations, options);
            File.WriteAllText(ReservationsFilePath, jsonString);
        }
        //Method to load our binary file
        public void LoadReservationsFromJson()
        {
            if (File.Exists(ReservationsFilePath))
            {
                string jsonString = File.ReadAllText(ReservationsFilePath);
                reservations = JsonSerializer.Deserialize<List<Reservation>>(jsonString);
            }
        }
        //This method filters and then searches based on user input
        public List<Reservation> SearchReservations(string code, string airline, string name)
        {
            LoadReservationsFromJson(); // This loads the reservations into the reservations list.

           
            var filteredReservations = reservations.Where(reservation =>
                (string.IsNullOrEmpty(code) || reservation.ReservationCode.Equals(code, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(airline) || reservation.Airline.Equals(airline, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(name) || reservation.TravelerName.Equals(name, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            return filteredReservations;
        }

    }
}

      