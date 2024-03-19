using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTravel.Classes
{
    public class Flights
    {
        // Constructor initializes a new instance of Flights with specified properties
        public Flights(string flightnumber, string airline, string departureairport, string arrivalairport, string dayofweek, string departuretime, int seatsavailable, decimal price)
        {
            FlightNumber = flightnumber;
            Airline = airline;
            DepartureAirport = departureairport;
            ArrivalAirport = arrivalairport;
            DayOfWeek = dayofweek;
            DepartureTime = departuretime;
            SeatsAvailable = seatsavailable;
            Price = price;
        }

        // Properties to get flight details.
        public string FlightNumber { get; }
        public string Airline { get; }
        public string DepartureAirport { get; }
        public string ArrivalAirport { get; }
        public string DayOfWeek { get; }
        public string DepartureTime { get; }
        public int SeatsAvailable { get; set; }
        public decimal Price { get; }

        // Returns a string representation of a flight
        public override string ToString()
        {
            return $"{FlightNumber} - {Airline} from {DepartureAirport} to {ArrivalAirport} on {DayOfWeek} at {DepartureTime}, Seats Available: {SeatsAvailable}, Price: {Price}";
        }

        // Decrements the seat availability count
        public void BookSeat()
        {
            if (SeatsAvailable <= 0)
            {
                throw new InvalidOperationException("Flight is fully booked.");
            }
            SeatsAvailable--;
        }

        // Loads flight data and returns a list of Flights objects
        public static List<Flights> LoadFlights(string filePath)
        {
            var flightsList = new List<Flights>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                var flight = CreateFlightFromParts(parts);
                if (flight != null)
                {
                    flightsList.Add(flight);
                }
            }

            return flightsList;
        }

        //method to create a Flights object from a split line of flight data
        private static Flights CreateFlightFromParts(string[] parts)
        {
            if (parts.Length != 8)
            {
                return null;
            }

            string flightnumber = parts[0];
            string airline = parts[1];
            string departureairport = parts[2];
            string arrivalairport = parts[3];
            string dayofweek = parts[4];
            string departuretime = parts[5];
            if (!int.TryParse(parts[6], out int seatsavailable) || !decimal.TryParse(parts[7], out decimal price))
            {
                return null;
            }
            return new Flights(flightnumber, airline, departureairport, arrivalairport, dayofweek, departuretime, seatsavailable, price);
        }

        //Increments the seat availability count for a flight
        public void IncrementSeatAvailability()
        {
            SeatsAvailable++;
        }

        //Decrements the seat availability count
        public void DecrementSeatAvailability()
        {
            if (SeatsAvailable <= 0) throw new InvalidOperationException("No seats available.");
            SeatsAvailable--;
        }

        // Finds and returns a flight by its flight number 
        public static Flights FindFlightByFlightNumber(List<Flights> flights, string flightNumber)
        {
            return flights.FirstOrDefault(flight => flight.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));
        }
    }
}
