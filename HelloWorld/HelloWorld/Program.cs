using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


#pragma warning disable IDE0051
namespace HelloWorld
{
    class Read
    {
        static readonly string ConsoleIndicator = "\n> ";
        static bool valid = false;
        public static int Int(string ConsoleText, string ErrorMessage)
        {
            valid = false;
            int integer = 0;
            do
            {
                try
                {
                    Console.Write(ConsoleText + ConsoleIndicator);
                    string temp = Console.ReadLine();
                    if (temp == "q" || temp == "quit")
                    {
                        Program.MainMenu();
                        return 0;
                    }
                    integer = Convert.ToInt32(temp);
                    valid = true;
                }
                catch
                {
                    Console.WriteLine(ErrorMessage);
                    valid = false;
                }

            } while (!valid);
            return integer;
        }
        static float Float(string ConsoleText, string ErrorMessage)
        {
            valid = false;
            float floatnumber = 0;
            do
            {
                try
                {
                    Console.Write(ConsoleText + ConsoleIndicator);
                    string temp = Console.ReadLine();
                    if (temp == "q" || temp == "quit")
                    {
                        Program.MainMenu();
                        return 0;
                    }
                    floatnumber = float.Parse(temp);
                    valid = true;
                }
                catch
                {
                    Console.WriteLine(ErrorMessage);
                    valid = false;
                }

            } while (!valid);
            return floatnumber;
        }
        public static string String(string ConsoleText)
        {
            Console.Write(ConsoleText + ConsoleIndicator);
            string input = Console.ReadLine();
            return input;
            //valid = false;
            //return "";
        }

        public static bool YesNo(string ConsoleText, string ErrorMessage)
        {
            string yesno = "Y/N";
            valid = false;
            bool yes = false;
            do
            {
                Console.WriteLine(ConsoleText + " " + yesno);
                ConsoleKey input = Console.ReadKey().Key;
                Console.WriteLine();
                if (input == ConsoleKey.Q)
                {
                    Program.MainMenu();
                    return false;
                }
                else if (input == ConsoleKey.Y)
                {
                    valid = true;
                    yes = true;
                }
                else if (input == ConsoleKey.N)
                {
                    valid = true;
                    yes = false;
                }
                else
                {
                    Console.WriteLine(ErrorMessage);
                    valid = false;
                }

            } while (!valid);
            return yes;
        }
    }
    class Program
    {
        #region randomvars
        public static string Worldstring = "Ham and Eggs are better than \"Hello World!\"";
        public static int TheAnswer = 42;
        public static bool WorldFlat = true;
        #endregion
        public static string UserName;
        public static int UserAge;
        static bool MainMenuOpened;
        public enum ReturnType
        {
            valid,
            invalid,
            quit
        }
        public enum OperationType
        {
            naught,
            subtract,
            add,
            multiply,
            divide,
            retry
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
        public static Random rnd = new Random(DateTime.Now.GetHashCode());
        static bool calendarHasMonth;
        static bool calendarHasYear;
        static int guessingNumber = 0;
        static void Main()
        {
            MainMenu();
        }
        public const string divider = "----------------------------";
        public static void DrawDivider(string a_divider = divider)
        {
                Console.WriteLine(a_divider);
        }
        public static void MainMenu(bool unknownInput = false)
        {
            if (!unknownInput)
            {
                Console.WriteLine("Welcome " + ((MainMenuOpened == true) ? "back " : "") + "to the Main Menu.");
            }
            MainMenuOpened = true;
            string input = Read.String("Please input your command.");
            switch (input)
            {
                case "potion game":
                case "potion seller":
                case "potion":
                case "p":
                    Gambling.Game();
                    return;
                case "help":
                case "h":
                    Console.WriteLine("Here a list of available commands. ()Brackets indicate shortcuts. \n> help \n> calendar \n> guessing game \n> super smart (ai) \n> (c)alculator \n> name \n> age \n> random number \n> (e)xit or (q)uit");
                    break;
                case "calendar":
                    DrawMonth();
                    break;
                case "q":
                case "quit":
                case "exit":
                case "e":
                    Environment.Exit(1);
                    break;
                case "super smart ai":
                case "super smart AI":
                case "AI":
                case "ai":
                    SuperSmartAI();
                    break;
                case "calculator":
                case "calc":
                case "c":
                    Calculator();
                    break;
                case "name":
                    Name();
                    break;
                case "age":
                    Age();
                    break;
                case "random number":
                    GenerateRandomNumberBetween();
                    break;
                case "guessing game":
                    GuessingGame(1, 5);
                    break;
                case "remindme":
                    RemindMe();
                    break;
                case "arraystuff":
                    ArrayStuff();
                    break;
                default:
                    Console.Write("Unknown Input. ");
                    MainMenu(true);
                    break;
            }
            MainMenu();
        }

