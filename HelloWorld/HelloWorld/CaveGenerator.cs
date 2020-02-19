using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HelloWorld
{
    
    

    public class CaveGenerator
    {
        Random rnd = new Random();
        public int[,] Map;
        bool exceeded = false;
        int Neighbours = 4;
        public int[,] CreateCave(int size_x, int size_y, int probability, int iterations = 300)
        {
            Map = new int[size_x, size_y];

            //go through each cell and use the specified probability to determine if it's open
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (rnd.Next(0, 100) < probability)
                    {
                        Map[x, y] = 1;
                    }
                }
            }

            //pick random cells
            for (int x = 0; x <= iterations; x++)
            {
                int rX = rnd.Next(0, Map.GetLength(0));
                int rY = rnd.Next(0, Map.GetLength(1));

                if (exceeded == true)
                {
                    if (examineNeighbours(rX, rY) > Neighbours)
                    {
                        Map[rX, rY] = 1;
                    }
                    else
                    {
                        Map[rX, rY] = 0;
                    }
                }
                else
                {
                    if (examineNeighbours(rX, rY) > Neighbours)
                    {
                        Map[rX, rY] = 0;
                    }
                    else
                    {
                        Map[rX, rY] = 1;
                    }
                }
            }

                return Map;
        }

        
        int examineNeighbours(int xVal, int yVal)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (checkCell(xVal + x, yVal + y) == true)
                        count += 1;
                }
            }

            return count;
        }
        Boolean checkCell(int x, int y)
        {
            if (x >= 0 & x < Map.GetLength(0) &
                y >= 0 & y < Map.GetLength(1))
            {
                if (Map[x, y] > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
