using System;
using System.Collections.Generic;
using System.Globalization;
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
        static bool MainMenuOpened;
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
        public enum Months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        public enum EnumSecret
        {
            Power,
            Bananas,
            Complication,
            Programming,
            Everything
        }
        public static Random rnd = new Random();
        static bool calendarHasMonth;
        static bool calendarHasYear;
        static bool inMainMenu;
        static void Main(string[] args)
        {
            MainMenu();
        }

        static void MainMenu(bool unknownInput = false)
        {
            inMainMenu = true;
            if (!unknownInput)
            {
                Console.WriteLine("Welcome " + ((MainMenuOpened == true) ? "back " : "") + "to the Main Menu.");
            }
            Console.WriteLine("Please input your command.");
            Console.Write("> ");
            MainMenuOpened = true;
            switch (Console.ReadLine())
            {
                case "help":
                case "h":
                    Console.WriteLine("Here a list of available commands. ()Brackets indicate shortcuts. \n help \n current calendar \n lookup calendar \n super smart (ai) \n (c)alculator \n name \n age \n random number \n (e)xit or (q)uit");
                    break;
                case "current calendar":
                    inMainMenu = false;
                    DrawMonth(true);
                    break;
                case "lookup calendar":
                    inMainMenu = false;
                    DrawMonth(false);
                    break;
                case "q":
                case "quit":
                case "exit":
                case "e":
                    System.Environment.Exit(1);
                    break;
                case "super smart ai":
                case "super smart AI":
                case "AI":
                case "ai":
                    inMainMenu = false;
                    SuperSmartAI();
                    break;
                case "calculator":
                case "calc":
                case "c":
                    inMainMenu = false;
                    Calculator();
                    break;
                case "name":
                    inMainMenu = false;
                    Name();
                    break;
                case "age":
                    inMainMenu = false;
                    Age();
                    break;
                case "random number":
                    inMainMenu = false;
                    GenerateRandomNumberBetween();
                    break;
                default:
                    Console.Write("Unknown Input. ");
                    MainMenu(true);
                    break;
            }
            MainMenu();
        }
        private static string ConsoleReadLineWithCancel()
        {
            string result = null;

            StringBuilder buffer = new StringBuilder();

            //The key is read passing true for the intercept argument to prevent
            //any characters from displaying when the Escape key is pressed.
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b");
                    buffer.Remove(buffer.Length - 1, 1);
                }
            }

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            if (info.Key == ConsoleKey.Escape)
            {
                if (inMainMenu)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine();
                    MainMenu();
                }
            }

            Console.WriteLine();
            return result;
        }
        static void DrawMonth(bool current = true)
        {
            int month = 0;
            int year = 0;
            #region GetMonthYear
            if (current)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;

            }
            else
            {
                if (!calendarHasYear)
                {
                    Console.Write("Please input the year you're looking for: ");

                    try
                    {
                        year = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could not convert this to a year. Please input a number.");
                        DrawMonth();
                    }
                    calendarHasYear = true;
                }
                if (!calendarHasMonth)
                {
                    Console.Write("Please input the month (as a number) you're looking for: ");

                    try
                    {
                        month = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could not convert this to a month. Please input a number.");
                        DrawMonth();
                    }
                    calendarHasMonth = true;
                }
            }
            #endregion
            #region DrawHeaders
            Console.Write("\n\n");
            Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + ", " + year);
            Console.WriteLine("Mo Tu We Th Fr Sa Su");
            #endregion
            #region FillCalendar
            /* todo: 
             * 1. get first day of the month. 
             * 2. get weekday of that day
             * 3. get last day of month / amount of days
             * 4. make a 2d array of the month
             * 5. populate array with offset depending on first weekday
             * 6. stop populating if we reached the last day of the month
             * 7. draw array
             * 7.1 don't forget that numbers can be single space
             * 8. ??? 
             * 9. profit
            */
            DateTime date = new DateTime(year, month, 1); // get the datetime for the first day of the month? i think
            int weekday = date.Day;
            int daysInMonth = DateTime.DaysInMonth(year, month); //get the amount of days in the month for this section of the calendar to draw
            int[,] calendar = new int[6,7]; //make our calendar 2D array, in this case being the weeks & weekdays
            int dayOfWeek =  Convert.ToInt32(date.DayOfWeek); //get the day of the week of the first day in the month as an int
            int currentlyDrawingDay = 1; //start counting here

            for (int x = 0; x < calendar.GetLength(0); x++) //if x(current day of the week) is within the week (length of 0 axis in calendar array)
            {
                for (int y = 0; y < calendar.GetLength(1) && (currentlyDrawingDay - dayOfWeek +1 /*plus one because we start counting at 1*/) <= daysInMonth; y++) //if y is within the week, and the currently drawing day minus the day of the week is within the month
                {
                    calendar[x, y] = currentlyDrawingDay - dayOfWeek +1; //set the day to the index minus the starting date 
                    currentlyDrawingDay++;

                }
            }
            #endregion
            #region DrawCalendar
            //x in this case is downwards, y is sideways (x week, y day)
            for (int x = 0; x < calendar.GetLength(0); x++)
            {
                for (int y = 0; y < calendar.GetLength(1); y++)
                {
                    if (calendar[x, y] > 0)
                    {
                        if (calendar[x, y] < 10) //if the date does not have 2 numbers, we need to add an extra space or a 0(to accommodate the missing number)
                        {
                            Console.Write("0" + calendar[x, y] + " ");
                        }
                        else
                        {
                            Console.Write(calendar[x, y] + " ");
                        }
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }
            }
            #endregion
            Console.ReadLine();
            #region ClearMonthYear
            calendarHasMonth = false;
            calendarHasYear = false;
            month = 0;
            year = 0;
            #endregion
        }

        static double GenerateRandomNumberBetween(int a = 0, int b = 1, bool floatnumbers = false)
        {
            try
            {
                Console.WriteLine("Please input the minmum: ");
                a = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected Input. Defaulting to 0.");
            }
            try
            {
                Console.WriteLine("Please input the maximum: ");
                b = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected Input. Defaulting to 0.");
            }
            double randomnumber = 0;
            if (floatnumbers)
            {
                Console.WriteLine("Generating random int number...");
                randomnumber = rnd.NextDouble();
            }
            else
            {
                Console.WriteLine("Generating random float number...");
                randomnumber = rnd.NextDouble() * (a - b) + b;
            }
            return randomnumber;
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
            Console.WriteLine("----------------------------");
            Console.WriteLine("I am an AI. Hello. Ask me any question you'd like.");

            string input ="";
            input = Console.ReadLine();
            while (!input.Contains("exit") && !input.Contains("goodbye"))
            {
                Console.WriteLine("Very good question, I will come back to you in a moment!");

                input = Console.ReadLine();
            }
            Console.WriteLine("----------------------------");
            MainMenu();
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
            Console.WriteLine("----------------------------");
            Console.WriteLine("Calculator initiated.");
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
            Console.WriteLine("----------------------------");
            MainMenu();
        }
        static void SetInteger(string intName, out int integer)
        {
            Console.Write("Input integer value for " + intName + ": ");
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
                case "b":
                case "back":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Unknown operator, try again.");
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
            if (name == "" && name != "b" && name != "back")
            {
                Name();
            }
            else if (name == "back" || name == "b")
            {
                MainMenu();
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
