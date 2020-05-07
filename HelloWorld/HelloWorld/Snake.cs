using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloNamespace
{
    class Snake
    {
        int foodx, foody;
        float speed = 100f;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
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
        List<Position> snake;
        enum Direction
        {
            Up, Right, Down, Left
        }

        public void ChangeCursor(int x, int y)
        {
            Console.SetCursorPosition(x,y);
            ResetScreen(30, 15);
        }

        void ResetScreen(int x, int y)
        {
            Console.Clear();
            Console.SetWindowSize(x, y); //rows by columns
        }


         void Play()
        {
            wait:
            sw.Start();
            if (sw.ElapsedMilliseconds >= speed)
            {
                sw.Reset();
                //update everything
            }
            goto wait;
            
        }

        

    }
}
