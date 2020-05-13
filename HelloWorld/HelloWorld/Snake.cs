using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;



namespace HelloNamespace
{   //currently works partially, snake can move, but body only follows in relation to space, not to time
    //documentation : https://imgur.com/a/h5s702i
    class Snake
    {
        int dx, dy;
        int speed = 500;
        bool running = true;
        ConsoleKeyInfo consoleKey;
        Dictionary<int, Position> snake = new Dictionary<int, Position>();

        List<ConsoleColor> snakeColors = new List<ConsoleColor>() {
        ConsoleColor.Green,
        ConsoleColor.Yellow,
        ConsoleColor.Blue,
        ConsoleColor.Magenta,
        ConsoleColor.Cyan,
        ConsoleColor.Red
        };

        int colorIndex = 0;
        

        struct Bodypart
        {
            public int index;
            public Position pos;

            public Bodypart(int index_m, Position pos_m)
            {
                index = index_m;
                pos = pos_m; 
            }
        }
        char snakechar = 'O';
        char borderchar = 'X';
        char foodchar = 'ö';
        bool trail =false;
        struct GameSize
        {
            public int x;
            public int y;
            public GameSize(int x_m, int y_m)
            {
                x = x_m;
                y = y_m;
            }
        }
        GameSize gm = new GameSize(20, 20);
        struct Position
        {
            public int x;
            public int y;
            public Position(int x_m, int y_m)
            {
                x = x_m;
                y = y_m;
            }
        }

        Position food;
        enum Direction
        {
            Up, Right, Down, Left
        }

        public void ChangeCursor(int x, int y)
        {
            Console.SetCursorPosition(x,y);
            //ResetScreen(30, 15);
        }

        void ResetScreen(int x, int y)
        {
            Console.Clear();
            Console.SetWindowSize(x, y); //rows by columns
        }


        public void Play(bool rainbow = false, bool useFullscreen = false)
        {
            food = nextFoodPos();
            snake.Add(0, useFullscreen ? new Position(Console.WindowWidth/2, Console.WindowHeight/2) : new Position (gm.x / 2, gm.y / 2));
            Console.Clear();
            for (int i = 0; i <= (useFullscreen ? Console.WindowWidth : gm.x); i++)
            {
                for (int j = 0; j <= (useFullscreen ? Console.WindowHeight : gm.y); j++)
                {
                    if (j <= 2 | i >= (useFullscreen ? (Console.WindowWidth - Console.WindowWidth/10)  : gm.x) | i <= 1 | j >= (useFullscreen ? (Console.WindowHeight - Console.WindowHeight / 10) : gm.y))
                    {
                        if (i == Console.WindowWidth | j == Console.WindowHeight)
                        {

                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(i, j);
                            Console.Write(borderchar);
                            Console.ResetColor();

                        }

                    }
                }
            }
            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Arrows change direction, 'esc' to quit. Use '+' and '-' to adjust speed.");

            do // until we press escape
            {
                
                Console.SetCursorPosition(snake[0].x, snake[0].y);

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.J:
                            //AddBodypart();
                            break;
                        case ConsoleKey.OemPlus:
                            if (speed > 200)
                            {
                                speed -= 100;
                            }
                            break;
                        case ConsoleKey.OemMinus:
                            if (speed < 700)
                            {
                                speed += 100;
                            }
                            break;
                        case ConsoleKey.C:
                            Console.Clear();
                            break;
                        case ConsoleKey.UpArrow: //UP
                            dx = 0;
                            dy = -1;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            dx = 0;
                            dy = 1;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            dx = -1;
                            dy = 0;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            dx = 1;
                            dy = 0;
                            break;
                        case ConsoleKey.Escape: //END
                            running = false;
                            Console.Clear();
                            ChangeCursor(0, 0);
                            break;
                    }
                }
                Position temp = new Position(0,0);
                Position last = new Position(0, 0);
                for (int i = 0; i < snake.Count; i++)
                {

                    Console.SetCursorPosition(snake[i].x, snake[i].y);

                    if (!trail)
                        Console.Write(' ');

                    temp = snake[i];
                    if (i == 0)
                    {
                        last = snake[i];
                        Console.ForegroundColor = snakeColors[colorIndex];
                        snake[i] = new Position(temp.x + dx, temp.y + dy);
                        temp = snake[i];
                        if (snake[i].x > (useFullscreen? (Console.WindowWidth - Console.WindowWidth / 10) : gm.x) - 1)
                            snake[i] = new Position(3, temp.y + dy);
                        if (snake[i].x < 2)
                            snake[i] = new Position((useFullscreen ? (Console.WindowWidth - Console.WindowWidth / 10) : gm.x)- 1, temp.y + dy);
                        temp = snake[i];

                        if (snake[i].y > (useFullscreen ? (Console.WindowHeight - Console.WindowHeight / 10) : gm.y) -1)
                            snake[i] = new Position(temp.x + dx, 3);

                        if (snake[i].y < 3)
                            snake[i] = new Position(temp.x + dx, (useFullscreen ? (Console.WindowHeight - Console.WindowHeight / 10) : gm.y) - 1);
                        //keep snake in the square

                        // write the character in the new position
                        Console.SetCursorPosition(snake[i].x, snake[i].y);
                        Console.Write(snakechar);
                        Console.ResetColor();

                    }
                    else if (i !=0)
                    {
                        if (rainbow)
                        {
                            Console.ForegroundColor = snakeColors[colorIndex];
                        }
                        Position last_t = snake[i];
                        snake[i] = new Position(last.x, last.y);
                        if (i == 1)
                        {
                            //???
                        }
                        Console.SetCursorPosition(snake[i].x, snake[i].y);
                        Console.Write(snakechar);
                        last = last_t;
                        if (rainbow)
                        {
                            Console.ResetColor();

                        }
                    }
                    

                }