        public static void ArrayStuff()
        {
            int[,] int2dArray = new int[4, 4];
            int counter = 0;
            for (int i = 0; i < int2dArray.GetLength(0); i++)
            {
                for (int j = 0; j < int2dArray.GetLength(1); j++)
                {
                    int2dArray[i, j] = counter;
                    counter++;
                }
            }
            counter = 0;
            foreach (int item in int2dArray)
            {
                if (counter%4 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write(item + " ");
                counter++;
            }
            Console.ReadLine();
        }

        void LoopThroughForeach(List<string> M_list)
        {
            Console.WriteLine("Printing all items on list.");
            foreach (string item in M_list)
            {
                Console.WriteLine(item);
            }

        }

        public static void RemindMe()
        {
            List<string> inputList = new List<string>();
            string input = Read.String("");
            while (!input.Contains("end"))
            {
                inputList.Add(input);
                input = Read.String("");
            }
           foreach (string item in inputList)
            {
                    Console.WriteLine(item);
            }
            inputList.Clear();  // not needed, but a good practice for me to 
                                // remember doing in case the variable is 
                                // declared outside of the scope
        }

        void LoopThroughFor(string[] m_array)
        {
            Console.WriteLine("Printing all items on list.");
            for (int i = 0; i < m_array.Length; i++)
            {
                Console.WriteLine(m_array[i]);
            }
        }

        static void GuessingGame(int a, int b, bool generate = true)
        {
            DrawDivider();
            if (a > b)
            {
                Console.WriteLine(a + "is bigger than " + b + ". Cancelling request.");
                return;
            }
            //Generate a random number the user has to guess. Ask the user for a number in a range of [n1-nn] and compare it with the value
            if (generate)
            {
                guessingNumber = rnd.Next(a, b);
                Console.WriteLine("Generating a number from " + a + " to " + b + "...");
            }
            int input = Read.Int("Guess the number!", "Not a valid number. Please guess again.");
            if (input == guessingNumber)
            {
                Console.WriteLine("Congratulations! You Win!");
                DrawDivider();
            }
            else if (input > guessingNumber)
            {
                Console.Write("You guessed too high... ");
                if (Read.YesNo("Guess again?", "Invalid input."))
                {
                    GuessingGame(a, b, false);
                    return;
                }
                DrawDivider();
            }
            else if (input < guessingNumber)
            {
                Console.Write("You guessed too low... ");
                if (Read.YesNo("Guess again?", "Invalid input."))
                {
                    GuessingGame(a, b, false);
                    return;
                }
                DrawDivider();
            }
            else
            {
                Console.Write("ERROR."); //shouldn't be able to reach this code
            }
        }
        static void DrawMonth()
        {
            bool current;
            switch (Read.String("Do you want the current or a different month?"))
            {
                case "c":
                case "current":
                    current = true;
                    break;
                case "d":
                case "different":
                    current = false;
                    break;
                case "q":
                case "quit":
                case "b":
                case "back":
                    return;
                default:
                    DrawMonth();
                    return;
            }

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
                    year = Read.Int("Please input the year you're looking for:", "Sorry, that was not a valid year. Please input a positive integer.");
                    calendarHasYear = true;
                }
                if (!calendarHasMonth)
                {
                    month = Read.Int("Please input the month you're looking for:", "Sorry, that was not a valid month. Please input a integer between 1 and 12.");
                    calendarHasMonth = true;
                }
            }
            #endregion
            #region DrawHeaders
            Console.Write("\n");
            Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + ", " + year);
            Console.WriteLine("Su Mo Tu We Th Fr Sa"); //weird cause some ppl start week on sunday
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
            int daysInMonth = DateTime.DaysInMonth(year, month); //get the amount of days in the month for this section of the calendar to draw
            int[,] calendar = new int[6, 7]; //make our calendar 2D array, in this case being the weeks & weekdays
            int dayOfWeek = Convert.ToInt32(date.DayOfWeek) + 1; //get the day of the week of the first day in the month as an int, plus one because week does not start on sunday
            int currentlyDrawingDay = 1; //start counting here

