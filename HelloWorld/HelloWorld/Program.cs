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
        public static string UserName;
        public static int UserAge;
        public enum OperationType
        {
            naught,
            subtract,
            add,
            multiply,
            divide
        }

        public enum Weekdays
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        public enum EnumSecret
        {
            Power,
            Bananas,
            Complication,
            Programming,
            Everything
        }

        static void Main(string[] args)
        {

            PrintGrid(10, 10);
            Console.ReadLine();

        }
        static void PrintGrid(int x, int y)
        {
            for (int localx = 0; localx < x; localx++)
            {
                for (int localy = 0; localy < y; localy++)
                {
                    Console.Write(localx + "/" + localy);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static string Secret()
        {
            string input = "";
            Console.Write("Input a secret passphrase: ");
            input = Console.ReadLine();

            switch (input)
            {
                case "Power":
                    return "The secret to power is... Milkshakes.";
                case "Bananas":
                case "Banana":
                case "bananas":
                case "banana":
                    return "Banana.Banana.Banana.Banana.Banana.Banana.Banana.Banana.Banana.Banana.Banana.";
                case "complication":
                    return "Sorry, there has been a complication. Goodbye.";
                case "Programming":
                    return "The secret to programming is trial and error.";
                case "Everything":
                    return "42";
                default:
                    return "Invalid Password.";
            }
        }

        static void Naughty(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine("\"I have been bad today, sorry!\"");
            }
        }
        static void SuperSmartAI()
        {
            Console.WriteLine("I am an AI. Ask me any question you'd like.");

            string input ="";
            input = Console.ReadLine();
            while (input != "Goodbye.")
            {
                Console.WriteLine("Very good question, I will come back to you in a moment!");

                input = Console.ReadLine();
            }
            Console.Read();
        }

        static void PrintWeekdayEnum()
        {
            foreach (var item in Enum.GetNames(typeof(Weekdays)))
            {
                Console.WriteLine(item);
            }
        }

        static void Calculator()
        {

            int a;
            SetInteger("A", out a);
            OperationType operation = OperationType.naught;
            SetOperator(out operation);
            int b;
            SetInteger("B", out b);
            int c;
            switch (operation)
            {
                case OperationType.subtract:
                    c = a - b;
                    break;
                case OperationType.add:
                    c = a + b;
                    break;
                case OperationType.multiply:
                    c = a * b;
                    break;
                case OperationType.divide:
                    c = a / b;
                    break;
                default:
                    c = 0;
                    break;
            }
            Console.WriteLine("Equals to " + c);
            Console.ReadLine();
        }
        static void SetInteger(string intName, out int integer)
        {
            Console.Write("Input variable " + intName + ": ");
            try
            {
                integer = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Input needs to be an integer.");
                SetInteger(intName, out integer);
                return;
            }
        }
        static void SetOperator(out OperationType localoperator)
        {
            localoperator = OperationType.naught;
            Console.Write("Input an operator: ");
            switch (Console.ReadLine())
            {
                case "plus":
                case "add":
                case "+":
                    localoperator = OperationType.add;
                    break;
                case "minus":
                case "remove":
                case "-":
                    localoperator = OperationType.subtract;
                    break;
                case "multiply":
                case "*":
                case "x":
                    localoperator = OperationType.multiply;
                    break;
                case "by":
                case "divide":
                case "/":
                    localoperator = OperationType.divide;
                    break;
                default:
                    Console.WriteLine("Unknown operator, try agtain.");
                    SetOperator(out localoperator);
                    break;
            }
        }
        static void dataTypes()
        {
            bool boolean = true;
            int integer = 1;
            double doublenumber = 1.00;
            float floatpoint = 1.11f;
            string textstring = "random text";
            char singlechar = 'a';
            Console.WriteLine("bool " + boolean);
            Console.WriteLine("int " + integer);
            Console.WriteLine("double " + doublenumber);
            Console.WriteLine("float " + floatpoint);
            Console.WriteLine("string " + textstring);
            Console.WriteLine("char " + singlechar);
        }
        static void ReadConsoleLine()
        {
            string amount = Console.ReadLine();
            int number;


            try
            {
                number = Convert.ToInt16(amount);
                Console.WriteLine(AddOne(number));
            }
            catch
            {
                Console.WriteLine("Not a number.");
            }

            Console.ReadLine();
        }
        static void Banana()
        {
            int sliceAmount = 1;
            bool sliced = true;

            switch (sliced)
            {
                case false:
                    Console.WriteLine("The banana is not sliced. " + "sliced = " + sliced);
                    break;
                case true:
                    Console.WriteLine("The banana is sliced. " + "sliced = " + sliced);
                    break;
            }

            Console.WriteLine("There are " + sliceAmount + " slices.");

        }
        static int AddOne(int number)
        {
            number++;
            return number;
        }
        static void Maths()
        {
            int a;
            int b;

            a = 10;
            b = 69;

            int c = a + b;

            Console.WriteLine(c);

            float d;
            float e;

            d = 12.400f;
            e = 57.44f;

            float f = d - e;

            Console.WriteLine(f);

            int g = a * b;
            float h = d * e;

            float i = g / h;

            Console.WriteLine(i);
        }
        static void Name()
        {
            string name;
            Console.Write("Please input your name: ");
            name = Console.ReadLine();
            if (name == "")
            {
                Name();
            }

            Console.WriteLine("Hello, " + name + "!");
            UserName = name;
            Console.ReadLine();

        }
        static void Age()
        {
            int age = -1;
            Console.Write("Please input your age as a number: ");
            try
            {
                age = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input.");
                Age();
            }

            if (UserName != null)
            {
                Console.WriteLine("Hello, " + UserName + "! Thank you for telling me your age (which is " + age + ").");
                UserAge = age;
            }
            else
            {
                Console.WriteLine("Your age is " + age + ".");
                UserAge = age;
            }
            Console.ReadLine();
        }
    }
}
