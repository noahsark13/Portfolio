// Noah Kasper
// 11/13/2022
// Stacks and Queues

namespace CustomStacksAndQueues
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameStack<string> stack = new GameStack<string>();

            Console.WriteLine("The following spells are being put on the stack: ");
            Console.WriteLine();

            Console.WriteLine(" - Shock");
            stack.Push("Shock");
            Console.WriteLine(" - Fork");
            stack.Push("Fork");
            Console.WriteLine(" - Counterspell");
            stack.Push("Counterspell");
            Console.WriteLine(" - Force of Will");
            stack.Push("Force of Will");
            Console.WriteLine(" - Deflection");
            stack.Push("Deflection");

            Console.WriteLine();

            Console.WriteLine("Spells resolving (in reverse order): ");
            Console.WriteLine();
            while (stack.IsEmpty == false)
            {
                Console.WriteLine(" - " + stack.Peek());
                stack.Pop();
            }
            Console.WriteLine();

            // ------------------------------------------

            GameQueue<string> queue = new GameQueue<string>();
            Console.WriteLine("The following players are joining the queue: ");
            Console.WriteLine();
           
            Console.WriteLine(" - Bob");
            queue.Enqueue("Bob");
            Console.WriteLine(" - Tom");
            queue.Enqueue("Tom");
            Console.WriteLine(" - Sara");
            queue.Enqueue("Sara");
            Console.WriteLine(" - Jim");
            queue.Enqueue("Jim");
            Console.WriteLine(" - Larry");
            queue.Enqueue("Larry");

            Console.WriteLine();


            while (queue.IsEmpty == false)
            {
                Console.WriteLine("\"" + queue.Peek() + "\" has joined the server - " + (queue.Count - 1) + " player(s) left in queue");
                queue.Dequeue();
            }



        }
    }
}