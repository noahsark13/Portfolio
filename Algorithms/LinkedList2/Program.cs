// Noah Kasper
// 3/20/2023
// Linked List

namespace LinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomLinkedList<string> linked = new CustomLinkedList<string>();

            Console.WriteLine("Linked list is created. Please add 5 inventory items to the list.");
            for (int i = 1; i <= 5; i++)
            {
                Console.Write("Enter item #" + i + ": ");
                string input = Console.ReadLine();
                linked.Add(input);
            }

            Console.WriteLine();

            try
            {
                Console.WriteLine("The list has 5 items. These items are:");
                for (int i = 0; i < linked.Count; i++)
                {
                    Console.WriteLine(" - " + linked[i]);
                }

                Console.WriteLine();

                Console.WriteLine("Removing item at index 99...");
                Console.WriteLine(linked.RemoveAt(99));

            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

            Console.WriteLine("Removing item at index 4...");
            Console.WriteLine("Removed " + linked.RemoveAt(4));

            Console.WriteLine("Removing item at index 0...");
            Console.WriteLine("Removed " + linked.RemoveAt(0));

            Console.WriteLine("Removing item at index 1...");
            Console.WriteLine("Removed " + linked.RemoveAt(1));

            Console.WriteLine();


            try
            {
                Console.WriteLine("The list has " + linked.Count + " items. These are :");
                for (int i = 0; i < linked.Count; i++)
                {
                    Console.WriteLine(" - " + linked[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            


        }

    }
}