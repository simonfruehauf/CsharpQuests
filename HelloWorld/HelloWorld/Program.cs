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
        public enum OperationType
        {
            subtract, 
            add, 
            multiply,
            divide
        } 

        static void Main(string[] args)
        {
            Calculator();
        }
        static void Calculator()
        {
            int a;
            retryVarA:
            Console.Write("Input variable A: ");
            try
            {
                a = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Variable needs to be an Int. ");
                goto retryVarA;
            }

            OperationType operation;
        retryOperator:
            Console.Write("Input an operator: ");

            switch (Console.ReadLine())
            {
                case "plus":
                    goto case "+";
                case "add":
                    goto case "+";
                case "+":
                    operation = OperationType.add;
                    break;
                case "minus":
                    goto case "-";
                case "remove":
                    goto case "+";
                case "-":
                    operation = OperationType.subtract;
                    break;
                case "multiply":
                    goto case "x";
                case "*":
                    goto case "+";
                case "x":
                    operation = OperationType.multiply;
                    break;
                case "by":
                    goto case "/";
                case "divide":
                    goto case "/";
                case "/":
                    operation = OperationType.divide;
                    break;
                default:
                    Console.WriteLine("Unknown operator, try agtain.");
                    goto retryOperator;
            }
            int b;
            retryVarB:
            Console.Write("Input variable B: ");
            try
            {
               b = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Variable needs to be an Int. ");
                goto retryVarB;
            }
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
            retryName:
            Console.Write("Please input your name: ");
            name = Console.ReadLine();
            if (name=="")
            {
                goto retryName;

            }          

            Console.WriteLine("Hello, " + name + "!");

            int age;
            retryAge:
            Console.Write("Please input your age as a number: ");
            try
            {
                age = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                goto retryAge;
            }

            
            if (name==age.ToString())
            {
                Console.WriteLine("Your Name is the same as your age. Interesting.");
            }
            Console.WriteLine("Hello, " + name + "! Thank you for telling me your age (which is " + age + ").");
            Console.ReadLine();


        }
    }
}
