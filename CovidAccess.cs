﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;

namespace F_W2021_991593731
{
    class CovidAccess
    {
        public void getAllLocations(String country)  // Option 1
        {
            string cs = GetConnectionString();
            string query = "Select Location.LocationId, Location.location, Continent.continent, Covid.population from Covid INNER JOIN Continent ON Covid.continentId=Continent.continentId INNER Join Location ON Covid.locationId=Location.LocationId WHERE Covid.LocationId = (Select LocationId FROM Location WHERE location LIKE '" + @country + "')";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();
            
            adapter.Fill(ds, "Location");
            DataTable tblLocations = ds.Tables["Location"];
            Console.WriteLine($"{"Location ID",10} {"Location",-10} {"Continent",23} {"Population",10}");
            foreach (DataRow row in tblLocations.Rows)
            {
                Console.WriteLine($"{row["LocationId"],11} {row["Location"],-10} {row["continent"],23} {row["population"],10}");
            }
            Console.WriteLine("\n");
        }

        public void addNewCase()  // Option 2
        {
            int continent = getContinentById();
            string q1 = "Select DISTINCT location.LocationId, location.location from Covid INNER JOIN location ON Covid.LocationId=location.LocationId WHERE continentId = "+ @continent;
            int country = getCountryById(q1);
            Console.WriteLine("How many new cases: ");
            float newCases = float.Parse(Console.ReadLine());

            string cs = GetConnectionString();
            SqlConnection conn = new SqlConnection(cs);
            DateTime todaysDate = DateTime.Today;
            string s = todaysDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            ////////////////////
            String sqlText = "SELECT MAX(total_cases) FROM Covid WHERE LocationId=" + country;

            
            SqlConnection conn1 = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(sqlText, conn1);
            conn1.Open();

            float total_cases = Convert.ToInt32(cmd.ExecuteScalar());
            Console.WriteLine(total_cases);
            
            String sqlText2 = "SELECT MAX(population) FROM Covid WHERE LocationId=" + country;
            SqlConnection conn2 = new SqlConnection(cs);
            conn2.Open();
            SqlCommand cmd2 = new SqlCommand(sqlText2, conn2);
            int population = Convert.ToInt32(cmd2.ExecuteScalar());
            Console.WriteLine(population);

            conn1.Close();
            float new_tot = total_cases + newCases;
            /////////////////////
            string query = "INSERT INTO Covid(continentId, locationId, new_cases, date, total_cases, population) VALUES(" + @continent +", " + @country + ", " + @newCases + ", getdate(), " + @new_tot + ", " + @population + ")";
            //SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter();
            conn.Open();
            adapter.InsertCommand = new SqlCommand(query, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
            conn1.Close();
            conn2.Close();
            Console.WriteLine("New case successfully added!");
        }

        public void addNewLocation(int continent, String location)  // Option 3
        {
            string cs = GetConnectionString();
            string query = "INSERT INTO location(location) VALUES('" + @location + "')";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter();
            conn.Open();
            adapter.InsertCommand = new SqlCommand(query, conn);
            adapter.InsertCommand.ExecuteNonQuery();
            Console.WriteLine("New location successfully inserted!");
        }
        public void updateLocation()  // Option 4
        {
            string cs = GetConnectionString();
            string q1 = "Select locationId, location FROM location";
            String country = getCountry(q1);
            string query = "Select Covid.locationId, location.location AS 'Location Name', Covid.population, Covid.total_cases, CONVERT(VARCHAR(10), covid.date, 105) AS 'date' FROM Covid INNER JOIN location ON Covid.locationId=location.LocationId WHERE Covid.LocationId = (Select LocationId FROM Location WHERE location LIKE LOWER('" + @country + "'))";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Countries");
            DataTable tblCountries = ds.Tables["Countries"];
            Console.WriteLine($"{"Location Id",10} {"Location Name",-13} {"Population",-10} {"Total Cases",-10} {"Date",-10}");
            foreach (DataRow row in tblCountries.Rows)
            {
                Console.WriteLine($"{row["locationId"],-11} {row["Location Name"],-13} {row["population"],-10} {row["total_cases"],-11} {row["Date"],-10}");
            }
            Console.WriteLine("\n");
        }
        public void getAllCovidCasesByLocation()  // Option 5
        {
            string cs = GetConnectionString();
            String continent = getContinent();
            string q1 = "Select DISTINCT location.LocationId, location.location from Covid INNER JOIN location ON Covid.LocationId=location.LocationId WHERE continentId = (Select continentId FROM Continent WHERE continent LIKE LOWER('" + @continent + "'))";
            Console.WriteLine("Enter a country name: ");
            String country = getCountry(q1);
            string query = "Select Covid.CovidId, Continent.continentId, Continent.continent AS 'Continent Name', Location.location AS 'Location Name', Covid.new_cases, Covid.total_cases, Covid.population, CONVERT(VARCHAR(10), covid.date, 105) AS 'Date' from Covid JOIN Continent ON Covid.continentId=Continent.continentId Join Location ON Covid.locationId=Location.LocationId where Covid.LocationId = (Select LocationId FROM Location WHERE location LIKE LOWER('" + @country +"'))";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Continents");
            DataTable tblContinents = ds.Tables["Continents"];
            Console.WriteLine($"{"CovidId",10} {"Continent Name",-13} {"Location Name",23} {"New Cases",-10} {"Total Cases",-10} {"Population",-10} {"Date",-10}");
            foreach (DataRow row in tblContinents.Rows)
            {
                Console.WriteLine($"{row["covidId"],10} {row["Continent Name"],-16} {row["Location Name"],21} {row["new_cases"],-10} {row["total_cases"],-11} {row["population"],-10} {row["Date"],-10}");
            }
            Console.WriteLine("\n");
        }

        public String getContinent()
        {
            string cs = GetConnectionString();
            string query = "Select continentId, continent FROM Continent";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Continents");
            DataTable tblContinents = ds.Tables["Continents"];
            Console.WriteLine($"{"continentId",10} {"Continent",-12}");
            foreach (DataRow row in tblContinents.Rows)
            {
                Console.WriteLine($"{row["continentId"],11} {row["Continent"],-10}");
            }
            while (true) { 
                Console.WriteLine("Enter a continent Name:");
                String continent = Console.ReadLine();
                if (continent == "")
                {
                    Console.WriteLine("You cannot leave country name blank! Please try again:");
                }
                else { return continent; }
            }
        }
        public String getCountry(String query)
        {
            string cs = GetConnectionString();
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Location");
            DataTable tblContinents = ds.Tables["Location"];
            Console.WriteLine($"{"LocationId",10} {"location",-12}");
            foreach (DataRow row in tblContinents.Rows)
            {
                Console.WriteLine($"{row["LocationId"],10} {row["location"],-10}");
            }
            while (true) { 
                Console.WriteLine("Select a country:");
                String country = Console.ReadLine();
                if(country == "")
                {
                    Console.WriteLine("You cannot leave country name blank! Please try again:");
                }
                else { return country; }
                
            }
        }
        public int getCountryById(String query)
        {
            string cs = GetConnectionString();
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Location");
            DataTable tblContinents = ds.Tables["Location"];
            Console.WriteLine($"{"LocationId",10} {"location",-12}");
            foreach (DataRow row in tblContinents.Rows)
            {
                Console.WriteLine($"{row["LocationId"],11} {row["location"],-10}");
            }
            while (true) { 
            try { 
            Console.WriteLine("Select a country ID:");
            int country = int.Parse(Console.ReadLine());
            return country;
            }catch (Exception)
            {
                    Console.WriteLine("Please select a country by its ID");
            }
            }
        }
        public int getContinentById()
        {
            string cs = GetConnectionString();
            string query = "Select continentId, continent FROM Continent";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Continents");
            DataTable tblContinents = ds.Tables["Continents"];
            Console.WriteLine($"{"continentId",10} {"Continent",-12}");
            foreach (DataRow row in tblContinents.Rows)
            {
                Console.WriteLine($"{row["continentId"],11} {row["Continent"],-10}");
            }
            while (true)
            {
                try
                {
                    Console.WriteLine("Select a continent ID:");
                    int continent = int.Parse(Console.ReadLine());
                    return continent;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please select a continent by its ID");
                }
            }
        }
        public void updateTotalCases(string date, float newCases)
        {
            string cs = GetConnectionString();
            string query = "UPDATE Covid SET total_cases=total_cases+"+ newCases + " WHERE Covid.date LIKE " + @date;
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter();
            conn.Open();
            adapter.InsertCommand = new SqlCommand(query, conn);
            adapter.InsertCommand.ExecuteNonQuery();
        }
        public string GetConnectionString()
        {
            string connectionStringName = "CovidLocal";
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("config.json");
            IConfiguration config = builder.Build();

            return config["ConnectionStrings:" + connectionStringName];
        }
    }
}
