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
  // => Used Pointers from lines 10 to 15 <=
  // => Used Pointers from lines 40 to 63 <=
  

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

        // LatLon

        // CityStates



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



} // end class
