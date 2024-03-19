using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTravel.Classes
{
    
    public class Airports
    {
        // Constructor to initialize a new instance of Airports with an airport code and name
        public Airports(string airportcode, string airportname)
        {
            AirportCode = airportcode;
            AirportName = airportname;
        }

        // Gets the airport code
        public string AirportCode { get; }

        // Gets the airport name
        public string AirportName { get; }

        // Overrides ToString method
        public override string ToString()
        {
            // Combines airport code and name for display
            return $"{AirportCode} - {AirportName}";
        }

        //  method to load airport data from a file
        public static List<Airports> LoadAirports(string filePath)
        {
            var airportsList = new List<Airports>();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    var airportCode = parts[0].Trim();
                    var airportName = parts[1].Trim();
                    airportsList.Add(new Airports(airportCode, airportName));
                }
            }

            // Return the populated list of airports
            return airportsList;
        }
    }
}
