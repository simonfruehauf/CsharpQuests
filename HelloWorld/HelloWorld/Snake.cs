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
        int foodx, foody, dx, dy;
        int speed = 500;
        bool running = true;
        ConsoleKeyInfo consoleKey;
        char snakechar = 'O';
        char border = '*';
        bool trail;
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
            List<Position> snake = new List<Position>();
            snake.Add(new Position(gm.x / 2, gm.y / 2));
            Console.Clear();
            for (int i = 0; i <= gm.x; i++)
            {
                for (int j = 0; j <= gm.y; j++)
                {
                    if (j <= 2 | i >= gm.x | i <= 1 | j >= gm.y)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(border);
                    }
                }
            }
            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Arrows move up/down/right/left. 'c' to clear  'esc' to quit.");

            do // until we press escape
            {
                
                //Console.SetCursorPosition(snake[0].x, snake[0].y);

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.J:
                            snake.Add(new Position(snake.Last<Position>().x - dx, snake.Last<Position>().y - dy));
                            break;
                        case ConsoleKey.T:
                            trail = true;
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

                


                for (int i = 0; i < snake.Count; i++)
                {
                    Console.SetCursorPosition(snake[i].x, snake[i].y);
                    
                    if (trail == false)
                        Console.Write(' ');

                    Position temp = snake[i];
                    snake[i] = new Position(temp.x + dx, temp.y + dy);
                    if (snake[i].x > gm.x-1)
                        snake[i] = new Position(3, temp.y + dy);
                    if (snake[i].x < 2)
                        snake[i] = new Position(gm.x-1, temp.y + dy);
                    temp = snake[i];
                    if (snake[i].y > gm.y-1)
                        snake[i] = new Position(temp.x, 2); 
                    if (snake[i].y < 3)
                        snake[i] = new Position(temp.x, gm.y-1);
                    //keep snake in the square

                    // write the character in the new position
                    Console.SetCursorPosition(snake[i].x, snake[i].y);
                    Console.Write(snakechar);
                }

                //draw borders

                
                
                // pause to allow eyeballs to keep up
                Thread.Sleep(speed);

            } while (running);
            Console.Clear();
            ChangeCursor(0, 0);
        }
    }
}
