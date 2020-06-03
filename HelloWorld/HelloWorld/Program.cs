using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
#pragma warning disable IDE0051 //unused var
#pragma warning disable IDE0044 //add readonly
#pragma warning disable IDE0060 //unused parameter
namespace HelloNamespace
{
    public class Read
    {
        public static readonly string ConsoleIndicator = "\n> ";
        static bool valid = false;
        public static int Int(string ConsoleText, string ErrorMessage)
        {
            valid = false;
            int integer = 0;
            do
            {
                try
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        Program.Menu();
                        return 0;
                    }
                    Console.Write(ConsoleText + ConsoleIndicator);
                    string temp = Console.ReadLine();
                    if (temp == "q" || temp == "quit")
                    {
                        Program.Menu();
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
                        Program.Menu();
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
                    Program.Menu();
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
    public class Write
    {
        public static void TypeLine(string line, int delay = 50, bool linebreak = false)
        {
            for (int i = 0; i < line.Length; i++)
            {
                Console.Write(line[i]);
                System.Threading.Thread.Sleep(delay);
            }
            if (linebreak)
            {
                Console.WriteLine();
            }
        }
    }
    public class Program
    {

        #region randomvars
        public static string Worldstring = "Ham and Eggs are better than \"Hello World!\"";
        public static int TheAnswer = 42;
        public static bool WorldFlat = true;
        #endregion
        public static string UserName;
        public static int UserAge;
        static bool MainMenuOpened;
        #region Menu
        static int selector;
        static string arrow = "<--";
        enum MenuItems
        {
            age,
            ai,
            arraystuff,
            calculator,
            calendar,
            cave,
            chessboard,
            cisharpmon,
            credits,
            guessinggame,
            iterator,
            map,
            name,
            negative,
            pathfinder,
            potionseller,
            randomnumber,
            readmap,
            remindme,
            roguelike,
            roots,
            rps,
            snake,
            sorter,
            spiral,
            spock,
            storefront,
            warehouse,
            weirdmenu,
            a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,
            quit

        };
        #endregion

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
        public struct Point2D
        {
            public float x, y;
            public int intx, inty;
            public Point2D(float x_value, float y_value)
            {
                x = x_value;
                y = y_value;
                intx = (int)x;
                inty = (int)y;
            }
        }
        public struct Vector2D
        {
            public Point2D origin;
            public Point2D direction;
            public Vector2D(float origin_x, float origin_y, float direction_x, float direction_y)
            {
                origin = new Point2D(origin_x, origin_y);
                direction = new Point2D(direction_x, direction_y);
            }
            public Vector2D(Point2D m_origin, Point2D m_direction)
            {
                origin = m_origin;
                direction = m_direction;
            }
        }
        string WhatIsFunction(string input)
        {
            Console.WriteLine("Hello " + input);
            return "Hello " + input;
        }
        public static Random rnd = new Random(DateTime.Now.GetHashCode());
        static bool calendarHasMonth;
        static bool calendarHasYear;
        static int guessingNumber = 0;
        static void Main()
        {
            Menu();
        }
        public const string divider = "----------------------------";
        public static void Menu()
        {
            //write Title
            Console.Clear();
            string title = "Hello. Welcome to my program.";
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(Console.WindowWidth / 2 - title.Length / 2, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(title);
            Console.ResetColor();


            int menuoptions = Enum.GetNames(typeof(MenuItems)).Length;
            bool exit = false;
            List<Point2D> positions = new List<Point2D>();
            int j = 0;
            int k = 0;
            int startbuffer = 5;
            int maxwordlength = 15;
            int buffer = 3;
            int maxperrow = ((Console.WindowHeight - 2) / 2);
            for (int i = 0; i < menuoptions; i++)
            {
                if (k*2 >= Console.WindowHeight-5)
                {

                    positions.Add(new Point2D(startbuffer + j * (maxwordlength + buffer + arrow.Length), 2+k * 2));
                    j++;
                    k = 0;
                }
                else
                {
                    positions.Add(new Point2D(startbuffer + j * (maxwordlength + buffer + arrow.Length), 2 + k * 2));
                    k++;
                }
            }
            int v = 0;
            foreach (Point2D item in positions)
            {
                Console.SetCursorPosition((int)item.x, (int)item.y);
                MenuItems menuItem = (MenuItems)v;
                Console.Write(menuItem.ToString());
                v++;
            }
            //return;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((int)positions[selector].x + maxwordlength, (int)positions[selector].y); //current position
                Console.Write(arrow);
                Console.ResetColor();


                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                    case ConsoleKey.UpArrow:

                        if ((selector % maxperrow) - 1 < 0)
                        {
                            selector = selector + (maxperrow - 1);
                            
                            if (selector >= menuoptions)
                            {
                                selector = menuoptions - 1;
                            }
                        }
                        else
                        {
                            selector = ((selector - 1) % menuoptions + menuoptions) % menuoptions; // keeps it between 0 and x and not -x and x
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selector + 1 >= menuoptions)
                        {
                            int l = (int)Math.Round((float)menuoptions / (float)maxperrow);
                            l = menuoptions - l;
                            selector = l;
                        }
                        else if (selector%maxperrow +1 >= maxperrow)
                        {
                            selector = selector - (maxperrow-1);
                        }
                        
                        else
                        {
                            selector = (selector + 1) % menuoptions;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (selector - maxperrow < 0)
                        {
                            int l = (menuoptions-1) / maxperrow;
                            if (selector + l*maxperrow >= menuoptions)
                            {
                                l--;
                            }
                            else
                            {
                                
                            }
                            l *= maxperrow;
                            selector += l;
                        }
                        else
                        {
                            selector = ((selector - maxperrow) % menuoptions + menuoptions) % menuoptions;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (selector + maxperrow >= menuoptions)
                        {

                            selector = (selector - maxperrow) % maxperrow;
                        }
                        else
                        {
                            selector = ((selector + maxperrow) % menuoptions + menuoptions) % menuoptions;
                        }
                        break;
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        //exit
                        selector = menuoptions-1;
                        exit = true;
                        break;
                    case ConsoleKey.H:
                        string helptext = "Press up & down to move the cursor, and enter to select.";
                        Console.SetCursorPosition(Console.WindowWidth / 2 - helptext.Length / 2, 1);
                        Console.Write(helptext);
                        Console.SetCursorPosition((int)positions[selector].x + maxwordlength, (int)positions[selector].y); //current position
                        break;
                    //help
                    default:
                        break;
                }
                //redraw arrow at index
                Console.SetCursorPosition(Console.CursorLeft - arrow.Length, Console.CursorTop);
                for (int i = 0; i < arrow.Length; i++)
                {
                    Console.Write(" ");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((int)positions[selector].x + maxwordlength, (int)positions[selector].y); //current position
                Console.Write(arrow);
                Console.ResetColor();


            } while (!exit);

            Console.Clear();
            SelectMenuItem(selector);

        }

        static void SelectMenuItem(int i)
        {
            SelectMenuItem((MenuItems)i);
        }
        static void SelectMenuItem(MenuItems m)
        {
            switch (m)
            {
                case MenuItems.credits:
                    Console.WriteLine("Made by Simon Frühauf for the Minerva C# Quests hosted by Simon Renger.");
                    Console.ReadKey();
                    break;
                case MenuItems.potionseller:
                    Gambling.Game();
                    break;
                case MenuItems.calendar:
                    DrawMonth();
                    break;
                case MenuItems.ai:
                    SuperSmartAI();
                    break;
                case MenuItems.calculator:
                    Calculator();
                    break;
                case MenuItems.age:
                    Age();
                    break;
                case MenuItems.name:
                    Name();
                    break;
                case MenuItems.randomnumber:
                    GenerateRandomNumberBetween();
                    Console.ReadKey();
                    break;
                case MenuItems.guessinggame:
                    GuessingGame(1, 5);
                    break;
                case MenuItems.remindme:
                    RemindMe();
                    break;
                case MenuItems.arraystuff:
                    ArrayStuff();
                    break;
                case MenuItems.warehouse:
                    Warehouse.MainWarehouse();
                    break;
                case MenuItems.chessboard:
                    Vector2D vector = new Vector2D(0, 0, 20, 20);
                    CheckerBoard(20, 20, vector);
                    break;
                case MenuItems.iterator:
                    IteratorTests();
                    break;
                case MenuItems.map:
                    ReadMap();
                    break;                    
                case MenuItems.cave:
                    CaveGenerator CG = new CaveGenerator();
                    int[,] map = CG.CreateCave(30, 30, 70, 2);
                    map = CG.iterateCave(map, 2, 5, 5);
                    //Print2dIntArray(map);
                    CG.WriteMap(map, "test");
                    Console.WriteLine();
                    int[,] error = new int[400, 400];
                    int[,] start = CG.findEmptyCell(map, true);
                    if (start == error)
                    {
                        //error
                    }
                    else
                    {
                        Print2dIntArray(map, true, start.GetLength(0), start.GetLength(1));
                    }
                    //Console.WriteLine(start.GetLength(0) + " " + start.GetLength(1)); 
                    Console.ReadKey();

                    break;
                case MenuItems.rps:
                    RPS RockPaperScissors = new RPS();
                    RockPaperScissors.Play();
                    Console.ReadKey();

                    break;
                case MenuItems.readmap:
                    CaveGenerator CG2 = new CaveGenerator();
                    string[,] thisMap = CG2.ReadMap("test");
                    Print2dArray(thisMap);
                    Console.WriteLine();
                    Console.WriteLine("This was the read map.");
                    Console.WriteLine();
                    Console.ReadKey();

                    break;
                case MenuItems.weirdmenu:
                    BuildMenu bm = new BuildMenu();
                    bool running = true;
                    while (running)
                    {
                        running = BuildMenu.Build();
                    }
                    break;
                case MenuItems.roots:
                    PrintRoots(new int[] { 5, 23, 55, -3, 0, 5, 323, 65, -5 });
                    Console.ReadKey();
                    break;
                case MenuItems.negative:
                    Console.WriteLine(findFirstNegative(new int[] { 5, 23, 55, -3, 0, 5, 323, 65, -5 }));
                    Console.ReadKey();
                    break;
                case MenuItems.spiral:
                    CaveGenerator CG3 = new CaveGenerator();
                    CG3.printSpiral(10);
                    Console.ReadKey();
                    break;
                case MenuItems.storefront:
                    Warehouse WH = new Warehouse();
                    Warehouse.LookAtInventory(WH.Inventory); 
                    Console.ReadKey();
                    break;
                case MenuItems.sorter:
                    int e = Sorter.ReadFile();
                    if (e == 0)
                    {
                        Sorter.Sort(Sorter.items, Sorter.values, true);
                        PrintArray(Sorter.items, Sorter.values);
                    }
                    Console.ReadKey();
                    break;
                case MenuItems.spock:
                    RPSLS RPSPlus = new RPSLS();
                    RPSPlus.Play();
                    Console.ReadKey();
                    break;
                case MenuItems.snake:
                    SnakeMenu snm = new SnakeMenu(true);
                    Console.ReadKey();
                    break;
                case MenuItems.roguelike:
                    Roguelike rgl = new Roguelike();
                    Console.ReadKey();
                    break;
                case MenuItems.cisharpmon:
                    Cisharpmon csm = new Cisharpmon();
                    csm.Play();
                    Console.ReadKey();
                    break;
                case MenuItems.pathfinder:
                    Pathfinder pf = new Pathfinder();
                    Console.ReadLine();
                    int[,] m_map = ReadMap();
                    if (m_map == null)
                    {
                        return;
                    }
                    Console.ReadLine();
                    MapReader MP = new MapReader();
                    Console.Clear();
                    bool[,] b_map = pf.makeWalkable(m_map, new List<int>() { 0 });
                    MP.Draw(b_map);
                    Console.SetCursorPosition(0, 0);
                    Console.Write("S"); 
                    Console.SetCursorPosition(b_map.GetLength(0)-1, b_map.GetLength(1)-1);
                    Console.Write("G");

                    List<Point2D> path = pf.FindPath(new Point2D(0, 0), new Point2D(b_map.GetLength(0)-1, b_map.GetLength(1)-1), b_map);
                    System.Threading.Thread.Sleep(100);
                    if (path == null)   
                    {
                        Console.WriteLine();
                        Console.WriteLine("No Path.");
                    }
                    else
                    {
                        foreach (Point2D point in path)
                        {
                            Console.SetCursorPosition(point.inty, point.intx);
                            Console.Write(".");
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                    Console.Read();
                    break;
                case MenuItems.quit:
                    Environment.Exit(1);
                    break;
                default:
                    break;
            }
            Menu();
        }
        static int[,] ReadMap()
        {
            MapReader MP = new MapReader();
            string map = Read.String("Input the map to read: ");
            int[,] m = MP.Read(map);
            if (m == null)
            {
                Console.WriteLine("Empty file.");
                Console.ReadLine();
                return null;
            }
            //MP.Write(m);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            MP.Draw(m);
            //MP.Blink(m, new Point2D(4, 4), m[4, 4], 2, 20); 
            Console.ReadKey(true);
            return m;
        }
        public static void DrawDivider(string a_divider = divider)
        {
            Console.WriteLine(a_divider);
        }
        public static bool IsPointOnLine(Point2D point, Point2D pointA, Point2D pointB)
        {
            if (pointB.x == 0)
            {
                return point.x == pointA.x;
            }
            if (pointB.y == 0)
            {
                return point.y == pointA.y;
            }
            return (pointA.x - point.x) / pointB.x == (pointA.y - point.y) / pointB.y;
        }
        public static bool IsPointOnLine(Point2D point, Point2D vectorOrigin, Point2D vectorDirection, float deviation = 0)
        {
            float difference = (Distance(point, vectorDirection) + Distance(point, vectorOrigin)) - Distance(vectorOrigin, vectorDirection);
            return difference <= deviation;
        }
        public static float Distance(Point2D point_a, Point2D point_b)
        {
            return (float)Math.Sqrt(((point_a.x - point_b.x) * (point_a.x - point_b.x)) + ((point_a.y - point_b.y) * (point_a.y - point_b.y)));
        }
        public static float Distance(float a_x, float a_y, float b_x, float b_y)
        {
            return (float)Math.Sqrt(((a_x - b_x) * (a_x - b_x)) + ((a_y - b_y) * (a_y - b_y)));
        }

        //// Old Main Menu
        //public static void MainMenu(bool unknownInput = false)
        //{
        //    if (!unknownInput)
        //    {
        //        Console.WriteLine("Welcome " + ((MainMenuOpened == true) ? "back " : "") + "to the Main Menu.");
        //    }
        //    MainMenuOpened = true;
        //    string input = Read.String("Please input your command.");
        //    switch (input)
        //    {
        //        case "potion game":
        //        case "potion seller":
        //        case "potion":
        //        case "p":
        //            Gambling.Game();
        //            return;
        //        case "help":
        //        case "h":
        //            Console.WriteLine("Here a list of available commands. ()Brackets indicate shortcuts. \n> help \n> calendar \n> guessing game \n> super smart (ai) \n> (c)alculator \n> name \n> age \n> random number \n> (e)xit or (q)uit");
        //            break;
        //        case "calendar":
        //            DrawMonth();
        //            break;
        //        case "q":
        //        case "quit":
        //        case "exit":
        //        case "e":
        //            Environment.Exit(1);
        //            break;
        //        case "super smart ai":
        //        case "super smart AI":
        //        case "AI":
        //        case "ai":
        //            SuperSmartAI();
        //            break;
        //        case "calculator":
        //        case "calc":
        //        case "c":
        //            Calculator();
        //            break;
        //        case "name":
        //            Name();
        //            break;
        //        case "age":
        //            Age();
        //            break;
        //        case "random number":
        //            GenerateRandomNumberBetween();
        //            break;
        //        case "guessing game":
        //            GuessingGame(1, 5);
        //            break;
        //        case "remindme":
        //            RemindMe();
        //            break;
        //        case "arraystuff":
        //            ArrayStuff();
        //            break;
        //        case "warehouse":
        //            Warehouse.MainWarehouse();
        //            break;
        //        case "chess":
        //        case "checker":
        //            Vector2D vector = new Vector2D(0, 0, 20, 20);
        //            CheckerBoard(20, 20, vector);
        //            break;
        //        case "iterator":
        //            IteratorTests();
        //            break;
        //        case "cave":
        //            CaveGenerator CG = new CaveGenerator();
        //            int[,] map = CG.CreateCave(15, 10, 70, 2);
        //            map = CG.iterateCave(map, 2, 5, 5);
        //            //Print2dIntArray(map);
        //            CG.WriteMap(map, "test");
        //            Console.WriteLine();
        //            int[,] error = new int[400, 400];
        //            int[,] start = CG.findEmptyCell(map, true);
        //            if (start == error)
        //            {
        //                //error
        //            }
        //            else
        //            {
        //                Print2dIntArray(map, true, start.GetLength(0), start.GetLength(1));
        //            }
        //            Console.WriteLine(start.GetLength(0)+ " " + start.GetLength(1));
        //            break;
        //        case "rps":
        //        case "rock paper scissors":
        //            RPS RockPaperScissors = new RPS();
        //            RockPaperScissors.Play();
        //            break;
        //        case "read map":
        //            CaveGenerator CG2 = new CaveGenerator();
        //            string[,] thisMap = CG2.ReadMap("test");
        //            Print2dArray(thisMap);
        //            Console.WriteLine();
        //            Console.WriteLine("This was the read map.");
        //            Console.WriteLine();
        //            break;
        //        case "weird menu":
        //            BuildMenu bm = new BuildMenu();
        //            bool running = true;
        //            while (running)
        //            {
        //                running = BuildMenu.Build();
        //            }
        //            break;
        //        case "roots":
        //            PrintRoots(new int[] { 5, 23, 55, -3, 0, 5, 323, 65, -5 });
        //            break;
        //        case "negative":
        //            Console.WriteLine(findFirstNegative(new int[] { 5, 23, 55, -3, 0, 5, 323, 65, -5 }));
        //            break;
        //        case "printspiral":
        //            CaveGenerator CG3 = new CaveGenerator();
        //            CG3.printSpiral(10);
        //            break;
        //        case "storefront":
        //            Warehouse WH = new Warehouse();
        //            Warehouse.LookAtInventory(WH.Inventory);
        //            break;
        //        case "sorter":
        //            int e = Sorter.ReadFile();
        //            if (e == 0)
        //            {
        //                Sorter.Sort(Sorter.items, Sorter.values, true);
        //                PrintArray(Sorter.items, Sorter.values);
        //            }
        //            break;
        //        case "rpsls":
        //        case "spock":
        //            RPSLS RPSPlus = new RPSLS();
        //            RPSPlus.Play();
        //            break;
        //        case "snake":
        //            SnakeMenu snm = new SnakeMenu(true);

        //            break;
        //        case "roguelike":
        //            Roguelike rgl = new Roguelike();
        //            break;
        //        case "pokemon":
        //            Cisharpmon csm = new Cisharpmon();
        //            csm.Play();
        //            break;
        //        case "mainmenu":
        //            Menu();
        //            break;
        //        default:
        //            Console.Write("Unknown Input. ");
        //            MainMenu(true);
        //            break;
        //    }
        //    MainMenu();
        //}

        static void PrintRoots(List<int> list)
        {
            foreach (int number in list)
            {
                if (number < 0)
                {
                    continue;
                }
                Console.WriteLine(Math.Sqrt(number));
                
            }
        }
        static void PrintRoots(int[] array)
        {
            foreach (int number in array)
            {
                if (number < 0)
                {
                    continue;
                }
                Console.WriteLine(Math.Sqrt(number));
            }
        }

        static int findFirstNegative(int[] array)
        {
            int i = 0;
            foreach (int number in array)
            {
                i++;
                if (number < 0 )
                {
                    return i; 
                    // either return i for the index,
                    //or number for the number itself
                }
            }
            return -1;
        }
        int returnIndex(string lookfor, dynamic array)
        {
            int i = 0;
            foreach (string item in array)
            {
                if (item == lookfor)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        int returnIndex(string lookfor, List<string> list)
        {
            foreach (string item in list)
            {
                if (item == lookfor)
                {
                    return list.IndexOf(item);
                }
            }
            return -1;
        }
        struct Tile { int i; }
        static void CheckerBoard(int x_size = 8, int y_size = 8, bool diagonal_line = false)
        {
            if (!diagonal_line)
            {
                int counter = 0;
                for (int row = 0; row < x_size; row++)
                {
                    for (int col = 0; col < y_size; col++)
                    {
                        if (counter >= x_size) //check if we wrote a whole row
                        {
                            Console.WriteLine();
                            counter = 0;
                        }
                        //check if the amount of tiles is odd or even, on this row, 
                        // offset by the row itself (so we have alternating 
                        //colors even on odd amount of tiles)
                        if ((row + col) % 2 == 0)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write("O");
                        }
                        counter++;
                    }
                }
            }
            else
            {
                int counter = 0;
                for (int row = 0; row < x_size; row++)
                {
                    for (int col = 0; col < y_size; col++)
                    {
                        if (counter >= x_size) //check if we wrote a whole row
                        {
                            Console.WriteLine();
                            counter = 0;
                        }
                        if (row == col)
                        {
                            Console.Write("I");
                        }
                        else
                        {
                            if ((row + col) % 2 == 0)
                            {
                                Console.Write("X");
                            }
                            else
                            {
                                Console.Write("O");
                            }
                        }
                        counter++;
                    }
                }
            }
            Console.WriteLine();
        }
        static void CheckerBoard(int x_size, int y_size, Vector2D line)
        {
            int counter = 0;
            for (int row = 0; row < x_size; row++)
            {
                for (int col = 0; col < y_size; col++)
                {
                    if (counter >= x_size) //check if we wrote a whole row
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    Point2D point = new Point2D(row, col);
                    if (!IsPointOnLine(point, new Point2D(0f, 0f), new Point2D(5f, 5f))) //!IsPointOnLine(point, line.origin, line.direction, 0.05f)
                    {
                        if ((row + col) % 2 == 0)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write("O");
                        }
                    }
                    else
                    {
                        Console.Write("*");
                    }
                    counter++;
                }
            }
            Console.WriteLine();
            Console.ReadKey();
        }
        static public void PrintArray(dynamic array)
        {
            Console.WriteLine();
            foreach (var item in array)
            {
                Console.WriteLine(item.ToString());
            }
        }

        static public void PrintArray(dynamic array, dynamic array_b)
        {
            Console.WriteLine();
            int l = array.Length;
            int l_b = array_b.Length;

            int length = Math.Max(l, l_b);
            for (int i = 0; i < length; i++)
            {
                if (array[i] != null)
                {
                    Console.Write(array[i]);
                    Console.Write(" --> ");
                    if (array_b != null)
                    {
                        Console.Write(array_b[i]);
                    }
                    else
                    {
                        Console.Write("null");
                    }
                    Console.WriteLine();
                }
            }
        }
        static public void Print2dIntArray(int[,] array, bool special = false, int x = 0, int y = 0)
        {
            Console.WriteLine();
            int x_size = array.GetLength(0);
            int y_size = array.GetLength(1);
            int counter = 0;
            for (int col = 0; col < y_size; col++)
            {
                for (int row = 0; row < x_size; row++)
                {
                    if (counter >= x_size) //check if we wrote a whole row
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    counter++;
                    if (row == x && col == y && special)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("@"); 
                        Console.ResetColor();
                    }
                    else
                    {
                        switch (array[row, col])
                        {
                            case 0:
                                Console.Write("X");
                                break;
                            case 1:
                                Console.Write(" ");
                                break;
                            default:
                                break;
                        }
                    }
                    //Console.Write(array[row, col] == 1 ? " " : "X");
                }
            }
        }
        static public void Print2dArray(dynamic[,] array)
        {
            Console.WriteLine(); Console.WriteLine();
            int x_size = array.GetLength(0);
            int y_size = array.GetLength(1);
            int counter = 0;
            for (int col = 0; col < y_size; col++)
            {
                for (int row = 0; row < x_size; row++)
                {
                    if (counter >= x_size) //check if we wrote a whole row
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    counter++;
                    Console.Write((string)array[row, col]);
                }
            }
        }

        public static void IteratorTests()
        {
            foreach (int number in EvenNumbers(25, 40))
            {
                Console.Write(number.ToString() + " ");
            }
            //prints: 26 28 30 32 34 36 38 40
        }
        IEnumerable<float> IteratorTestNumbers()
        {
            yield return 1f;
            yield return 2f;
            yield return 5f;
        }
        static IEnumerable<float> EvenNumbers(float start, float end)
        {
            //yields all even numbers
            for (float number = start; number <= end; number++)
            {
                if (number % 2 == 0)
                {
                    yield return number;
                }
            }
        }
        public static void ArrayStuff()
        {
            List<int> intlist = new List<int> { };
            foreach (int item in intlist)
            {
                Console.WriteLine(item);
            }
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
                if (counter % 4 == 0)
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
            Console.ReadKey();  // remember doing in case the variable is 
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
            Console.ReadKey();
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
            Console.ReadKey();
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
            Console.Write("Hello ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(name);
            Console.ResetColor();
            Console.WriteLine("!");
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
                    keyinfo = Console.ReadKey(true).Key;
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
                            if (itemIndex < maxItems - 1)
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
            Program.Menu();
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
                while (charCount != itemWidth - 1)
                {
                    if (charCount >= itemWidth)
                    {
                        Console.WriteLine();
                        break;
                    }
                    switch (flipFlop)
                    {
                        case true:
                            lable = " " + lable;
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
            Console.SetCursorPosition(0, Console.CursorTop - amount + 1);
            for (int i = 0; i < amount; i++)
            {
                Console.Write("                 ", 0, 80);
            }
            Console.WriteLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }

}