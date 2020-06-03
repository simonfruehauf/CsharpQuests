using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HelloNamespace.Program;

namespace HelloNamespace
{
    class Pathfinder
    {
        public List<Point2D> FindPath(Point2D from, Point2D to, bool[,] map, bool diagonal = false)
        {
            bool pathfound = false;
            List<Point2D> path = new List<Point2D>();
            int counter = 1;
            List<Tuple<Point2D, int>> points = new List<Tuple<Point2D, int>>();
            points.Add(new Tuple<Point2D, int>(new Point2D(to.intx, to.inty), counter));

            while (!pathfound)
            {
                //create list of adjacent cells (from goal) + counter
                counter++;
                //check if cell exists in path, & if walkable
                Point2D[] valid = ValidMovements(diagonal);
                List<Tuple<Point2D, int>> t_points = new List<Tuple<Point2D, int>>();
                t_points = points;
                foreach (Tuple<Point2D, int> tuplepoint in t_points.ToList())
                {
                    foreach (Point2D p in valid.ToList())
                    {
                        Tuple<Point2D, int> m_p = new Tuple<Point2D, int>(new Point2D(tuplepoint.Item1.intx + p.intx, tuplepoint.Item1.inty + p.inty), counter);
                        List<Tuple<Point2D, int>> _points = points;
                        bool add = true;
                        if (IsValidCoord(m_p.Item1.intx, m_p.Item1.inty, map))
                        {

                            foreach (Tuple<Point2D, int> px in points)
                            {
                                if (points.Count > 1000)
                                {
                                    return null;
                                }
                                if (m_p.Item1.x == px.Item1.x && m_p.Item1.y == px.Item1.y) //cell exists in path 
                                {
                                    add = false;
                                }
                            }

                            if (add)
                            {
                                if (map[m_p.Item1.intx, m_p.Item1.inty])
                                {
                                    points.Add(new Tuple<Point2D, int>(new Point2D(tuplepoint.Item1.intx + p.intx, tuplepoint.Item1.inty + p.inty), counter));
                                }
                                //Console.SetCursorPosition(tuplepoint.Item1.intx, tuplepoint.Item1.inty);
                                //Console.Write(tuplepoint.Item2);
                                if (counter > 15)
                                {
                                    return null;
                                }
                                //System.Threading.Thread.Sleep(10);
                            } //cell doesn't exist

                        }
                    }
                }
                t_points = points;
                foreach (Tuple<Point2D, int> item in t_points.ToList())
                {
                    if (IsValidCoord(item.Item1.intx, item.Item1.inty, map))
                    {
                        if (map[item.Item1.intx, item.Item1.inty]) //walkable
                        {

                        }
                        else
                        {
                            points.Remove(item); //not walkable, remove
                        }
                    }

                }

                foreach (Tuple<Point2D, int> item in points)
                {
                    if (IsValidCoord(item.Item1.intx, item.Item1.inty, map))
                    {
                        if (item.Item1.intx == from.intx && item.Item1.inty == from.inty)
                        {
                            pathfound = true;
                        }
                    }
                }


                //foreach (Tuple<Point2D, int> item in points)
                //{
                //    Console.SetCursorPosition(item.Item1.intx, item.Item1.inty);
                //    Console.Write(item.Item2);
                //    System.Threading.Thread.Sleep(10);
                //}
            }

            //go through points and put them in paths as a single list
            Point2D[] valid2 = ValidMovements(diagonal);
            //map to 2d array

            int[,] m_map = new int[map.GetLength(0), map.GetLength(1)];
            foreach (Tuple<Point2D, int> item in points)
            {
                m_map[item.Item1.intx, item.Item1.inty] = item.Item2;
            }

            //end map
            int x = 0;
            foreach (int item in m_map)
            {
                if (x > item)
                {

                }
                else
                    x = item;
            }
            Tuple<Point2D, int> current = new Tuple<Point2D, int>(from, x);
            Tuple<Point2D, int> t_current = current;


            bool pathdone = false;
            int counter2 = 0;
            while (!pathdone)
            {
                if (current.Item1.intx == to.intx && current.Item1.inty == to.inty)
                {
                    pathdone = true;
                }
                if (counter2 >= x)
                {
                    pathdone = true;
                }
                foreach (Point2D p in valid2.ToList())
                {
                    int test = m_map[current.Item1.intx + p.intx, current.Item1.inty + p.inty];
                    //Console.SetCursorPosition(current.Item1.intx + p.intx, current.Item1.inty + p.inty);
                    //Console.Write(":");
                    if (current.Item2 > test && test != 0)
                    {
                        t_current = new Tuple<Point2D, int>(new Point2D(current.Item1.intx + p.intx, current.Item1.inty + p.inty), m_map[current.Item1.intx + p.intx, current.Item1.inty + p.inty]);

                    }

                }

                current = t_current;
                path.Add(new Point2D(t_current.Item1.intx, t_current.Item1.inty));
                //Console.SetCursorPosition(current.Item1.intx, current.Item1.inty);
                //Console.Write("+");
                counter2++;
            }

            return path;
        }
        public bool IsValidCoord(int x, int y, bool[,] map)
        {
            // Our coordinates are constrained between 0 and x.
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x > map.GetLength(0) - 1)
            {
                return false;
            }
            if (y > map.GetLength(1) - 1)
            {
                return false;
            }
            return true;
        }
        static public bool IsValidCoord(int x, int y, Tile[,] map)
        {
            // Our coordinates are constrained between 0 and x.
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x > map.GetLength(0) - 1)
            {
                return false;
            }
            if (y > map.GetLength(1) - 1)
            {
                return false;
            }
            return true;
        }
        Point2D[] ValidMovements(bool diagonal)
        {
            Point2D[] _movements = null;
            // Use 4 or 8 directions, depending on the setup. We use collection
            // initializer syntax here to create the arrays.
            if (!diagonal)
            {
                _movements = new Point2D[]
                {
                    new Point2D(0, -1),
                    new Point2D(1, 0),
                    new Point2D(0, 1),
                    new Point2D(-1, 0)
                };
            }
            else
            {
                _movements = new Point2D[]
                {
                    new Point2D(-1, -1),
                    new Point2D(0, -1),
                    new Point2D(1, -1),
                    new Point2D(1, 0),
                    new Point2D(1, 1),
                    new Point2D(0, 1),
                    new Point2D(-1, 1),
                    new Point2D(-1, 0)
                };
            }
            return _movements;
        }
        public bool[,] makeWalkable(Tile[,] map, Tile target)
        {
            bool[,] m_map = new bool[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].type == Tile.TileType.Floor || map[i, j] == target || map[i, j].type == Tile.TileType.Player || map[i, j].type == Tile.TileType.Character)
                    {
                        m_map[i, j] = true;
                    }
                    else
                    {
                        m_map[i, j] = false;
                    }
                }
            }
            return m_map;
        }
        public bool[,] makeWalkable(int[,] map, List<int> walkable)
        {
            bool[,] m_map = new bool[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    m_map[i, j] = walkable.Contains(map[i, j]) ? true : false;
                }
            }

            return m_map;
        }
        public List<Point2D> bresenham(Tile a, Tile b)
        {
            return bresenham(new Point2D(a.position.intx, a.position.inty), new Point2D(b.position.intx, b.position.inty));
        }
        public List<Point2D> bresenham(int x1, int y1, int x2, int y2)
        {
            return bresenham(new Point2D(x1, y1), new Point2D(x2, y2));
        }
        public List<Point2D> bresenham(Point2D a, Point2D b)
        {
            int m_new = 2 * (b.inty - a.inty);
            int slope_error_new = m_new - (b.intx - a.intx);
            List<Point2D> r = new List<Point2D>();

            for (int x = a.intx, y = a.inty; x <= b.intx; x++)
            {
                //Console.Write("(" + x + "," + y + ")\n");
                r.Add(new Point2D(x, y));
                // Add slope to increment angle formed 
                slope_error_new += m_new;

                // Slope error reached limit, time to 
                // increment y and update slope error. 
                if (slope_error_new >= 0)
                {
                    y++;
                    slope_error_new -= 2 * (b.intx - a.intx);
                }
            }
            return r;
        }
        public bool checkLineOfSight(Tile a, Tile b, Tile[,] map)
        {
            return checkLineOfSight(new Point2D(a.position.intx, a.position.inty), new Point2D(b.position.intx, b.position.inty), map);
        }
        public bool checkLineOfSight(Point2D a, Point2D b, Tile[,] map) //true if nothing blocks
        {
            List<Point2D> line = bresenham(a, b);
            foreach (Point2D point in line)
            {
                switch (map[point.intx, point.inty].type)
                {                    
                    case Tile.TileType.Character:
                    case Tile.TileType.Player:
                    case Tile.TileType.Floor:
                    case Tile.TileType.Item:
                    case Tile.TileType.Object:
                        return false;
                    case Tile.TileType.Null:
                        break;
                    case Tile.TileType.Wall:
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        public int distance(Point2D a, Point2D b)
        {

            return (int)Math.Sqrt(Math.Pow((b.x - a.x), 2) + Math.Pow((b.y - a.y), 2));

        }

        public int distance(Tile a, Tile b)
        {
            return distance(new Point2D(a.position.intx, a.position.inty), new Point2D(b.position.intx, b.position.inty));
        }
    }
}
