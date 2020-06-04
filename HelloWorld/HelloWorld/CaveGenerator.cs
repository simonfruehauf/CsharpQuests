using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HelloNamespace
{

    public class CaveGenerator
    {
        public const string folder = "files";
        Random rnd = new Random();
        public int[,] Map;
        public Tile[,] TileMap;
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
        public int[,] CreateCave(int size_x, int size_y, int probability, int iterations = 300, bool old = false, float deathLimit = 5, float birthLimit = 7, bool showprogress = false)
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
                if (showprogress)
                {
                    Console.Clear();
                    Program.Print2dIntArray(blowUpArray(Map));
                    System.Threading.Thread.Sleep(100);
                }
            }


            return blowUpArray(Map);
        }
        public Tile[,] CreateCave(Program.Point2D size, int probability, int iterations = 300)
        {

            TileMap = new Tile[(int)size.x, (int)size.y];

            //go through each cell and use the specified probability to determine if it's open
            for (int x = 0; x < TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < TileMap.GetLength(1); y++)
                {
                    if (rnd.Next(0, 100) < probability)
                    {
                        TileMap[x, y] = new Tile('X', new Program.Point2D(x, y), Tile.TileType.Wall);
                    }
                    else
                    {
                        TileMap[x, y] = new Tile(' ', new Program.Point2D(x, y), Tile.TileType.Floor);
                    }

                }
            }

            for (int i = 0; i <= iterations; i++)
            {
                TileMap = doSimulationStep(TileMap, 5, 6);
            }

            return TileMap;
        }
        public int[,] iterateCave(int[,] map, int iterations, float deathLimit = 5, float birthLimit = 7, bool showSteps = false)
        {
            int[,] t_map = map;
            for (int i = 1; i <= iterations; i++)
            {
                t_map = doSimulationStep(map, deathLimit, birthLimit);
                if (showSteps)
                {
                    Console.Clear();
                    Program.Print2dIntArray(blowUpArray(Map));
                    System.Threading.Thread.Sleep(100);
                }
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
        public Tile[,] doSimulationStep(Tile[,] map, float deathLimit = 5, float birthLimit = 7)
        {
            Tile[,] t_map = map;
            for (int x = 0; x < t_map.GetLength(0); x++)
            {
                for (int y = 0; y < t_map.GetLength(1); y++)
                {
                    if (examineNeighbours(map, x, y) < deathLimit)
                    {
                        t_map[x, y].TileDie(); //dies
                    }

                    else if (examineNeighbours(map, x, y) > birthLimit)
                    {
                        t_map[x, y].TileBorn(); //gets born
                    }

                }
            }

            return t_map;

        }

        public Tile[,] invertMap(Tile[,] map)
        {
            foreach (Tile item in map)
            {
                if (item.type == Tile.TileType.Floor)
                {
                    item.type = Tile.TileType.Wall;
                    item.symbol = 'X';
                }
                else if (item.type == Tile.TileType.Wall)
                {
                    item.type = Tile.TileType.Floor;
                    item.symbol = ' ';
                }
            }

            return map;
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
        int examineNeighbours(Tile[,] map, int xVal, int yVal, Tile.TileType t = Tile.TileType.Wall)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (checkCell(map, xVal + x, yVal + y, t) == true)
                        count += 1;
                }
            }

            return count;
        }

        public Tile[,] addLakes(Tile[,] map, int amount, int size = 4)
        {

            List<Tile> empties = new List<Tile>();
            foreach (Tile item in map) //get all empty tiles
            {
                if (item.type == Tile.TileType.Floor || item.type == Tile.TileType.Null)
                {
                    empties.Add(item);
                }
            }

            Random rnd = new Random();

            for (int i = 0; i < amount; i++) //add amount of lake seeds
            {
                int rtile = rnd.Next(0, empties.Count());
                map[empties[rtile].position.intx, empties[rtile].position.inty] = new Tile('~', new Program.Point2D(empties[rtile].position.intx, empties[rtile].position.inty), Tile.TileType.Water, ConsoleColor.Cyan);
                //Program.PrintTileArray(map);
            }
            List<Tile> watertiles = new List<Tile>();
            for (int i = 0; i < size; i++)
            {
                Tile[,] t_map = map;
                foreach (Tile tile in t_map)
                {
                    if (tile.type == Tile.TileType.Water)
                    {
                        watertiles.Add(tile);
                    }
                }
                foreach (Tile w in watertiles)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (w.position.intx + x > map.GetLength(0) - 1 || w.position.inty + y > map.GetLength(1) - 1 || w.position.intx + x < 0 || w.position.inty + y < 0)
                            {
                                //out of bounds
                                //map[tile.position.intx + x, tile.position.inty + y] = new Tile('-', new Program.Point2D(tile.position.intx + x, tile.position.inty + y), Tile.TileType.Null, ConsoleColor.White);
                                //Console.Beep();
                            }
                            else
                            { 
                                if (map[w.position.intx + x, w.position.inty + y].type != Tile.TileType.Wall )
                                //{
                                    map[w.position.intx + x, w.position.inty + y] = new Tile('~', new Program.Point2D(w.position.intx + x, w.position.inty + y), Tile.TileType.Water, ConsoleColor.Cyan);

                                //}
                            }
                        }
                    }
                }
                t_map = map;
            }
            map = SmoothWater(map, 3);

            return map;
        }

        Tile[,] SmoothWater(Tile[,] map, int iterations, int deathlimit = 5)
        {
            Tile[,] t_map = map;
            for (int i = 0; i < iterations; i++)
            {
                foreach (Tile tile in t_map)
                {
                    if (tile.type == Tile.TileType.Water)
                    {
                        int n = examineNeighbours(map, tile.position.intx, tile.position.inty, Tile.TileType.Water);
                        if (n < deathlimit)
                        {
                            map[tile.position.intx, tile.position.inty] = new Tile(' ', new Program.Point2D(tile.position.intx, tile.position.inty), Tile.TileType.Floor);
                        }
                        else
                        {

                        }
                    }
                }
                t_map = map;
                //Console.Clear();
                //Program.PrintTileArray(map);
                //Console.ReadKey();
            }

            return map;
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

        Boolean checkCell(Tile[,] map, int x, int y, Tile.TileType t = Tile.TileType.Wall)
        {
            if (x >= 0 & x < map.GetLength(0) &
                y >= 0 & y < map.GetLength(1))
            {
                if (map[x, y].type == t)
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

        int countCells(int[,] m_array, int tocount=0)
        {
            int amount = 0;
            foreach (int tile in m_array)
            {
                if (tile==tocount)
                {
                    amount++;
                }
            }
            return amount;

        }

        public int[,] findEmptyCell(int[,] m_array, bool middle = true, int empty = 1)
        {
            if (middle==false)
            {
                int[,] m_tile = null;
                for (int x = 0; x < m_array.GetLength(0); x++)
                {
                    for (int y = 0; y < m_array.GetLength(1); y++)
                    {
                        if (m_array[x, y] == empty)
                        {
                            m_tile = new int[x, y];
                            return m_tile;
                            
                        }
                    }
                }
                return new int[0, 0]; //technically an error
            }
            else
            {
                //doing a spiral
                int x=0;
                int y=0;

                //get center
                x = (int)Math.Floor((m_array.GetLength(0) / 2.0f)-1.0f);
                y= (int)Math.Floor((m_array.GetLength(1) / 2.0f) - 1.0f);

                int d = 0; //direction
                int c = 1; //current chain size

                for (int k = 0; k <= (m_array.GetLength(0)); k++)
                {
                    for (int j = 0; j < (m_array.GetLength(1)); j++)
                    {
                        for (int l = 0; l < c; l++)
                        {

                            //current tile: m_array[x,y]
                            try
                            {
                                if (m_array[x, y] == empty)
                                {
                                    return new int[x, y];
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Cannot find an empty tile in the middle.");
                                return new int[400, 400];
                            }
                            switch (d)
                            {
                                case 0:
                                    y++;
                                    break;
                                case 1:
                                    x++;
                                    break;
                                case 2:
                                    y--;
                                    break;
                                case 3:
                                    x--;
                                    break;

                            }
                        }
                        d = (d + 1) % 4; //switch directions
                    }
                    c++;
                }
                return new int[0,0]; //technically an error

            }
        }
        public Program.Point2D findEmptyCell(Tile[,] m_array, bool middle = true, int empty = 1)
        {
            if (middle == false)
            {

                for (int x = 0; x < m_array.GetLength(0); x++)
                {
                    for (int y = 0; y < m_array.GetLength(1); y++)
                    {
                        if (m_array[x, y].type == Tile.TileType.Floor)
                        {

                            return new Program.Point2D(x,y);

                        }
                    }
                }
                return new Program.Point2D(0, 0); //technically an error
            }
            else
            {
                //doing a spiral
                int x = 0;
                int y = 0;

                //get center
                x = (int)Math.Floor((m_array.GetLength(0) / 2.0f) - 1.0f);
                y = (int)Math.Floor((m_array.GetLength(1) / 2.0f) - 1.0f);

                int d = 0; //direction
                int c = 1; //current chain size

                for (int k = 0; k <= (m_array.GetLength(0)); k++)
                {
                    for (int j = 0; j < (m_array.GetLength(1)); j++)
                    {
                        for (int l = 0; l < c; l++)
                        {

                            //current tile: m_array[x,y]
                            try
                            {
                                if (m_array[x, y].type == Tile.TileType.Floor)
                                {
                                    return new Program.Point2D(x, y);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Cannot find an empty tile in the middle.");
                                return new Program.Point2D(404, 404);
                            }
                            switch (d)
                            {
                                case 0:
                                    y++;
                                    break;
                                case 1:
                                    x++;
                                    break;
                                case 2:
                                    y--;
                                    break;
                                case 3:
                                    x--;
                                    break;

                            }
                        }
                        d = (d + 1) % 4; //switch directions
                    }
                    c++;
                }
                return new Program.Point2D(0, 0);
            }
        }

        public void printSpiral(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    // x stores the layer in which  
                    // (i, j)th element lies 
                    int x;

                    // Finds minimum of four inputs 
                    x = Math.Min(Math.Min(i, j),
                        Math.Min(n - 1 - i, n - 1 - j));

                    // For upper right half 
                    if (i <= j)
                        Console.Write((n - 2 * x) *
                                      (n - 2 * x) -
                                      (i - x) - (j - x) + "\t");

                    // for lower left half 
                    else
                        Console.Write((n - 2 * x - 2) *
                                      (n - 2 * x - 2) +
                                      (i - x) + (j - x) + "\t");
                }
                Console.WriteLine();
            }
        }
    }

}
