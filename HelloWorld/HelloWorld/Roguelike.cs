using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace HelloNamespace
{
   public class Roguelike
    {
        string inv = "Inventory";
        string titleScreen = "Welcome to the world of";
        string worldName;
        Tile[,] map;
        Map mapsize;
        Map inventorysize;
        bool running = true;
        ConsoleKeyInfo currentInput;
        public Tile player;
        static Random rnd = new Random();
        public Roguelike()
        {
            WordMaker wm = new WordMaker();
            worldName = UppercaseFirst(wm.WordFinder(rnd.Next(3, 10)));
            worldName.ToUpperInvariant();
            titleScreen += " " + worldName + "!";
            CaveGenerator cvg = new CaveGenerator();
            Console.Clear();
            DrawScreen();
            DrawMap(cvg, true, true);
            Play();
        }

        public void Play()
        {
            Thread.Sleep(10);

            do
            {
                bool endround = false;

                
                currentInput = Console.ReadKey(true);
                switch (currentInput.Key)
                {
                    case ConsoleKey.I:
                        //inventory
                        break;
                    case ConsoleKey.UpArrow: //UP
                    case ConsoleKey.W:
                        player.Move(map, Direction.n, false);
                        endround = true;
                        break;
                    case ConsoleKey.DownArrow: // DOWN
                    case ConsoleKey.S:
                        player.Move(map, Direction.s, false);
                        endround = true;
                        break;
                    case ConsoleKey.RightArrow: // DOWN
                    case ConsoleKey.D: // DOWN
                        player.Move(map, Direction.e, false);
                        endround = true;
                        break;
                    case ConsoleKey.LeftArrow: // DOWN
                    case ConsoleKey.A: // DOWN
                        player.Move(map, Direction.w, false);
                        endround = true;
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        Console.Clear();
                        break;
                    default:
                        break;
                }
                Thread.Sleep(10);
                if (endround)
                {
                    //do AI
                    Thread.Sleep(10);
                }
            } while (running);
        }
        public void SpawnEnemy(Enemy e)
        {

        }

        public void SpawnRandomEnemy()
        {
            Tile enemy = new Tile('Q', new Program.Point2D(0, 0), Tile.TileType.Character, ConsoleColor.Red);
            enemy.enemy = enemies[rnd.Next(0, enemies.Count())];
        }
        #region Pregen
        public List<Item> Artefacts = new List<Item>()
        {
            new Weapon("Demonslayer", "A weapon of great power.", 19, 100, 20, 1),
            new Weapon("Bow of Ages", "A bow wielded by heroes.", 19, 60, 20, 2)
        };

        public List<Enemy> enemies = new List<Enemy>()
        {
            new Enemy("Skeleton", 3, 2, 3, 3, null, null, Enemy.Types.melee),
            new Enemy("Zombie", 5, 1, 2, rnd.Next(1,3), null, null, Enemy.Types.melee)

        };
        #endregion

        public enum Axis
        {
            x,
            y
        }
        public enum Direction
        {
            n, ne, e, se, s, sw, w, nw
        }
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
        struct Map
        {
            public Position min;
            public Position max;

            public Map(Position minsize, Position maxsize)
            {
                min = minsize;
                max = maxsize;
            }
        }
        void DrawScreen()
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30
            mapsize = new Map(new Position(1, 1), new Position(((2 * x) / 3) - 1, y - 1));
            inventorysize = new Map(new Position(((2 * x) / 3) + 1, 1), new Position(x - 1, y - 1));

            #region drawborder
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    if ((i == 0 || i == x - 1 || j == 0 || j == y - 1) && i != x && j != y)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(i, j);
                        Console.Write('#');
                    }
                    if (i == 2 * x / 3 && j < y)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write('#');
                    }
                }
            }
            Console.ResetColor();
            #endregion



            //Console.SetCursorPosition(mapsize.min.x + 2, mapsize.min.y + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(GetMiddle(titleScreen, mapsize, Axis.x).x, GetMiddle("", mapsize, Axis.y).x-1);
            Console.Write(titleScreen);
            Console.ResetColor();

            #region inventory
            for (int i = inventorysize.min.x; i < inventorysize.max.x; i++)
            {
                for (int j = inventorysize.min.y; j < inventorysize.max.y; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write('.');

                }
            }

            Position titlepos = GetMiddle(inv, inventorysize, Axis.x);
            Console.SetCursorPosition(titlepos.x, 2);
            Console.Write(inv);
            #endregion

            Console.SetCursorPosition(0, 0);

            Thread.Sleep(200);
        }

        void DrawMap(CaveGenerator c, bool replaceempty = false, bool drawplayer = false)
        {

            Thread.Sleep(300);
            Position titlepos = GetMiddle(worldName, mapsize, Axis.x);
            Console.SetCursorPosition(titlepos.x, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(worldName);
            Console.ResetColor();
            Console.SetCursorPosition(mapsize.min.x, mapsize.min.y);
            map = c.CreateCave(new Program.Point2D(GetDistance(mapsize, Axis.x) + 2, GetDistance(mapsize, Axis.y) + 1), 70, 2);
            map = c.invertMap(map);
            Program.Point2D start = c.findEmptyCell(map, true);
            //Program.Print2dIntArray(map, true, start.GetLength(0), start.GetLength(1));
            map[(int)start.x, (int)start.y] = new Tile('@', new Program.Point2D((int)start.x, (int)start.y), Tile.TileType.Player, ConsoleColor.Red);
            map[(int)start.x, (int)start.y].standingOn = new Tile(' ', new Program.Point2D((int)start.x, (int)start.y), Tile.TileType.Floor);
            map[(int)start.x, (int)start.y].player = new Player(10);
            player = map[(int)start.x, (int)start.y];
            for (int i = mapsize.min.x; i <= mapsize.max.x; i++)
            {
                for (int j = mapsize.min.y; j < mapsize.max.y; j++)
                {
                    Console.ForegroundColor = map[i, j].color;
                    if (map[i, j].type == Tile.TileType.Wall)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(map[i, j].symbol);
                        Thread.Sleep(1);
                    }
                    if (drawplayer && map[i, j].type == Tile.TileType.Player)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(map[i, j].symbol);
                    }
                    if (replaceempty && map[i, j].type == Tile.TileType.Floor)
                    {
                        Console.SetCursorPosition(i, j);

                        Console.Write(map[i, j].symbol);
                    }
                    Console.ResetColor();
                }
            }

            //set player

        }
        int GetDistance(Map map_m, Axis ax)
        {
            switch (ax)
            {
                case Axis.x:
                    return map_m.max.x - map_m.min.x;
                case Axis.y:
                    return map_m.max.y - map_m.min.y;
            }
            return 0;
        }

        int GetMiddle(string input, int space) //from left
        {

            return space - input.Length / 2;
        }

        Position GetMiddle(string input, Map map_m, Axis xy)
        {
            Position pos;
            int stringhalf = input.Length / 2;
            int space = GetDistance(map_m, xy) / 2;

            int posLeft = space - stringhalf;
            if (xy == Axis.x)
            {
                pos = new Position(map_m.min.x + posLeft, 0);

            }
            else
            {
                pos = new Position(map_m.min.y + posLeft, 0);
            }

            return pos;
        }
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }

    public abstract class Item
    {
        public enum ItemType
        {
            Questitem,
            Weapon,
            Equipment,
            Material
        }

        public struct Equipment
        {
            int slot;
            int slot2;
            public Equipment(int s, int s2 = -1)
            {
                slot = s;
                slot2 = s2;

            }

        }
        public Equipment equip;
        public string name;
        public string description;
        public void Pickup(Player p)
        {
            p.inventory.Add(this);
        }
    }

    public class Weapon : Item
    {
        int damage;
        int range;
        public Weapon(string n, string d, int slot, int dam, int slot2 = -1, int abilityindex = 0, int r = 0)
        {
            if (slot2 !=-1)
            {
                equip = new Equipment(slot, slot2);
            }
            else
            {
                equip = new Equipment(slot);
            }
            name = n;
            description = d;
            damage = dam;
            if (r != 0)
            {
                range = r;
            } 
        }
        void Ability(int index)
        {
            switch (index)
            {
                case 0:
                    //does not have ability
                    break;
                case 1: //Demonslayer
                    break;
                case 2: //Bow of Ages
                    break;
                case 3: //
                    break;
                case 4: //
                    break;
                default:
                    break;
            }
        }

    }
    public class Equipment : Item
    {
        public Equipment(string n, string d, ItemType t, int slot, int a, int hb = 0, int sb = 0, int mb = 0)
        {
            equip = new Equipment(slot);
            name = n;
            description = d;
            armor = a;
        }
        public int armor;

        public int healthbonus;
        public int speedbonus;
        public int manabonus;
        
        public void Ability()
        {

        }
    }

    public class Tile
    {

        public Tile(char s, Program.Point2D pos, TileType t, ConsoleColor col = ConsoleColor.White)
        {
            symbol = s;
            type = t;
            position = pos;
            color = col;
        }
        public enum possibleStatus
        {
            Confused,
            Cursed,
            Bleeding,
            Unconscious
        }
        public struct Status
        {
            public possibleStatus name;
            public int length;
        }

        public Item item;
        public Player player;
        public Enemy enemy;
        public List<Status> statuses; 

        public char symbol;
        public ConsoleColor color;
        public enum TileType
        {
            Null,
            Character,
            Player,
            Floor, //can walk on
            Wall,  //blocks player
            Item,  //can be picked up
            Object //can be interacted with
        }
        Program.Point2D position;
        public TileType type;

        public void TileDie()
        {
            type = TileType.Floor;
            symbol = ' ';
        }

        public void TileBorn()
        {
            type = TileType.Wall;
            symbol = 'X';
        }

        public void GainStatus(Status s)
        {
            statuses.Add(s);
        }

        public Tile standingOn;
        public void Move(Tile[,] map, Roguelike.Direction dir, bool ignorewalls = false)
        {
            Program.Point2D moveto;
            switch (dir)
            {
                case Roguelike.Direction.n:
                    moveto = new Program.Point2D(position.x, position.y-1);
                    break;
                case Roguelike.Direction.e:
                    moveto = new Program.Point2D(position.x+1, position.y);
                    break;
                case Roguelike.Direction.s:
                    moveto = new Program.Point2D(position.x, position.y+1);
                    break;
                case Roguelike.Direction.w:
                    moveto = new Program.Point2D(position.x-1, position.y);
                    break;
                default:
                    moveto = new Program.Point2D(position.x, position.y);
                    break;
            }

            Tile goal = map[(int)moveto.x, (int)moveto.y];

            if (!ignorewalls && (map[(int)moveto.x, (int)moveto.y].type == TileType.Floor || map[(int)moveto.x, (int)moveto.y].type == TileType.Item))
            {
                Console.SetCursorPosition((int)position.x, (int)position.y); //select self
                map[(int)position.x, (int)position.y] = standingOn; //set tile beneath to standing on
                Console.ForegroundColor = map[(int)position.x, (int)position.y].color; //set color of tile
                Console.Write(map[(int)position.x, (int)position.y].symbol); //draw tile beneath
                Console.ResetColor();

                standingOn = map[(int)moveto.x, (int)moveto.y]; //set tile beneath to goal
                if (goal.type == TileType.Item && type == TileType.Player)
                {
                    map[(int)position.x, (int)position.y].item.Pickup(player); //pickup if we are player & goal is item
                }

                map[(int)moveto.x, (int)moveto.y] = this; //move ourselves on map

                position = moveto; //update our own position
                Console.ForegroundColor = color;
                Console.SetCursorPosition((int)moveto.x, (int)moveto.y); //draw ourselves
                Console.Write(symbol);
                Console.ResetColor();
            }
            else
            {

            }
            Console.SetCursorPosition(0, 0);

        }

    }

    public class Player
    {
        Item[] equipment = new Item[24];
        int experience;
        public int level;
        int expToNext = 10;
        int gold;
        #region explanationequipment
        // 0    -    9  ring
        // 10   -   11  gloves 
        // 12   -   14  shoes
        // 15           chest
        // 16           pants
        // 17   -   18  sholderpads
        // 19           left hand
        // 20           right hand
        // 21   -   22  amulets
        // 23           pouch
        // 24           backpack
        // -1           filler slot
        public Player(int h)
        {
            health = h;
            maxHealth = h;
        }
        #endregion
        int maxHealth;
        int health;
        public List<Item> inventory;
        
        void Drop(Item i)
        {
            inventory.Remove(i);
        }
        public void GainExp(int exp)
        {
            if (experience+exp >= expToNext)
            {
                experience = exp - (expToNext - experience);
                LevelUp();
            }
            else
            {
                experience += exp;
            }
        }
        void LevelUp()
        {
            level++;

        }

        void Damage(int d, Tile attacker)
        {
            if (health - d <= 0)
            {
                Die();
            }
            else
            {
                health -= d;
            }
        }
        public void GainLoot(Tile source)
        {

        }
        public void GainLoot(Enemy source)
        {
            gold += source.golddrop;
            foreach (var item in source.drops)
            {
                inventory.Add(item);
            }
        }
        void Heal(int h, Tile healer)
        {

            if (health + h > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += h;
            }

        }
        void Die()
        {

        }
    }

    public class Enemy
    {
        string name;
        int damage;
        int maxHealth;
        int health;
        public List<Item> drops; //for material drops
        Item loot; //for direct drop, e.g. weapon
        int expdrop;
        public int golddrop;
        public enum Types
        {
            ranged, 
            melee,
            other
        }
        Types type;
        public Enemy(string n, int h, int da, int xp, int g, List<Item> d = null, Item l = null, Types t = Types.melee)
        {
            damage = da;
            name = n;
            maxHealth = h;
            health = h;
            loot = l;
            drops = d;
            expdrop = xp;
            golddrop = g;
            type = t;
        }
        void Damage(int d, Tile attacker)
        {
            if (health - d <= 0)
            {
                Die(attacker);
            }
            else
            {
                health -= d;
            }
        }
        void Heal(int h, Tile healer)
        {

            if (health + h > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += h;
            }

        }
        void Die(Tile killer = null)
        {
            if (killer != null && killer.player != null)
            {
                killer.player.GainExp(expdrop);
                killer.player.GainLoot(this);
            }
        }

    }

}
