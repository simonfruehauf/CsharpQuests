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

        public int[,] CreateCave(int size_x, int size_y, int probability, int iterations = 300, bool old = false, float deathLimit = 5, float birthLimit = 5)
        {
            Map = new int[size_x, size_y];

            //go through each cell and use the specified probability to determine if it's open
            //for (int x = 0; x < Map.GetLength(0); x++)
            //{
            //    for (int y = 0; y < Map.GetLength(1); y++)
            //    {
            //        if (rnd.Next(0, 100) < probability)
            //        {
            //            Map[x, y] = 1;
            //        }
            //    }
            //}
            int banana = 0;
            int apple = 0;
            Console.WriteLine(Map.GetLength(0));
            Console.WriteLine(Map.GetLength(1));


            // Somehow, the outer loop completes before the inner loop, multiple times
            for (banana = 0; banana < size_x; banana++)
            {
                for (apple = 0; apple < size_y; apple++)
                {
                    if (apple < 1)
                    {
                        Map[banana, apple] = 1;
                    }
                    else
                    {
                        Map[banana, apple] = 0;
                    }
                }
            }
            //return Map;


            for (int i = 0; i <= iterations; i++)
            {
                //Map = doSimulationStep(Map, deathLimit, birthLimit);
            }
           

            return Map;
        }

        public int[,] doSimulationStep(int[,] map, float deathLimit = 5, float birthLimit = 5)
        {
            int[,] t_map = map;
            Program.Print2dIntArray(t_map);
            for (int x = 0; x < t_map.GetLength(0); x++)
            {
                for (int y = 0; y < t_map.GetLength(1); y++)
                {
                    if (examineNeighbours(map, x, y) < deathLimit)
                    {
                        t_map[x, y] = 0;
                    }

                    else if (examineNeighbours(map, x, y) > birthLimit)
                    {
                        t_map[x, y] = 1;
                    }

                }
            }

            return t_map;

        }

        int examineNeighbours(int[,] map, int xVal, int yVal)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (checkCell(map, xVal + x, yVal + y) == true)
                        count += 1;
                }
            }

            return count;
        }


        Boolean checkCell(int[,] map, int x, int y)
        {
            if (x >= 0 & x < map.GetLength(0) &
                y >= 0 & y < map.GetLength(1))
            {
                if (map[x, y] > 0)
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
