using System;
using System.Collections.Generic;
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


        public void Play()
        {
            food = nextFoodPos();
            snake.Add(0, new Position (gm.x / 2, gm.y / 2));
            Console.Clear();
            for (int i = 0; i <= gm.x; i++)
            {
                for (int j = 0; j <= gm.y; j++)
                {
                    if (j <= 2 | i >= gm.x | i <= 1 | j >= gm.y)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(borderchar);
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        snake[i] = new Position(temp.x + dx, temp.y + dy);
                        temp = snake[i];
                        if (snake[i].x > gm.x - 1)
                            snake[i] = new Position(3, temp.y + dy);
                        if (snake[i].x < 2)
                            snake[i] = new Position(gm.x - 1, temp.y + dy);
                        temp = snake[i];

                        if (snake[i].y > gm.y-1)
                            snake[i] = new Position(temp.x + dx, 3);

                        if (snake[i].y < 3)
                            snake[i] = new Position(temp.x + dx, gm.y - 1);
                        //keep snake in the square

                        // write the character in the new position
                        Console.SetCursorPosition(snake[i].x, snake[i].y);
                        Console.Write(snakechar);
                        Console.ResetColor();

                    }
                    else if (i !=0)
                    {
                        Position last_t = snake[i];
                        snake[i] = new Position(last.x, last.y);
                        if (i == 1)
                        {
                            //???
                        }
                        Console.SetCursorPosition(snake[i].x, snake[i].y);
                        Console.Write(snakechar);
                        last = last_t;
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

                Console.SetCursorPosition(gm.x, gm.y);
                // pause to allow eyeballs to keep up
                Thread.Sleep(speed);

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
            string text = "Game over. You have a score of " + snake.Count() + "!";

            Console.SetCursorPosition(gm.x - (text.Length / 2), gm.y / 2);
            Console.Write(text);

            Console.SetCursorPosition(0, gm.y + 2);
        }
    }
}