            for (int x = 0; x < calendar.GetLength(0); x++) //if x(current day of the week) is within the week (length of 0 axis in calendar array)
            {
                for (int y = 0; y < calendar.GetLength(1) && (currentlyDrawingDay - dayOfWeek + 1 /*plus one because we start counting at 1*/) <= daysInMonth; y++) //if y is within the week, and the currently drawing day minus the day of the week is within the month
                {
                    calendar[x, y] = currentlyDrawingDay - dayOfWeek + 1; //set the day to the index minus the starting date 
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
                Console.WriteLine();
            }
            #endregion
            #region ClearMonthYear
            calendarHasMonth = false;
            calendarHasYear = false;
            #endregion
        }

        static double GenerateRandomNumberBetween(bool floatnumbers = false)
        {
            DrawDivider();
            int a = Read.Int("Please input the minmum:", "Sorry, that was not a valid integer. Try again.");

            int b = Read.Int("Please input the maximum:", "Sorry, that was not a valid Integer. Try again.");

            double randomnumber;
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
            Console.WriteLine("Random Number: " + randomnumber);
            DrawDivider();
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
            string input = Read.String("Input a secret passphrase: ");

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

            DrawDivider();
            string input = Read.String("I am an AI. Hello. Ask me any question you'd like.");
            while (!input.Contains("exit") && !input.Contains("goodbye") && !input.Contains("quit"))
            {
                Console.WriteLine("Very good question, I will come back to you in a moment!");

                input = Read.String("I am an AI. Hello. Ask me any question you'd like.");
            }

            DrawDivider();
        }

        static void PrintWeekdayEnum()
        {
            foreach (string item in Enum.GetNames(typeof(Weekdays)))
            {
                Console.WriteLine(item);
            }
        }

