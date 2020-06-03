using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace HelloNamespace
{
    class MapReader
    {
        string mapDirectory = "files\\maps";
        int[,] currentMap;
        static char[] characters = new char[]{
            // # is used as an error indicator
            ' ',
            'X',
            'x',
            '.',
            ' ',
            ' ',
        };
        public int[,] Read(string file)
        {
            int[,] map;
            CheckCreateFiles(null, file);
            if (new FileInfo(mapDirectory + "\\" + file + ".txt").Length == 0)
            {
                // empty
                return null;
            }
            string[] s = File.ReadAllLines(mapDirectory + "\\" + file + ".txt");
            string longest = s.OrderByDescending(sx => sx.Length).First();

            map = new int[s.Length, longest.Length];
            for (int i = 0; i < s.Length; i++) //i == current line
            {
                string line = s[i];
                // Process line
                int j = 0;
                foreach (char ch in line)
                {
                    bool success = Int32.TryParse(ch.ToString(), out map[i, j]);
                    if (!success)
                    {
                        map[i, j] = 0;
                    }
                    j++;
                }
            }
            currentMap = map;
            return map;

        }
        public void Write(int[,] map, char[] chars = null, string file = null)
        {
            CheckCreateFiles(mapDirectory);

                if (chars == null)
            {
                chars = characters; //circumvent compile time constant error
            }

            if (file == null)
            {
                //check files, add one, save as
                int n = ExistingMaps()-1;
                CheckCreateFiles(mapDirectory, n.ToString());
                file = mapDirectory + "\\" + n + ".txt";
            }
            else
            {
                CheckCreateFiles(mapDirectory, file);
                file = mapDirectory + "\\" + file + ".txt";
            }

            using (TextWriter tw = new StreamWriter(file))
            {
                int x_size = map.GetLength(0);
                int y_size = map.GetLength(1);
                int counter = 0;
                for (int col = 0; col < y_size; col++)
                {
                    for (int row = 0; row < x_size; row++)
                    {
                        if (counter >= x_size) //check if we wrote a whole row, add line break
                        {
                            tw.Write("\n");
                            counter = 0;
                        }
                        counter++;
                        tw.Write(map[row, col]);
                    }
                }
            }
        }

        public void ChangeTile(int[,] map, Program.Point2D p, int to)
        {
            map[(int)p.x, (int)p.y] = to;
        }

        public void Blink(int[,] map, Program.Point2D p, int fr, int to, int times)
        {
            //this currently does not allow anything else to run while blinking,
            //it might require multithreading to work 
            bool flip = true;
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(200); 
                Console.SetCursorPosition((int)p.x, (int)p.y);
                if (flip)
                    Console.Write(characters[fr]);
                else
                    Console.Write(characters[to]);
                flip = !flip;
            }
        }
        public void Draw(bool[,] map)
        {
            int counter = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (counter >= map.GetLength(1)) //check if we wrote a whole row
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    counter++;

                    Console.Write((map[i, j] ? " " : "#"));
                }
            }


        }
        public void Draw(int[,] map)
        {
            int counter = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (counter >= map.GetLength(1)) //check if we wrote a whole row
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    counter++;

                    Console.Write((map[i, j] <= characters.Length - 1/*0 based*/) ? characters[map[i, j]].ToString() : "#");

                }
            }
        }
        int ExistingMaps(string dir = null)
        {
            if (dir == null)
            {
                dir = mapDirectory;
            }
            return Directory.GetFiles(dir).Length;
        }
        int CheckCreateFiles(string dir = null, string file = null) //1 files existed, 0 files created
        {
            if (dir == null)
            {
                dir = mapDirectory;
            }
            if (file == null)
            {
                //check dir only
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                if (file != null)
                {
                    var t_file = File.Create(dir + "\\" + file + ".txt");
                    t_file.Close();
                }
                return 0;
            }

            if (!File.Exists(dir + "\\" + file + ".txt"))
            {
                var t_file = File.Create(dir + "\\" + file + ".txt");
                t_file.Close();
                return 0;
            }
            else
            {
                return 1;
            }

        }
    }
}
