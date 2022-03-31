using System;
using System.Collections.Generic;
using FreightSchedule;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace FlightFreight
{
    class Program
    {
        static void Main(string[] args)
        {
            List<schedule> sch = new List<schedule>{
            new schedule{ day=1, flightno=1, origin="YUL",destination="YYZ" },
            new schedule{ day=1, flightno=2, origin="YUL",destination="YYC" },
            new schedule{ day=1, flightno=3, origin="YUL",destination="YVR" },
            new schedule{ day=2, flightno=4, origin="YUL",destination="YYZ" },
            new schedule{ day=2, flightno=5, origin="YUL",destination="YYC" },
            new schedule{ day=2, flightno=6, origin="YUL",destination="YVR" },
            };

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(sch);
            }
        }
        private static bool MainMenu(List<schedule> sch)
        {
            Console.Clear();
            Console.WriteLine("Hello User, welcome to SkyTrek!");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Display Current Flight Schedule");
            Console.WriteLine("2) Clear and Enter New Flight Schedule");
            Console.WriteLine("3) Add new flights to existing schedule");
            Console.WriteLine("4) Generate Flight Itineraries ");
            Console.WriteLine("5) Exit");
            Console.Write("\r\nSelect an option: ");
 
            switch (Console.ReadLine())
            {
                case "1":
                DisplayCurrent(sch);
                    return true;
                case "2":
                ClearFlights(sch);
                    return true;
                case "3":
                bool showMenu = true;
                while (showMenu)
                {
                    showMenu = enternewflight(sch);
                }                       
                    return true;
                case "4":
                    ReadFile(sch);
                    return true;
                case "5":
                    return false;
                default:
                    return true;
            }
        }

        private static void DisplayCurrent(List<schedule> sch)
        {
            Console.WriteLine("Current Flight Schedule");
            Console.WriteLine("________________________________________________________________");
            foreach (var scd in sch)
            {
                Console.WriteLine("Flight: {0}, Departure: {1}, Arrival: {2}, Day: {3}",scd.flightno,scd.origin,scd.destination,scd.day);
            }
            Console.WriteLine("________________________________________________________________");
            Console.WriteLine("Please press any key to continue.");
            Console.ReadLine();
        }

        private static void ClearFlights(List<schedule> sch)
        {
            sch.Clear();
            Console.WriteLine("All scheduled flights cleared");
            Console.WriteLine("________________________________________________________________");
           
            Console.WriteLine("Please enter new flight details");
            Console.WriteLine("________________________________________________________________");
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = enternewflight(sch);
            } 
        }

        private static bool enternewflight(List<schedule> sch)
        {
            int day;
            int flightno;
            string origin;
            string destination;

            Console.WriteLine("Day:");
            while(!int.TryParse(Console.ReadLine(), out day))
            {
                Console.Clear();
                Console.WriteLine("Please enter new flight details");
                Console.WriteLine("________________________________________________________________");
                Console.WriteLine("You entered an invalid number");
                Console.Write("Day: ");

            }

            Console.WriteLine("FlightNo:");
            while(!int.TryParse(Console.ReadLine(), out flightno))
            {
                Console.Clear();
                Console.WriteLine("You entered an invalid number");
                Console.Write("FlightNo: ");
            }

            Console.WriteLine("Origin:");
            origin= Console.ReadLine();

            Console.WriteLine("Destination:");
            destination= Console.ReadLine();
            
            schedule newsch= new schedule
            {
                day=day,
                flightno=flightno,
                origin=origin,
                destination=destination
            };

            sch.Add(newsch);
            
            char response = YorN("Do you want to add another flight schedule? (y/n)");

            if (response == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static char YorN(string prompt)
            {
                Console.Write(prompt + " ");

                while (true)
                {
                    char input = char.ToUpper(Console.ReadKey().KeyChar);

                    if (input == 'Y' || input == 'N')
                    {
                        Console.WriteLine();
                        return input;
                    }

                    Console.Write("\b \b");
                }
            }

        public static void ReadFile(List<schedule> sch)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+"\\data\\coding-assigment-orders.json";
            using (StreamReader file = File.OpenText(path))
            {
                string json = file.ReadToEnd();

                var jObj = JObject.Parse(json);
                var props = jObj.Properties().Select(x => ((JProperty)x).Name).ToList();
           
                string destination;
                 
                foreach (var prop in props)
                {
                    destination=jObj[prop]["destination"].ToString();
                    schedule flight = sch.Where(x => x.destination == destination &&  x.capacity>0).FirstOrDefault();
                    
                    if(flight==null)
                    {
                        Console.WriteLine("order: {0}, flightnumber: not scheduled", prop);
                    }
                    else
                    {
                        Console.WriteLine("order: {0}, flightnumber: {1}, departure:{2}, arrival: {3}, day: {4}, capacity {5}", prop, flight.flightno, flight.origin, flight.destination, flight.day, flight.capacity);
                        flight.capacity--;
                    }
                    
                }
                Console.WriteLine("Flight Itineraries have been generated. Please press any key to go back to the menu.");
                Console.WriteLine("________________________________________________________________");
                Console.ReadLine();
            }
            
        }
    }
    
    
}
