using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppTravel.Classes
{
    [Serializable]
    public class Reservation
    {
        // Public properties to hold reservation and flight details.
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DayOfWeek { get; set; }
        public string DepartureTime { get; set; }
        public decimal Price { get; set; }
        public string TravelerName { get; set; }
        public string Citizenship { get; set; }
        public string ReservationCode { get; set; }
        public bool IsActive { get; set; }

        // Constructor for creating a new reservation 
        public Reservation(string flightCode, string airline, string day, string time, decimal cost, string name, string citizenship, string reservationCode)
        {
            FlightNumber = flightCode ?? throw new ArgumentException("Flight code cannot be null.");
            Airline = airline ?? throw new ArgumentException("Airline cannot be null.");
            DayOfWeek = day ?? throw new ArgumentException("Day cannot be null.");
            DepartureTime = time ?? throw new ArgumentException("Time cannot be null.");
            Price = cost;
            TravelerName = name ?? throw new ArgumentException("Traveler name cannot be empty.");
            Citizenship = citizenship ?? throw new ArgumentException("Citizenship cannot be empty.");
            ReservationCode = reservationCode ?? throw new ArgumentException("Reservation code cannot be null.");
        }

        // Constructor for creating a new reservation based on an existing Flights object 
        public Reservation(Flights flight, string name, string citizenship, string reservationCode)
        {
            FlightNumber = flight.FlightNumber;
            Airline = flight.Airline;
            DayOfWeek = flight.DayOfWeek;
            DepartureTime = flight.DepartureTime;
            Price = flight.Price;
            TravelerName = name;
            Citizenship = citizenship;
            ReservationCode = reservationCode;
        }

        // constructor marked with the JsonConstructor attribute for deserialization 
        [JsonConstructor]
        public Reservation(string flightNumber, string airline, string dayOfWeek, string departureTime, decimal price, string travelerName, string citizenship, string reservationCode, bool isActive)
        {
            FlightNumber = flightNumber;
            Airline = airline;
            DayOfWeek = dayOfWeek;
            DepartureTime = departureTime;
            Price = price;
            TravelerName = travelerName;
            Citizenship = citizenship;
            ReservationCode = reservationCode;
            IsActive = isActive;
        }
    }
}