                //check for intersections

                for (int i = 0; i < snake.Count; i++)
                {
                    if (i != 0)
                    {
                        if (snake[i].x == snake[0].x && snake[i].y == snake[0].y)
                        {
                            running = false;
                            break;
                        }
                    }
                    else if (i == 0 && snake[i].x == food.x && snake[i].y == food.y)
                    {
                        //eat
                        food = nextFoodPos();
                        if (rainbow)
                        {
                            colorIndex = (colorIndex + 1) % snakeColors.Count();
                        }
                        AddBodypart();
                    }
                }

                // draw food

                if (food.x != 0 && food.y != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(food.x, food.y);
                    Console.Write(foodchar);
                    Console.ResetColor();

                }

                Console.SetCursorPosition(useFullscreen ? Console.WindowWidth-1 : gm.x, useFullscreen ? Console.WindowHeight-1 : gm.y);
                // pause to allow eyeballs to keep up
                Thread.Sleep(useFullscreen ? speed/8 : speed);

            } while (running);

            //Game over
            PrintScore();
        }

        Position nextFoodPos()
        {
            List<Position> all = new List<Position>();

            for (int i = 2; i < gm.x-1; i++)
            {
                for (int j = 4; j < gm.y-1; j++)
                {
                    if (!snake.ContainsValue(new Position(i, j)))
                    {
                        all.Add(new Position(i, j));
                    }
                    
                }
            }
            
            Random rnd = new Random();

            return all[rnd.Next(0, all.Count-1)];
        }
        void AddBodypart()
        {
            snake.Add(snake.Count, new Position(snake[snake.Count - 1].x - dx, snake[snake.Count - 1].y - dy));
        }

        void PrintScore()
        {
            int score = snake.Count();
            string text = "Game over. You have a score of " + score + "!";
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.Write(text);
            Console.WriteLine("Please enter your name!");

            string name = Console.ReadLine();
            if (name == "" && name != "b" && name != "back" && name != "q" && name != "quit")
            {
                Console.WriteLine("Not saving the score.");
                goto skip;
            }
            else if (name == "back" || name == "b" || name == "q" || name == "quit")
            {
                return;
            }
            Console.WriteLine("Saved your score, " + name + ".");
            List<string> scores = new List<string>();
            if (new FileInfo("files\\snake.txt").Length != 0)
            {
                //file not empty
                scores = new List<string>(File.ReadAllLines("files\\snake.txt"));
                
            }
            scores.Add(name + "-" + score);
            File.WriteAllLines("files\\snake.txt", scores);
        skip:
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
        }
    }
    class SnakeMenu
    {
        List<Tuple<string, int>> Highscores;
        private int index = 0;
        struct firstIndex
        {
            public int x;
            public int y;
            public firstIndex(int xpos, int ypos)
            {
                x = xpos;
                y = ypos;
            }
        }
        string arrow = "<--";
        public static string highscoreFile = "files\\snake.txt";

        public SnakeMenu(bool firstrun)
        {
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
            }
            if (!File.Exists(highscoreFile))
            {
                File.Create(highscoreFile);
            }
            if(firstrun)
                Console.WriteLine("-------------------\nWelcome to the snake menu. Select your option: ");
            firstIndex firstIndex_m = new firstIndex(0, 0);
            Console.WriteLine("- play     ");
            Console.WriteLine("- higscores");
            Console.WriteLine("- credits  ");
            Console.Write("- quit     ");
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 3);
            firstIndex_m = new firstIndex(Console.CursorLeft, Console.CursorTop);
            
            index = 0;
            backtomenu:
            Console.SetCursorPosition(firstIndex_m.x, firstIndex_m.y + index);
            Console.Write(arrow);

            bool exit = false;
            do
            {
                
                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                    case ConsoleKey.UpArrow:
                        index = ((index - 1) % 4 + 4) % 4; // keeps it between 0 and 3 and not -3 and 3
                        break;
                    case ConsoleKey.DownArrow:
                        index = (index + 1) % 4;
                        break;
                    default:
                        break;
                }
                //delete current arrow
                Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                Console.Write("   ");
                //redraw arrow at index
                Console.SetCursorPosition(firstIndex_m.x, firstIndex_m.y + index);
                Console.Write(arrow);

            } while (!exit);

            switch (index)
            {
                case 0: // play
                    this.Play(true, false);
                    break;
                case 1: // highscore
                    Score(firstIndex_m);
                    Console.SetCursorPosition(firstIndex_m.x, firstIndex_m.y + index);
                    Console.Write("   ");
                    goto backtomenu;
                case 2: // credits
                    Credits(firstIndex_m);
                    Console.SetCursorPosition(firstIndex_m.x, firstIndex_m.y + index);
                    Console.Write("   ");
                    goto backtomenu;
                    
                case 3: // quit
                    Quit();
                    break;
                default:
                    break;
            }

        }
        public void Play(bool rainbow, bool fullscreen)
        {
            Snake snek = new Snake();
            snek.Play(rainbow, fullscreen);
        }
        void Score(firstIndex x)
        {
            Console.SetCursorPosition(0, x.y + 3);
            Console.WriteLine();
            Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");

            if (File.Exists(highscoreFile))
            {
                Console.SetCursorPosition(0, x.y + 4);
                List<string> highscores = new List<string>(File.ReadAllLines(highscoreFile));

                List<Tuple<int, string>> hscore = new List<Tuple<int, string>>();
                foreach (var item in highscores)
                {
                    List<string> v = new List<string>(item.Split('-'));
                    hscore.Add(new Tuple<int, string>(Convert.ToInt32(v[1]), v[0]));
                }
                hscore.Sort();
                hscore.Reverse(); //otherwise 0 is highest
                int maxscores = 10;
                int counter = 0;
                foreach (Tuple<int,string> score in hscore)
                {   
                    counter++;
                    Console.WriteLine(score.Item1 + " - " + score.Item2);
                    if (counter >= maxscores)
                    {
                        break;
                    }
                }
            }

            else
            {
                Console.WriteLine("No scores found.");
            }
        }

        void Credits(firstIndex x)
        {
            Console.SetCursorPosition(0, x.y + 3);
            Console.WriteLine();
            Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
            Console.WriteLine("----- Created by Simon Frühauf.");

            for (int i = 0; i < 9; i++)
            {
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                Console.WriteLine();


            }
        }

        void Quit()
        {
            Console.Clear();
            Console.WriteLine();

        }

    }
}
