// Noah Kasper
// Graphs: Dijkstra's Algorithm
// 4/20/2023

namespace Graphs
{
    /// <summary>
    /// Main
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Graph graph = new Graph();

            string selection = null;

            Console.WriteLine("Search 1: Starting in the main hall.");
            graph.ShortestPath("main hall");

            // Loops till their is a valid selection.
            while (selection == null)
            {
                Console.WriteLine();
                Console.Write("Which room are you trying to get to? ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                // Checks that the current choice is valid.
                if (graph.MapContainsRoom(choice))
                {
                    selection = choice;
                    graph.PrintPath(choice);
                }
                else
                {
                    Console.Write("Sorry, that is not a room.");
                }

            }

            Console.WriteLine("____________________________________________________");
            Console.WriteLine();


            selection = null;
            Console.WriteLine("Search 2: Starting in the kitchen.");
            graph.ShortestPath("kitchen");

            while (selection == null)
            {
                Console.WriteLine();
                Console.Write("Which room are you trying to get to? ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                if (graph.MapContainsRoom(choice))
                {
                    selection = choice;
                    graph.PrintPath(choice);
                }
                else
                {
                    Console.Write("Sorry, that is not a room.");
                }

            }



        }


    }
}