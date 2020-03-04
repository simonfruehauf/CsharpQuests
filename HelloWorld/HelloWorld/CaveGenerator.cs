using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace HelloWorld
{

    public class CaveGenerator
    {
        public const string folder = "files";
        Random rnd = new Random();
        public int[,] Map;
        public dynamic ReadMap(string filename, bool asInt = false)
        {
          
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Console.WriteLine("Missing Directory, now created. Unable to read files.");
            }
            string line = File.ReadLines(folder + "\\" + filename + ".txt").First();
            string[] length = line.Split('.');

            string[,] t_map_string = new string[length.Length, CountLinesInFile(folder + "\\" + filename + ".txt")];
            int[,] t_map = new int[length.Length, CountLinesInFile(folder + "\\" + filename + ".txt")];
            for (int i = 0; i < File.ReadLines(folder + "\\" + filename + ".txt").Count(); i++)
                {
                    line = File.ReadLines(folder + "\\" + filename + ".txt").Skip(i).Take(1).First();
                    int c = 0;
                    foreach (string item in line.Split('.'))
                    {
                    if (asInt)
                    {
                        try
                        {
                            t_map[c, i] = Convert.ToInt32(item);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Error. Could not parse file.");
                            return new object[0, 0];
                        }
                        
                    }
                    else
                    {
                        t_map_string[c, i] = item;
                    }
                    c++;
                }


            }

            if (asInt)
            {
                return t_map;
            }
            else
            {
                return t_map_string;
            }
           


        }
        static long CountLinesInFile(string f)
        {
            long count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        }
        public void WriteMap(int[,] map, string filename)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(folder + "\\" + filename + ".txt"))
            {
                var t_file = File.Create(folder + "\\" + filename + ".txt");
                t_file.Close();
            }
                using (TextWriter tw = new StreamWriter(folder + "\\" + filename + ".txt"))
            {
                int x_size = map.GetLength(0);
                int y_size = map.GetLength(1);
                int counter = 0;
                for (int col = 0; col < y_size; col++)
                {
                    for (int row = 0; row < x_size; row++)
                    {
                        if (counter >= x_size) //check if we wrote a whole row
                        {
                            tw.Write("\n");
                            counter = 0;
                        }
                        counter++;
                        tw.Write(map[row, col]);
                        if (counter < x_size)
                        {
                            tw.Write(".");
                        }
                    }
                }
                
            }
        }
        public int[,] CreateCave(int size_x, int size_y, int probability, int iterations = 300, bool old = false, float deathLimit = 5, float birthLimit =7)
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

            for (int i = 0; i <= iterations; i++)
            {
                Map = doSimulationStep(Map, deathLimit, birthLimit);
            }
           
            
            return blowUpArray(Map);
        }

        public int[,] iterateCave(int[,] map, int iterations, float deathLimit = 5, float birthLimit = 7)
        {
            int[,] t_map = map;
            for (int i = 1; i <= iterations; i++)
            {
                t_map = doSimulationStep(map, deathLimit, birthLimit);
            }
            return t_map;
        }
        public int[,] doSimulationStep(int[,] map, float deathLimit = 5, float birthLimit = 5)
        {
            int[,] t_map = map;
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

        int[,] blowUpArray(int[,] m_array, int size = 2)
        {
            int sizeX = m_array.GetLength(0) * size;
            int sizeY = m_array.GetLength(1) * size;
            int[,] t_array = new int[sizeX, sizeY];
            for (int i = 0; i < m_array.GetLength(0); i++)
            {
                for (int j = 0; j < m_array.GetLength(1); j++)
                {
                        t_array[i * size, j * size] = m_array[i, j];

                    for (int x = 1; x < size; x++)
                    {
                        t_array[i * size + x, j * size] = m_array[i, j];
                        t_array[i * size, j * size + x] = m_array[i, j];
                        t_array[i * size + x, j * size + x] = m_array[i, j];
                    }
                    
                }
            }
            return t_array;
        }
    }
}