        static void Calculator()
        {

            DrawDivider();
            Console.WriteLine("Calculator initiated.");
            if (!GetIntegerInput("A", out int a))
            {
                return;
            }

            if (!SetOperator(out OperationType operation))
            {
                return;
            }
            int b;
            if (operation != OperationType.naught)
            {
                if (!GetIntegerInput("B", out b))
                {
                    return;
                }
            }
            else
            {
                return;
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
                case OperationType.retry:
                    c = 0;
                    break;
                case OperationType.naught:
                    return;
                default:
                    c = 0;
                    break;
            }
            Console.WriteLine("Equals to " + c);
            DrawDivider();
        }
        static bool GetIntegerInput(string intName, out int integer)
        {
            switch (SetInteger(intName, out integer))
            {
                case ReturnType.valid:
                    return true;
                case ReturnType.invalid:
                    GetIntegerInput(intName, out integer);
                    return true;
                case ReturnType.quit:
                    return false;
                default:
                    return false;
            }
        }
        static ReturnType SetInteger(string intName, out int integer)
        {
            Console.Write("Input integer value for " + intName + ": ");
            try
            {
                string input = Console.ReadLine();
                if (input == "q")
                {
                    integer = 0;
                    return ReturnType.quit;
                }
                integer = Convert.ToInt32(input);
            }
            catch (Exception)
            {
                Console.WriteLine("Input needs to be an integer.");
                integer = 0;
                return ReturnType.invalid;
            }
            return ReturnType.valid;
        }
        static bool SetOperator(out OperationType localoperator)
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
                case "q":
                case "quit":
                case "back":
                    return false;
                default:
                    Console.WriteLine("Unknown operator, try again.");
                    localoperator = OperationType.retry;
                    break;
            }
            return true;
        }
        static void DataTypes()
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
            else if (name == "back" || name == "b" || name == "q" || name == "quit")
            {
                return;
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
    public class Gambling
    {
        static int itemWidth = 9;
        const int maxItems = 5;
        static readonly string[] Bottle = new string[] 
        { 
            "   { }   " ,
            "   | |   ", 
            "   ) (   ", 
            "  |___|  " , 
            "  |   |  " , 
            "  |___|  " 
        };

        static readonly string[] SelectionArrow = new string[]
        {
            "    ^    ",
            "    |    "
        };
        struct Player
        {
            public int maxHealth;
            public int health;
            public bool poisoned;
            public Player(int a_MaxHealth) //simple init with max health
            {
                maxHealth = a_MaxHealth;
                health = maxHealth;
                poisoned = false;
            }
            public Player(int a_MaxHealth, bool hasMaxHealth, int a_Health, bool a_Poisoned = false) //init with all data health
            {
                maxHealth = a_MaxHealth;
                if (!hasMaxHealth)
                {
                    health = a_Health;
                }
                else
                {
                    health = maxHealth;
                }
                poisoned = a_Poisoned;
            }
        }

        public enum PotionTypes
        {
            Yellow,
            Blue,
            Brown,
            Pink,
            Red,
            Blank,
            Purple,
            Green, 
            Cyan,
            White,
            Black
        }
        
        enum PotionEvents
        {
            Damage,
            Heal,
            Nothing
        }
        public static List<PotionTypes> shop = new List<PotionTypes>();
        public static int score;
        public static void Game()
        {
            Player currentPlayer = new Player(10);
            score = 0;
            int itemIndex = 0;
            Console.WriteLine("Welcome Traveler. I am the Potion Seller. I have the strongest potions in the land.");
            do
            {

                PopulateShop(out shop);
                Console.WriteLine();
                Console.WriteLine("These are my potions. Please do not touch them.");
                DrawBottle(maxItems);
                ConsoleKey keyinfo;
                Console.WriteLine();
                RedrawArrow(itemIndex);


                do
                {

                    keyinfo = Console.ReadKey().Key;
                    switch (keyinfo)
                    {
                        case ConsoleKey.LeftArrow:
                            if (itemIndex > 0)
                            {
                                itemIndex--;
                                MoveCursorUp(2);
                                RedrawArrow(itemIndex);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (itemIndex < maxItems-1)
                            {
                                itemIndex++;
                                MoveCursorUp(2);
                                RedrawArrow(itemIndex);
                            }
                            break;

                        case ConsoleKey.Enter:
                            Console.WriteLine();
                            Console.WriteLine("<You picked a " + shop[itemIndex].ToString() + " potion.>");
                            break;
                        default:
                            break;
                    }
                } while (keyinfo != ConsoleKey.Enter);

                switch (GetEvent(shop[itemIndex]))
                {
                    case PotionEvents.Damage:
                        currentPlayer = DamagePlayer(currentPlayer, 2);
                        break;
                    case PotionEvents.Heal:
                        currentPlayer = DamagePlayer(currentPlayer, -1); //heals
                        break;
                    case PotionEvents.Nothing:
                        Console.WriteLine("<Nothing happened after drinking the potion.>");
                        break;
                    default:
                        break;
                }

                // here: select potion

                // switch on potion type

                // display health / redraw health
                itemIndex = 0;
                score++;
            } while (currentPlayer.health > 0);
            Console.WriteLine("I told you Traveler... You were not strong enough for my potions...\n <<You drank " + score + ((score > 0) ? "potions.>>" : "potion.>>"));
        }

        static PotionTypes GetPotion()
        {
            return (PotionTypes)Program.rnd.Next(Enum.GetNames(typeof(PotionTypes)).Length);
        }
        static Player DamagePlayer(Player a_Player, int damage)
        {
        #pragma warning disable IDE0059 // Unnecessary assignment of a value

            string _output;
            if (damage <= 0)
            {
                a_Player.health -= damage;
                if (a_Player.health > a_Player.maxHealth)
                {
                    a_Player.health = a_Player.maxHealth;
                }

                Console.WriteLine("<You gained [" + (damage * -1) + "] health, and now have [" + a_Player.health.ToString() + "].>");
                switch (Program.rnd.Next(2))
                {
                    case 0:
                        _output = "You managed to drink one of my potions? Congratiolations traveler.";
                        break;
                    case 1:
                        _output = "I would not have thought you'd live.";
                        break;
                    case 2:
                        _output = "Congratiolations traveler, on surviving one of my strongest potions!";
                        break;
                    default:
                        _output = "You managed to drink one of my potions? Congratiolations traveler.";
                        break;
                }
                return a_Player;
            }
            if (a_Player.health - damage > 0)
            {
                a_Player.health -= damage;
                Console.WriteLine("<You took [" + damage + "] damage, and have [" + a_Player.health.ToString() + "] left.>");
            }
            else
            {
                a_Player.health -= damage;
                Console.WriteLine("<You took [" + damage + "] damage and died.>");
                Console.WriteLine("I told you! I told you! ... I told you!!!!!");
                GameOver();
            }

            switch (Program.rnd.Next(3))
            {
                case 0:
                    _output = "You can't handle my strongest potions! No one can! My strongest potions are fit for a beast let alone a man.";
                    break;
                case 1:
                    _output = "You can't handle my potions. They're too strong for you.";
                    break;
                case 2:
                    _output = "My strongest potions will kill a dragon, let alone a man. You need a seller that sells weaker potions, because my potions are too strong.";
                    break;
                case 3:
                    _output = "My strongest potions would kill you, traveler. You can't handle my strongest potions. You'd better go to a seller that sells weaker potions.";
                    break;
                default:
                    _output = "You can't handle my strongest potions! No one can! My strongest potions are fit for a beast let alone a man.";
                    break;
            }
            Console.WriteLine(_output);
            return a_Player;
        }
        #pragma warning restore IDE0059 // Unnecessary assignment of a value

        static void GameOver()
        {
            Console.WriteLine("<Congratulations on drinking " + score + " potions.>\n<Going back to the main menu.>");
            Program.DrawDivider();
            Program.MainMenu();
        }
        static PotionEvents GetEvent(PotionTypes a_potionToCheck)
        {
            switch (a_potionToCheck)
            {
                case PotionTypes.Yellow:
                    return PotionEvents.Damage;
                case PotionTypes.Blue:
                    return PotionEvents.Damage;
                case PotionTypes.Brown:
                    return PotionEvents.Damage;
                case PotionTypes.Pink:
                    return PotionEvents.Heal;
                case PotionTypes.Red:
                    return PotionEvents.Heal;
                case PotionTypes.Blank:
                    return PotionEvents.Nothing;
                case PotionTypes.Purple:
                    return PotionEvents.Damage;
                case PotionTypes.Green:
                    return PotionEvents.Damage;
                case PotionTypes.Cyan:
                    return PotionEvents.Nothing;
                case PotionTypes.White:
                    return PotionEvents.Nothing;
                case PotionTypes.Black:
                    return PotionEvents.Nothing;
                default:
                    return PotionEvents.Nothing;
            }
        }

        public static void DrawBottle(int amount)
        {
            int count = 0;
            foreach (string line in Bottle)
            {
                for (int i = 0; i < amount; i++)
                {
                    Console.Write(Bottle[count]);
                }
                Console.WriteLine();
                count++;
            }
            for (int i = 0; i < maxItems; i++)
            {
                int charCount = shop[i].ToString().Length;
                string lable = shop[i].ToString();
                bool flipFlop = false;
                while (charCount != itemWidth-1)
                {
                    if (charCount >= itemWidth)
                    {
                        Console.WriteLine();
                        break;
                    }
                    switch (flipFlop)
                    {
                        case true:
                            lable = " " +lable;
                            flipFlop = !flipFlop;
                            break;
                        case false:
                            lable += " ";
                            flipFlop = !flipFlop;
                            break;
                        default:
                            break;
                    }
                    charCount = lable.Length;
                }

                Console.Write(lable + "|"); ///ADD POTION NAME
            }
        }
        public static void PopulateShop(out List<PotionTypes> shop)
        {
            shop = new List<PotionTypes>();

            for (int i = 0; i < maxItems; i++)
            {
                PotionTypes thistype = (PotionTypes)Program.rnd.Next(Enum.GetNames(typeof(PotionTypes)).Length);
                shop.Add(thistype);
                //Console.WriteLine(thistype);
            }
        }
        public static void RedrawArrow(int offset)
        {
            int count = 0;
            foreach (string line in SelectionArrow)
            {
                if (count != 0)
                {
                    Console.WriteLine();
                }
                for (int i = 0; i < maxItems; i++)
                {

                    if (i == offset)
                    {
                        Console.Write(SelectionArrow[count]);
                    }
                    else 
                    {
                        Console.Write(String.Concat(Enumerable.Repeat(" ", itemWidth)));
                    }
                }
                count++;

            }
        }
        public static void MoveCursorUp(int amount = 1) //and delete
        {

            Console.SetCursorPosition(0, Console.CursorTop - amount+1);
            
            for (int i = 0; i < amount; i++)
            {
                Console.Write("                 ", 0, 80);
            }
            Console.WriteLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}
