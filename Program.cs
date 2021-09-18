using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace F_W2021_991593731
{
    class Program
    {
        static void Main(string[] args)
        {
            CovidAccess db = new CovidAccess();
            bool running = true;
            while (running) { 
                menu();
                try
                {
                    int userMenu = int.Parse(Console.ReadLine());
                    switch (userMenu)
                    {
                        case 1: // All Locations
                            string q1 = "Select LocationId, location from location";
                            String country = db.getCountry(q1);
                            db.getAllLocations(country);
                            break;
                        //case 2: // All Cases
                            //db.getAllCases();
                            //break;
                        case 2: // Add Cases, get continent, location, # new cases
                            db.addNewCase();                           
                            break;                            
                        case 3: // Add Location, get location id, location name
                            int continent = db.getContinentById();
                            Console.WriteLine("Enter the name of the Country: ");
                            String location = Console.ReadLine();
                            db.addNewLocation(continent, location);
                            break;
                        case 4: // Update Location, search by id
                            db.updateLocation();
                            break;
                        case 5: // Get Case By Location
                            db.getAllCovidCasesByLocation();
                            break;
                        case 6: // END
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Please enter a value between 1-6");
                            break;
                    }
                }
                catch (Exception e){
                    Console.WriteLine("You have entered an invalid menu option. Please enter a value between 1-6");
                    Console.WriteLine(e);
                }
            }
        }

        static void menu()
        {
            Console.WriteLine("1 - Get All Locations");
            Console.WriteLine("2 - Add New COVID Case");
            Console.WriteLine("3 - Add New Location");
            Console.WriteLine("4 - Update Location");
            Console.WriteLine("5 - Get Cases By Location");
            Console.WriteLine("6 - Exit");
        }
    }
}
