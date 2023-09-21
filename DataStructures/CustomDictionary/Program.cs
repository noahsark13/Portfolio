using System;
using System.Reflection;
// Noah Kasper
// 12/1/2022
// Homework #6: Custom Dictionary

namespace CustomDictionary
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CustomDictionary<string, string> dictionary = new CustomDictionary<string, string>(5);
            dictionary.Add("pizza", "garlic");
            dictionary.Add("hamberger", "bacon and cheese");
            dictionary.Add("taco", "beef");
            dictionary["milkshake"] = "vanilla";
            dictionary["stirfry"] = "chicken";
            dictionary["pretzel"] = "baked";


            string input = "";

            // loops until quit
            while (input != "quit")
            {
                Console.WriteLine("Custom Dictionary menu: Count  LoadFactor  Add  Remove  Get  Set  Clear  Quit");
                Console.Write(" >> ");
                input = Console.ReadLine().ToLower();

                switch(input)
                {
                    case "count":
                        {
                            Console.WriteLine("The dictionary has " + dictionary.Count + " entries");
                            break;
                        }
                    case "loadfactor":
                        {
                            Console.WriteLine("The dictionary has a load factor of " + dictionary.LoadFactor);
                            break;
                        }
                    case "add":
                        {
                            Console.Write("Type a key: ");
                            string key = Console.ReadLine();
                            Console.Write("Type a value: ");
                            string value = Console.ReadLine();

                            try
                            {
                                dictionary.Add(key, value);
                                Console.WriteLine($"The key '{key}' was added");

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            
                            break;
                        }
                    case "remove":
                        {
                            Console.Write("Type a key: ");
                            string key = Console.ReadLine();

                            // calls the remove method in the if statement
                            if (dictionary.Remove(key))
                            {
                                Console.WriteLine($"The key '{key}' was removed");
                            }
                            else
                            {
                                Console.WriteLine($"The key '{key}' is not in the dictionary.");
                            }

                           
                            break;
                        }
                    case "get":
                        {
                            Console.Write("Type a key: ");
                            string key = Console.ReadLine();

                            string foundKey;

                            // This if statement feels redundant, but it was noted to be part of the "get" action
                            // on the homework6 assignment sheet.
                            if (dictionary.ContainsKey(key))
                            {
                                try
                                {
                                    foundKey = dictionary[key];
                                    Console.WriteLine("Value is: " + foundKey);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("That key does not exist.");
                            }

                            break;
                        }
                    case "set":
                        {
                            Console.Write("Type a key: ");
                            string key = Console.ReadLine();
                            Console.Write("Type a value: ");
                            string value = Console.ReadLine();

                            int oldCount = dictionary.Count;

                            dictionary[key] = value;

                            // Checks if a value was changed or if a new CustomPair was added
                            if (dictionary.Count > oldCount)
                            {
                                Console.WriteLine($"The key '{key}' was added");
                            }
                            else
                            {
                                Console.WriteLine("The value was changed for the key '" + key + "'");
                            }
                            break;
                        }
                    case "clear":
                        {
                            dictionary.Clear();
                            Console.WriteLine("Dictionary was cleared");
                            break;
                        }
                    case "quit":
                        {
                            Console.WriteLine("Goodbye!");
                            break;
                        }
                }

                Console.WriteLine();
            }

        }
    }
}