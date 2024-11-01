/* 
  Homework#4

  Add your name here: Ethan Varner

  You are free to create as many classes within the Hw4.cs file or across 
  multiple files as you need. However, ensure that the Hw4.cs file is the 
  only one that contains a Main method. This method should be within a 
  class named hw4. This specific setup is crucial because your instructor 
  will use the hw4 class to execute and evaluate your work.
  */
  // BONUS POINT:

  /*
  1- => Used Lambda in line/s: 167, 215, 250
  */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Hw4
{
    public static void Main(string[] args)
    {
        // Capture the start time
        // Must be the first line of this method
        DateTime startTime = DateTime.Now; // Do not change
        // ============================
        // Do not add or change anything above, inside the 
        // Main method
        // ============================
        // TODO: your code goes here
       
        // CommonCityNames
        var states = LoadStates("states.txt");
        var commonCities = FindCommonCities("zipcodes.txt", states);
        OutputCommonCities("CommonCityNames.txt", commonCities);

        // LatLon
        var latLonValues = GetLatLon("zips.txt", "zipcodes.txt");
        OutputLatLon("LatLon.txt", latLonValues);

        // CityStates
        // var cityStatesValues = GetCityStates("cities.txt", "zipcodes.txt");
        // OutputCityStates("CityStates.txt", cityStatesValues);
        var cityStatesValues = GetCityStates("cities.txt", "zipcodes.txt");
        OutputCityStates("CityStates.txt", cityStatesValues, "cities.txt");

        // ============================
        // Do not add or change anything below, inside the 
        // Main method
        // ============================

        // Capture the end time
        DateTime endTime = DateTime.Now;  // Do not change
        
        // Calculate the elapsed time
        TimeSpan elapsedTime = endTime - startTime; // Do not change
        
        // Display the elapsed time in milliseconds
        Console.WriteLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} ms");
    } // end main

        



    //===================================================================CommonCityNames==========================================


    /*
    Method to load all entered states in states.txt
    */
    static HashSet<string> LoadStates(string filename)
    {
        var states = new HashSet<string>();
        // read through each line in file
        foreach (var line in File.ReadLines(filename))
        {
            // split data and add each entry into HashSet 
            var stateNames = line.Split(' ');
            foreach (var state in stateNames)
            {
                states.Add(state);
            }
        }
        return states;
    }


    /*
    Method to find the common cities among states listed in states.txt
    */
    static SortedSet<string> FindCommonCities(string zipcodesFile, HashSet<string> states)
    {
        // hashset to remove duplicates in result. this is a set of the cities and all the states they appear in 
        var cityStates = new Dictionary<string, HashSet<string>>();

        // load and read all lines from zipcodes.txt
        foreach (var line in File.ReadLines(zipcodesFile))
        {
            // parse through each part of the line and split it by tab
            var fields = line.Split('\t');
            var state = fields[4];

            // only check relevant states
            if (states.Contains(state))
            {
              var city = fields[3];
              // if city isn't already in the dictionary, add it with a corresponding hashset
              if (!cityStates.ContainsKey(city))
              {
                cityStates[city] = new HashSet<string>();
              }
                cityStates[city].Add(state);
            } 
            else 
            {
              // continue loop to ignore cases of irrelevant states
              continue;
            }
        }

        // sortedset to sort the results
        var commonCities = new SortedSet<string>();
        foreach (var city in cityStates.Keys)
        {
            // loop over all answers and determine which intersect 
            // ChatGPT assisted in utilization of IsSupersetOf() method
            if (cityStates[city].IsSupersetOf(states))
            {
                commonCities.Add(city);
            }
        }
        return commonCities;
    }


    /*
    Method to write output to output file of common cities
    */
    static void OutputCommonCities(string filename, SortedSet<string> commonCities)
    {
        using (var writer = new StreamWriter(filename))
        {
            foreach (var city in commonCities)
            {
                writer.WriteLine(city);
            }
        }
    }


    //===================================================================LatLon==============================================


    /*
    Method to retrieve Lat and Lon of respective zip codes in zips.txt
    */
    static Dictionary<string, string> GetLatLon(string zipsFile, string zipcodesFile)
    {
        // although for a small purpose, I did use a lambda function here to trim the lines
        // ChatGPT assisted me in making sure this was correctly formatted
        // get zip codes from zips.txt
        var zipCodes = new HashSet<string>(File.ReadLines(zipsFile).Select(line => line.Trim()));
        // make a dictionary for lat and lon
        var latLonValues = new Dictionary<string, string>();

        // for each line in zipcodes.txt, assign relevant parts of each line to variables
        foreach (var line in File.ReadLines(zipcodesFile))
        {
            // split each line and assign parts to variables
            var splitLine = line.Split('\t');
            var zip = splitLine[1];
            var lat = splitLine[6];
            var lon = splitLine[7];

            // check if already exists--to avoid duplicates
            if (zipCodes.Contains(zip) && !latLonValues.ContainsKey(zip))
            {
                // update
                latLonValues[zip] = $"{lat} {lon}";
            }
        }
        return latLonValues;
    }


    /*
    Method to output the latlon data to an output file
    */
    static void OutputLatLon(string filename, Dictionary<string, string> latLonData)
    {
        using (var writer = new StreamWriter(filename))
        {
            foreach (var zip in latLonData.Keys)
            {
                writer.WriteLine($"{latLonData[zip]}");
            }
        }
    }


    //===================================================================CityStates==============================================

    
    /*
    Method to find all states in which a city is present based on cities.txt
    */
    static Dictionary<string, SortedSet<string>> GetCityStates(string citiesFile, string zipcodesFile)
    {
        // Read cities from citiesFile into a HashSet, ensuring uppercase and trimmed format
        var cities = new HashSet<string>(File.ReadLines(citiesFile).Select(line => line.ToUpper().Trim()));
        var cityStatesValues = new Dictionary<string, SortedSet<string>>();

        // Process each line in zipcodesFile to associate cities with states
        foreach (var line in File.ReadLines(zipcodesFile))
        {
            var fields = line.Split('\t');
            var city = fields[3].Trim().ToUpper();
            var state = fields[4].Trim();

            // Check if city is in cities set
            if (cities.Contains(city))
            {
                // Initialize a new sorted set if the city key does not exist
                if (!cityStatesValues.ContainsKey(city))
                {
                    cityStatesValues[city] = new SortedSet<string>();
                }
                // Add the state to the city's set
                cityStatesValues[city].Add(state);
            }
        }
        return cityStatesValues;
    }


    // /*
    // Method to output the CityStates results to an output file
    // */
    static void OutputCityStates(string filename, Dictionary<string, SortedSet<string>> cityStatesData, string citiesFile)
    {
        using (var writer = new StreamWriter(filename))
        {
            // Read cities in the original order from citiesFile. ChatGPT helped me here a bit as well with 
            // noticing that this would fix the error in my results
            foreach (var city in File.ReadLines(citiesFile).Select(line => line.Trim().ToUpper()))
            {
                if (cityStatesData.ContainsKey(city))
                {
                    var states = cityStatesData[city];
                    writer.WriteLine($"{string.Join(" ", states)}");
                }
                else
                {
                    // added case for which no state exists
                    writer.WriteLine($"{city}: No matching states found.");
                }
            }
        }
    }
} // end class
