using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace HelloNamespace
{

    class SaveHandler
    {
        public static string savefiles = "files\\roguelike\\";
        public static string mapsave = "maps\\";
        public static string mapprefix = "m";
        public static string playersave = "players\\";
        public static string playerprefix = "p";
        public static void SaveMap(Tile[,] map, string name = "default", bool removePlayer = false)
        { 
            string file = savefiles + mapsave + mapprefix+ "-" + name + ".xml";
            List<List<Tile>> convertedMap = new List<List<Tile>>();

            if (!Directory.Exists(savefiles))
            {
                var d = Directory.CreateDirectory(savefiles);
                var f = File.Create(file);
                f.Close();
            }
            if (!Directory.Exists(savefiles+mapsave))
            {
                var d = Directory.CreateDirectory(savefiles + mapsave);
                var f = File.Create(file);
                f.Close();
            }
            if (!Directory.Exists(savefiles + playersave))
            {
                var d = Directory.CreateDirectory(savefiles + playersave);
                var f = File.Create(file);
                f.Close();
            }
            if (!File.Exists(file))
            {
                var f = File.Create(file);
                f.Close();
            }
            if (removePlayer)
            {
                foreach (Tile tile in map)
                {
                    if (tile.player != null)
                    {
                        map[tile.position.intx, tile.position.inty] = tile.standingOn;
                    }
                }
            }
            Type[] types = new Type[] { typeof(Weapon) };
            using (StreamWriter sw = new StreamWriter(file))
            {
                XmlSerializer x = new XmlSerializer(typeof(List<List<Tile>>),types);
                //XmlSerializer x = new XmlSerializer(typeof(Tile[,])); //does not support multidimensional arrays
                x.Serialize(sw, ConvertToListOfLists(map));
            }
            
        }
        public static void SavePlayer(Tile p, string name = "default")
        {
            string file = savefiles + playersave +playerprefix+"-" + name + ".xml";

            if (!Directory.Exists(savefiles))
            {
                var d = Directory.CreateDirectory(savefiles);
                var f = File.Create(file);
                f.Close();
            }
            if (!File.Exists(file))
            {
                var f = File.Create(file);
                f.Close();
            }
           
            using (StreamWriter sw = new StreamWriter(file))
            {
                XmlSerializer x = new XmlSerializer(typeof(Tile));
                //XmlSerializer x = new XmlSerializer(typeof(Tile[,])); //does not support multidimensional arrays
                x.Serialize(sw, p);
            }
        }
        public static Tile[,] ReadMap(string name = "default")
        {
            string file = savefiles + mapsave + mapprefix + "-" + name + ".xml";
            if (!File.Exists(file))
            {
                var f = File.Create(file);
                f.Close();
                return null;
            }
            Type[] types = new Type[] { typeof(Weapon) };

            List<List<Tile>> m = null;
            using (StreamReader sw = new StreamReader(file))
            {
                XmlSerializer x = new XmlSerializer(typeof(List<List<Tile>>), types);
                m = (List<List<Tile>>)x.Deserialize(sw);
            }
            return ConvertToTileMap(m);
        }
        public static Tile ReadPlayer(string name = "default")
        {
            string file = savefiles + playersave + playerprefix + "-" + name + ".xml";
            Tile p;
            if (!File.Exists(file))
            {
                var f = File.Create(file);
                f.Close();
                return null;
            }
            using (StreamReader sw = new StreamReader(file))
            {
                XmlSerializer x = new XmlSerializer(typeof(Tile));
                p = (Tile)x.Deserialize(sw);
            }
            return p;
        }
        public static List<List<Tile>> ConvertToListOfLists(Tile[,] array)
        {
            List<List<Tile>> result = new List<List<Tile>>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                List<Tile> row = new List<Tile>();
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    row.Add(array[i, j]);
                }
                result.Add(row);
            }

            return result;
        }
        public static Tile[,] ConvertToTileMap(List<List<Tile>> list)
        {
            Tile[,] map = new Tile[list.Count(), list[0].Count()];
            int i = 0;
            int j = 0;

            foreach (List<Tile> x in list)
            {
                foreach (Tile y in x)
                {

                    map[i, j] = y;
                    j++;
                }
                j = 0;
                i++;
            }

            return map;
        }
    }
}
