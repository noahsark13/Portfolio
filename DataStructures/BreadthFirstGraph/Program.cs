// Noah Kasper
// Graphs
// 4/10/2023

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
            string playerRoom = "main hall";
            string choice = null;

            //graph.ListAllVertices();

            Console.WriteLine("BFS starting at MAIN HALL");
            Console.WriteLine();

            graph.BreadthFirst("main hall");




        }


    }
}