using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{

    class Program
    {
        public static string Worldstring = "Ham and Eggs are better than \"Hello World!\"";
        public static int TheAnswer = 42;
        public static bool WorldFlat = true;

        static void Main(string[] args)
        {

            Console.WriteLine(Worldstring);
            Console.WriteLine("World is flat: " + WorldFlat);
            Console.WriteLine("Whats the answer to everything? ");
            Console.WriteLine(TheAnswer);
            Console.ReadKey();



        }
    }
}
