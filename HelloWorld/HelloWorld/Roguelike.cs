using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static HelloNamespace.SaveHandler;
using static HelloNamespace.Tile;

namespace HelloNamespace
{
    public class Roguelike
    {
        string inv = "Inventory";
        string sts = "Stats";
        string pse = "Game Paused";
        string titleScreen = "Welcome to the world of";
        char borderchar = '▓';
        char barchar = '█';
        string worldName;
        int healthbarsize = 1;
        List<MapDirections> Areas;
        public struct MapDirections
        {
            public string area;
            public MapPaths paths { get; set; }
            public MapDirections(string a, MapPaths p)
            {
                area = a;
                paths = p;
            }
        }
        public struct MapPaths 
        {
            public string west { get; set; }
            public string east { get; set; }
            public string north { get; set; }
            public string south { get; set; }
            public MapPaths(string w, string e, string n, string s)
            {
                west = w;
                east = e;
                north = n;
                south = s;
            }
        }

        Tile[,] map;
        Map mapsize;
        Map pausesize;
        Map inventorysize;
        bool running = true;
        ConsoleKeyInfo currentInput;
        public Tile playerTile;
        static Random rnd = new Random();
        List<Enemy> enemies = new List<Enemy>();
        CaveGenerator cvg = new CaveGenerator();

        enum SidePanel
        {
            Inventory,
            Stats,
            Help
        }
        enum Bars
        {
            Mana,
            Health
        }
        enum PauseOptions
        {
            Resume,
            Other,
            Quit,

        }
        int pauseSelector;
        SidePanel sidePanel;
        int sidePanelPage;
        enum PlayerStats
        {
            Wisdom,
            Strength,
            Constitution,
            Null,
            Health,
            Mana
        }
        bool lastpage = false;
        List<string> helpText = new List<string>()
        {
            "W A S D to move",
            "TAB to swap panel",
            "J K to swap page",
            "H for help"
        };
        public Roguelike()
        {
            Areas = new List<MapDirections>();
            if (System.IO.Directory.Exists(savefiles))
            {
                if (System.IO.Directory.Exists(savefiles + playersave))
                {
                    //currently just gets the first savefile //TODO select player to load
                    int count = System.IO.Directory.GetFiles(savefiles + playersave).Length;
                    if (count > 0)
                    {
                        string toread = System.IO.Directory.GetFiles(savefiles + playersave).First();
                        toread = toread.Split('-')[1].Split('.').First();

                        playerTile = ReadPlayer(toread);
                        Areas = playerTile.player.world;
                        worldName = playerTile.player.currentArea;
                        if (System.IO.Directory.Exists(savefiles + mapsave))
                        {
                            map = ReadMap(worldName);
                            if (map == null)
                            {
                                NewWorld();
                            }
                            //find enemies
                            titleScreen += " " + worldName + "!";
                            foreach (Tile item in map)
                            {
                                if (item.player != null)
                                {
                                    playerTile = item;
                                }

                                if (item.enemy != null)
                                {
                                    item.enemy.tile = item;
                                    enemies.Add(item.enemy);
                                }
                            }

                            //end find enemies
                            Console.Clear();
                            DrawScreen();
                            DrawMap(map, true, true);

                        }
                    }
                    else
                    {
                        //save directory exists, but no player save
                        NewWorld();
                    }
                }
                else
                {
                    if (System.IO.Directory.Exists(savefiles + mapsave))
                    {
                        //currently just gets the first savefile //TODO select map to load
                        int count = System.IO.Directory.GetFiles(savefiles + mapsave).Length;
                        if (count > 0)
                        {
                            string toread = System.IO.Directory.GetFiles(savefiles + mapsave).First();
                            worldName = toread.Split('-')[1].Split('.').First();
                            map = ReadMap(worldName);
                            //find enemies
                            titleScreen += " " + worldName + "!";
                            foreach (Tile item in map)
                            {
                                if (item.player != null)
                                {
                                    playerTile = item;
                                }

                                if (item.enemy != null)
                                {
                                    item.enemy.tile = item;
                                    enemies.Add(item.enemy);
                                }
                            }

                            //end find enemies
                            Console.Clear();
                            DrawScreen();
                            DrawMap(map, true, true);
                        }
                    }
                    else
                    {
                        NewWorld();
                    }
                }
            }

            sidePanel = SidePanel.Inventory;
            //for (int i = 0; i < 100; i++) //Debug to populate player inventory
            //{
            //    Item item = Artefacts[rnd.Next(0, Artefacts.Count - 1)];
            //    item.Pickup(player.player);
            //}
            Play();
        }
        public static Program.Point2D directionAsPoint(Direction dir)
        {
            Program.Point2D point;
            switch (dir)
            {
                case Roguelike.Direction.n:
                    point = new Program.Point2D(0,-1);
                    break;
                case Roguelike.Direction.e:
                    point = new Program.Point2D(1, 0);
                    break;
                case Roguelike.Direction.s:
                    point = new Program.Point2D(0, 1);
                    break;
                case Roguelike.Direction.w:
                    point = new Program.Point2D(- 1, 0);
                    break;
                case Roguelike.Direction.ne:
                    point = new Program.Point2D(1, -1);
                    break;
                case Roguelike.Direction.nw:
                    point = new Program.Point2D(-1, -1);
                    break;
                case Roguelike.Direction.se:
                    point = new Program.Point2D(1, 1);
                    break;
                case Roguelike.Direction.sw:
                    point = new Program.Point2D(-1, 1);
                    break;
                default:
                    point = new Program.Point2D(0,0);
                    break;
            }
            return point;
        }
        void NewWorld(string oldWorld = "")
        {
            if (worldName != "" && map != null)
            {
                SaveMap(map, worldName, true); //save old map
            }
            WordMaker wm = new WordMaker();

            worldName = UppercaseFirst(wm.WordFinder(rnd.Next(4, 10)));
            worldName.ToUpperInvariant();
            if (rnd.Next(0, 10) > 8)
            {
                worldName += " " + UppercaseFirst(wm.WordFinder(rnd.Next(3, 4)));
            }
            titleScreen = "Welcome to the world of "+ worldName + "!";

            Console.Clear();
            DrawScreen();
            if (playerTile != null && playerTile.player != null)
            {
                CreateDrawMap(cvg, true, true, true, playerTile.player); 
                MovePlayerNewMap();

            }
            else
            {
                CreateDrawMap(cvg, true, true, true);
            }
            SaveMap(map, worldName, true); //save new map
            if (oldWorld == "")
            {
                Areas.Add(new MapDirections(worldName, new MapPaths())); //adding possible paths
            }
            else //create path to new world
            {
                List<Program.Point2D> edge = new List<Program.Point2D>();
                if (playerTile.position.x <= 1)
                {
                    //east
                    LinkWorlds(oldWorld, worldName, Direction.e);
                    LinkWorlds(worldName, oldWorld, Direction.w);
                    edge = findEmtpyEdge(Direction.w);

                }
                else if (playerTile.position.x >= map.GetLength(0) - 1)
                {
                    //west
                    LinkWorlds(oldWorld, worldName, Direction.w);
                    LinkWorlds(worldName, oldWorld, Direction.e);
                    edge = findEmtpyEdge(Direction.e);
                }
                else if (playerTile.position.y <= 1)
                {
                    //south
                    LinkWorlds(oldWorld, worldName, Direction.s);
                    LinkWorlds(worldName, oldWorld, Direction.n);
                    edge = findEmtpyEdge(Direction.n);

                }
                else if (playerTile.position.y >= map.GetLength(1) - 1)
                {
                    //north
                    LinkWorlds(oldWorld, worldName, Direction.n);
                    LinkWorlds(worldName, oldWorld, Direction.s);
                    edge = findEmtpyEdge(Direction.s);
                }

                //create paths
                //TODO currently does not work
                //createPath(playerTile.position);
            }

        }
        bool LinkWorlds(string from, string to, Direction d)
        {
            int ind = -1;
            retryfind:
            for (int i = 0; i < Areas.Count; i++)
            {
                if (Areas[i].area == from)
                {
                    ind = i;
                }
            }
            if (ind == -1)
            {
                Areas.Add(new MapDirections(from, new MapPaths())); //adding possible paths
                goto retryfind;
            }
            else
            {
                switch (d)
                {
                    case Direction.n:
                        Areas[ind] = new MapDirections(Areas[ind].area, new MapPaths(Areas[ind].paths.west, Areas[ind].paths.east, to, Areas[ind].paths.south));
                            break;
                    case Direction.e:
                        Areas[ind] = new MapDirections(Areas[ind].area, new MapPaths(Areas[ind].paths.west, to, Areas[ind].paths.north, Areas[ind].paths.south));
                        break;
                    case Direction.s:
                        Areas[ind] = new MapDirections(Areas[ind].area, new MapPaths(Areas[ind].paths.west, Areas[ind].paths.east, Areas[ind].paths.north, to));
                        break;
                    case Direction.w:
                        Areas[ind] = new MapDirections(Areas[ind].area, new MapPaths(to, Areas[ind].paths.east, Areas[ind].paths.north, Areas[ind].paths.south));
                        break;
                }
            }
            return true;
        }
        List<Program.Point2D> findEmtpyEdge(Direction d)
        {
            List<Program.Point2D> values = new List<Program.Point2D>();
            Program.Point2D dir = new Program.Point2D(0, 0);
            Program.Point2D start = new Program.Point2D(0, 0);
            bool down = true;
            switch (d)
            {
                case Direction.n:
                    dir = new Program.Point2D(1, 0);
                    start = new Program.Point2D(1, 1);
                    down = false;
                    break;
                case Direction.e:
                    dir = new Program.Point2D(0, 1);
                    start = new Program.Point2D(map.GetLength(0) - 1, 1);
                    break;
                case Direction.s:
                    dir = new Program.Point2D(1, 0);
                    start = new Program.Point2D(1, map.GetLength(1)-1);
                    down = false;
                    break;
                case Direction.w:
                    dir = new Program.Point2D(0, 1);
                    start = new Program.Point2D(1,1);
                    break;
                default:
                    break;
            }
            int count = map.GetLength(down ? 1 : 0) - 2;
            for (int i = 0; i < count; i++)
            {
                Program.Point2D p = map[start.intx + dir.intx * i, start.inty + dir.inty * i].position;
                if (map[start.intx + dir.intx*i, start.inty + dir.inty * i].type == TileType.Floor || map[start.intx + dir.intx * i, start.inty + dir.inty * i].type == TileType.Water)
                {
                    values.Add(map[start.intx + dir.intx * i, start.inty + dir.inty * i].position);
                }
            }

            return values;
        }
        public void Play()
        {
            Thread.Sleep(100);
            DrawBars(Bars.Health);
            DrawBars(Bars.Mana);
            do
            {
                bool endround = false;
                int type = 0;
                if (playerTile.player.equipment[20] != null)
                {
                    if (playerTile.player.equipment[20].GetType() == typeof(Weapon))
                    {
                        type = ((Weapon)playerTile.player.equipment[20]).damageType; //right hand determins attack type
                    }
                    else
                    {
                        type = 0;
                    }
                }
                else
                {
                    type = 0;
                }
                playerTile.DrawSelf();
                currentInput = Console.ReadKey(true);
                switch (currentInput.Key)
                {
                    case ConsoleKey.F:  //DEBUG

                       //SpawnRandomEnemy();
                        playerTile.player.health -= 10;

                        //Pathfinder pf = new Pathfinder();
                        //Tile target = map[player.standingOn.position.intx+7, player.standingOn.position.inty + 1];
                        //List<Program.Point2D> line = pf.bresenham(player, target);
                        //foreach (Program.Point2D point in line)
                        //{
                        //    Console.SetCursorPosition(point.intx, point.inty);
                        //    Console.Write("Ö");
                        //}
                        //List<Program.Point2D> path = new List<Program.Point2D>();
                        //path = pf.FindPath(player.standingOn.position, new Program.Point2D(player.standingOn.position.intx, player.standingOn.position.inty + 5), pf.makeWalkable(map, target));
                        //foreach (Program.Point2D item in path)
                        //{
                        //    Console.SetCursorPosition(item.intx, item.inty);
                        //    Console.Write("Ö");
                        //}
                        break;
                    case ConsoleKey.Tab:
                        int panels = (Enum.GetNames(typeof(SidePanel)).Length);
                        sidePanel = (SidePanel)((int)(sidePanel + 1) % (panels - 1)); //-1 because last is help panel
                        sidePanelPage = 0; //reset pages
                        DrawPanel(sidePanel);
                        break;
                    case ConsoleKey.J: //next page
                        if (sidePanel == SidePanel.Stats)
                        {

                        }
                        else if (!lastpage)
                        {
                            sidePanelPage++;
                            DrawPanel(sidePanel);
                        }
                        break;
                    case ConsoleKey.K: //prev page
                        if (!(sidePanelPage - 1 < 0))
                        {
                            sidePanelPage--;
                            DrawPanel(sidePanel);
                        }
                        break;
                    case ConsoleKey.H: //help
                        DrawPanel(SidePanel.Help);
                        break;
                    case ConsoleKey.UpArrow: //UP ATTACK
                        map[playerTile.position.intx, playerTile.position.inty].TryTarget(map, Direction.n, playerTile.player.GetRange(type), true);
                        endround = true;
                        break;
                    case ConsoleKey.W://UP MOVE
                        playerTile.Move(map, Direction.n, false);
                        endround = true;
                        break;
                    case ConsoleKey.DownArrow: // DOWN ATTACK
                        map[playerTile.position.intx, playerTile.position.inty].TryTarget(map, Direction.s, playerTile.player.GetRange(type), true);
                        endround = true;
                        break;
                    case ConsoleKey.S: // DOWN MOVE
                        playerTile.Move(map, Direction.s, false);
                        endround = true;
                        break;
                    case ConsoleKey.RightArrow: // RIGHT ATTACK
                        map[playerTile.position.intx, playerTile.position.inty].TryTarget(map, Direction.e, playerTile.player.GetRange(type), true);
                        endround = true;
                        break;
                    case ConsoleKey.D: // RIGHT MOVE
                        playerTile.Move(map, Direction.e, false);
                        endround = true;
                        break;
                    case ConsoleKey.LeftArrow: // LEFT ATTACK
                        map[playerTile.position.intx, playerTile.position.inty].TryTarget(map, Direction.w, playerTile.player.GetRange(type), true);
                        endround = true;
                        break;
                    case ConsoleKey.A: // LEFT MOVE
                        playerTile.Move(map, Direction.w, false);
                        endround = true;
                        break;
                    case ConsoleKey.Spacebar: // SKIP
                        endround = true;
                        break;
                    case ConsoleKey.Escape:
                        //display pause

                        switch (Pause())
                        {
                            case PauseOptions.Other: //???
                            case PauseOptions.Resume: //resume
                                DrawMap(map, true, true);
                                break;
                            case PauseOptions.Quit: //exit
                                running = false;
                                SaveMap(map, worldName, true);
                                playerTile.player.world = Areas;
                                playerTile.player.currentArea = worldName;
                                SavePlayer(playerTile, "default");
                                Console.Clear();
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }
                Thread.Sleep(10);
                if (endround)
                {
                    DrawBars(Bars.Health);
                    DrawBars(Bars.Mana);
                    //AI
                    List<Enemy> t_enemies = enemies;
                    foreach (Enemy e in t_enemies.ToList())
                    {
                        if (e == null)
                        {
                            enemies.Remove(e);
                        }
                        else if (e.health <= 0)
                        {
                            enemies.Remove(e);
                        }
                    }
                    Pathfinder pf = new Pathfinder();
                    foreach (Enemy m_enemy in enemies)
                    {
                        if (m_enemy != null && m_enemy.tile != null)
                        {


                            if (pf.distance(m_enemy.tile, playerTile) < m_enemy.sightRadius)
                            {
                                m_enemy.target = playerTile;
                                //sees player
                            }
                            else if (pf.distance(m_enemy.tile, playerTile) > m_enemy.sightRadius * 2)
                            {
                                m_enemy.target = null;
                                //lost player
                            }
                            if (m_enemy.target != null)
                            {
                                Random rnd = new Random();
                                int chance = rnd.Next(10, 100);
                                if (chance <= m_enemy.speed * 100) //walk tick
                                {
                                    m_enemy.MoveTowards(map, m_enemy.target);
                                    int i = 1;
                                    while (m_enemy.speed * 100 - chance * i > 100)
                                    {
                                        m_enemy.MoveTowards(map, m_enemy.target);
                                        i++;
                                    }
                                }
                            }
                            Thread.Sleep(10);
                        }

                        Thread.Sleep(10);
                    }
                }
                playerTile.player.EndOfRound();
                if (playerTile.player.offscreen)
                {
                    string toload = "";
                    foreach (MapDirections item in Areas)
                    {
                        if (item.area == worldName) //current world
                        {
                            if (playerTile.position.x <= 1)
                            {
                                //west
                                if (item.paths.west!=null)
                                {
                                    toload = item.paths.west;
                                }
                                else
                                {
                                    toload = "";
                                }
                            }
                            else if (playerTile.position.x >= map.GetLength(0)-1)
                            {
                                //east
                                if (item.paths.east != null)
                                {
                                    toload = item.paths.east;
                                }
                                else
                                {
                                    toload = "";
                                }
                            }
                            else if (playerTile.position.y <= 1)
                            {
                                //north
                                if (item.paths.north != null)
                                {
                                    toload = item.paths.north;
                                }
                                else
                                {
                                    toload = "";
                                }
                            }
                            else if (playerTile.position.y >= map.GetLength(1)-1)
                            {
                                //south
                                if (item.paths.south != null)
                                {
                                    toload = item.paths.south;
                                }
                                else
                                {
                                    toload = "";
                                }
                            }
                        }
                    }
                    if (toload == "")
                    {
                        NewWorld(worldName);
                    }
                    else //load that map 
                    {
                        map = ReadMap(toload);
                        MovePlayerNewMap();
                        List<Program.Point2D> edge = new List<Program.Point2D>();
                        if (playerTile.position.x <= 1)
                        {
                            edge = findEmtpyEdge(Direction.w);

                        }
                        else if (playerTile.position.x >= map.GetLength(0) - 1)
                        {
                            
                            edge = findEmtpyEdge(Direction.e);
                        }
                        else if (playerTile.position.y <= 1)
                        {
                            //south
                            edge = findEmtpyEdge(Direction.n);

                        }
                        else if (playerTile.position.y >= map.GetLength(1) - 1)
                        {
                            //north
                            edge = findEmtpyEdge(Direction.s);
                        }

                        //create paths
                        //TODO currently does not work
                        //createPath(playerTile.position);

                        //find empty on edge, put player there
                        //pick random empty
                        if (edge.Count == 0)
                        {
                            //couldn't find empty space. shit
                        }
                        else
                        {
                            Program.Point2D playerpos = edge[rnd.Next(0, edge.Count() - 1)];
                            playerTile.MoveTeleport(map, playerpos);
                        }

                        worldName = toload;
                        DrawBorder();
                        DrawMap(map, true, true);
                    }
                    playerTile.player.offscreen = false;
                }
                //DrawPanel(sidePanel);
            } while (running);
        }
        void MovePlayerNewMap()
        {
            //TODO invert player position
            if (playerTile.position.x <= 1)
            {
                //north to south
                playerTile.position = new Program.Point2D(map.GetLength(0) - 1, playerTile.position.inty) ;
            }
            else if (playerTile.position.y <= 1)
            {
                //west to east
                playerTile.position = new Program.Point2D(playerTile.position.intx, map.GetLength(1) - 1) ;

            }
            else if (playerTile.position.x >= map.GetLength(0)-1)
            {
                //south to north
                playerTile.position = new Program.Point2D(1, playerTile.position.inty);
            }
            else if (playerTile.position.y >= map.GetLength(1)-1)
            {
                //west to east
                playerTile.position = new Program.Point2D(playerTile.position.intx, 1);
            }
            map[playerTile.position.intx, playerTile.position.inty] = playerTile;
        }
        PauseOptions Pause()
        {
            //draw box
            DrawPause();
            //select stuff
            return SelectPause();



        }
        PauseOptions SelectPause()
        {
            int pauseOptions = Enum.GetNames(typeof(PauseOptions)).Length;
            bool exit = false;
            //draw
            List<Program.Point2D> positions = new List<Program.Point2D>();
            string arrow = "<<<";
            int j = pausesize.min.x;
            int k = pausesize.min.y;
            int maxwordlength = 13;
            int topbuffer = 5;
            int arrowbuffer = 3;
            int titlex = (j + (pausesize.max.x - j) / 2) - pse.Length / 2;
            positions.Add(new Program.Point2D(titlex, k + 1)); //title

            for (int i = 1; i < pauseOptions + 1; i++)
            {
                if (i * 2 < pausesize.max.x)
                {
                    positions.Add(new Program.Point2D(j + topbuffer, k + 2 * i));
                }
            }
            int v = 0;

            foreach (Program.Point2D item in positions)
            {

                Console.SetCursorPosition((int)item.x, (int)item.y);
                if (v != 0)
                {
                    PauseOptions pauseItem = (PauseOptions)v - 1;
                    Console.Write(pauseItem.ToString());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(pse);
                    Console.ResetColor();
                }
                v++;
            }
            positions.RemoveAt(0);
            //select

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((int)positions[pauseSelector].x + arrowbuffer + maxwordlength, (int)positions[pauseSelector].y); //current position
                Console.Write(arrow);
                Console.ResetColor();


                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if ((pauseSelector % pauseOptions) - 1 < 0)
                        {
                            pauseSelector = pauseSelector + (pauseOptions - 1);

                            if (pauseSelector >= pauseOptions)
                            {
                                pauseSelector = pauseOptions - 1;
                            }
                        }
                        else
                        {
                            pauseSelector = ((pauseSelector - 1) % pauseOptions + pauseOptions) % pauseOptions; // keeps it between 0 and x and not -x and x
                        }
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (pauseSelector + 1 >= pauseOptions)
                        {

                            pauseSelector = 0;
                        }
                        else
                        {
                            pauseSelector = (pauseSelector + 1) % pauseOptions;
                        }
                        break;
                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                }
                Console.SetCursorPosition(Console.CursorLeft - arrow.Length, Console.CursorTop);
                for (int i = 0; i < arrow.Length; i++)
                {
                    Console.Write(" ");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((int)positions[pauseSelector].x + arrowbuffer + maxwordlength, (int)positions[pauseSelector].y); //current position
                Console.Write(arrow);
                Console.ResetColor();
            } while (!exit);
            return (PauseOptions)pauseSelector;
        }
        public void SpawnEnemy(Enemy e)
        {
            Tile enemy = new Tile('Q', new Program.Point2D(0, 0), Tile.TileType.Character, ConsoleColor.Red);
            enemy.enemy = e;
            enemy.enemy.tile = enemy;
            enemies.Add(enemy.enemy);
        }

        public void SpawnRandomEnemy()
        {
            int x, y;
            x = rnd.Next(playerTile.position.intx - 10, playerTile.position.intx + 20);
            y = rnd.Next(playerTile.position.inty - 10, playerTile.position.inty + 20);
            if (Pathfinder.IsValidCoord(x, y, map) && (x != playerTile.position.intx && y != playerTile.position.inty))
            {
                SpawnRandomEnemy(new Program.Point2D(x, y));
            }
        }
        public void SpawnRandomEnemy(Program.Point2D p)
        {
            Tile thisEnemy = new Tile('Q', p, Tile.TileType.Character, ConsoleColor.Red);
            thisEnemy.enemy = pregenEnemies[rnd.Next(0, pregenEnemies.Count())];
            thisEnemy.enemy.tile = thisEnemy;
            enemies.Add(thisEnemy.enemy);
            thisEnemy.enemy.tile.standingOn = map[thisEnemy.enemy.tile.position.intx, thisEnemy.enemy.tile.position.inty];
            map[thisEnemy.enemy.tile.position.intx, thisEnemy.enemy.tile.position.inty] = thisEnemy.enemy.tile;
            thisEnemy.DrawSelf(map);
        }
        #region Pregen
        public List<Item> Artefacts = new List<Item>()
        {
            new Weapon("Demonslayer", "A weapon of great power.", 19, 100, 20, 1),
            new Weapon("Bow of Ages", "A bow wielded by heroes.", 19, 60, 20, 2),
            new Weapon("Potato Cannon", "", 19, 60, 20, 2),
            new Weapon("Bananabread", "", 19, 60, 20, 2),
            new Weapon("Yellow Boat", "", 19, 60, 20, 2),
            new Weapon("Bag of Holding", "", 19, 60, 20, 2),
            new Weapon("Filler Object", "", 19, 60, 20, 2),
        };

        public List<Enemy> pregenEnemies = new List<Enemy>()
        {
            new Enemy("Skeleton", 3, 2, 3, 3, null, null, Enemy.Types.melee),
            new Enemy("Ghoul", 3, 2, 3, 3, null, null, Enemy.Types.melee),
            new Enemy("Goblin", 3, 2, 3, 3, null, null, Enemy.Types.melee),
            new Enemy("Ghost", 3, 2, 3, 3, null, null, Enemy.Types.melee),
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
        void DrawBorder()
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    if ((i == 0 || i == x - 1 || j == 0 || j == y - 1 || (j == y - 3 && i < inventorysize.min.x /*Bars*/)) && i != x && j != y)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(i, j);
                        Console.Write(borderchar);
                    }
                    if (i == 2 * x / 3 && j < y) //sidebar line
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(i, j);
                        Console.Write(borderchar);
                    }
                    if (!isEven(mapsize.max.x / 2))
                    {
                        if ((i == mapsize.max.x / 2 || i == (mapsize.max.x / 2) - 1) && j == mapsize.max.y + healthbarsize) //middle bottom
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.SetCursorPosition(i, j);
                            Console.Write(borderchar);
                        }
                    }
                    else
                    {
                        if (i == mapsize.max.x / 2 && j == mapsize.max.y + healthbarsize) //middle bottom
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.SetCursorPosition(i, j);
                            Console.Write(borderchar);
                        }
                    }

                }
            }
            Console.ResetColor();
        }
        void DrawScreen(bool sleep = true)
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30
            GetMapSize();
            inventorysize = new Map(new Position(((2 * x) / 3) + 1, 1), new Position(x - 1, y - 1));


            #region drawborder
            DrawBorder();
            #endregion



            //Console.SetCursorPosition(mapsize.min.x + 2, mapsize.min.y + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(GetMiddle(titleScreen, mapsize, Axis.x).x, GetMiddle("", mapsize, Axis.y).x - 1);
            Console.Write(titleScreen);
            Console.ResetColor();


            Console.SetCursorPosition(0, 0);
            if (sleep)
            {
                Thread.Sleep(100);
            }
        }
        bool isEven(int i)
        {
            return i % 2 == 0;
        }
        void DrawBars(Bars bar)
        {
            List<int> lines = new List<int>(); //acts as y
            for (int i = 1; i <= healthbarsize; i++)
            {
                lines.Add(mapsize.max.y + i);
            }
            ConsoleColor c;

            int startx = 0;
            int endx = 0; //acts as from to x
            int current = 0;
            int max = 0;
            if (bar == Bars.Health)
            {
                startx = mapsize.min.x;
                endx = isEven(mapsize.max.x / 2) ? (mapsize.max.x / 2) - 2 : (mapsize.max.x / 2) - 1;
                c = ConsoleColor.DarkRed;
                current = playerTile.player.health;
                max = playerTile.player.maxHealth;
            }
            else
            {
                c = ConsoleColor.DarkBlue;
                startx = isEven(mapsize.max.x / 2) ? (mapsize.max.x / 2) + 2 : (mapsize.max.x / 2) + 1;
                endx = mapsize.max.x + 1;
                current = playerTile.player.mana;
                max = playerTile.player.maxMana;
            }


            int length = endx - startx;
            int missing = max - current;
            missing = missing * length / max; //normalize to 0-x range, missing out of x are empty
            //invert if health
            if (bar == Bars.Health)
            {
                missing = length - missing;
            }
            foreach (int iy in lines)
            {
                Console.SetCursorPosition(startx, iy);

                for (int i = 0; i < length; i++)
                {
                    if (bar == Bars.Health)
                    {
                        if (i >= missing)//check if missing
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = c;
                        }
                    }
                    else
                    {
                        if (i < missing)//check if missing
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = c;
                        }
                    }
                    Console.Write(barchar);
                }
            }

            lines.Clear();

        }
        void GetMapSize()
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30
            mapsize = new Map(new Position(1, 1), new Position(((2 * x) / 3) - 1, y - 1 - healthbarsize - 1));

        }
        void DrawPause()
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30#            
            GetMapSize();
            pausesize = new Map(new Position(mapsize.min.x + (mapsize.max.x / 3), mapsize.min.y + (mapsize.max.y / 3)), new Position(mapsize.max.x - (mapsize.max.x / 3), mapsize.max.y - (mapsize.max.y / 3)));
            for (int i = pausesize.min.x; i <= pausesize.max.x; i++)
            {
                for (int j = pausesize.min.y; j <= pausesize.max.y; j++)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(i, j);
                    if (i == pausesize.min.x)
                    {
                        if (j == pausesize.min.y)
                            Console.Write('┌');
                        else if (j == pausesize.max.y)
                            Console.Write("└");
                        else
                            Console.Write("│");
                    }
                    else if (i == pausesize.max.x)
                    {
                        if (j == pausesize.min.y)
                            Console.Write("┐");
                        else if (j == pausesize.max.y)
                            Console.Write("┘");
                        else
                            Console.Write("│");
                    }
                    else if (j == pausesize.min.y || j == pausesize.max.y)
                    {
                        Console.Write('─');
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.ResetColor();
                }
            }
        }
        void DrawPanel(SidePanel panel = SidePanel.Inventory)
        {
            int x = Console.WindowWidth;  //120
            int y = Console.WindowHeight; //30
            GetMapSize();
            inventorysize = new Map(new Position(((2 * x) / 3) + 1, 1), new Position(x - 1, y - 1));
            switch (panel)
            {
                case SidePanel.Help:
                    for (int i = inventorysize.min.x; i < inventorysize.max.x; i++)
                    {
                        for (int j = inventorysize.min.y; j < inventorysize.max.y; j++)
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(i, j);
                            Console.Write(' ');
                            Console.ResetColor();
                        }
                    }
                    Position helppos = GetMiddle("Help", inventorysize, Axis.x);
                    Console.SetCursorPosition(helppos.x, 2);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Help");
                    Console.ResetColor();

                    //help lines

                    int helpLines = helpText.Count;
                    List<Program.Point2D> helpPoints = new List<Program.Point2D>();
                    for (int i = 0; i < helpLines; i++)
                    {
                        helpPoints.Add(new Program.Point2D(inventorysize.min.x + 3, inventorysize.min.y + 2 + i * 2));
                    }
                    int l = 0;
                    foreach (Program.Point2D item in helpPoints)
                    {
                        Console.SetCursorPosition((int)item.x, (int)item.y);
                        Console.Write(helpText[l]);
                        l++;
                    }
                    //help lines end

                    break;
                case SidePanel.Inventory:
                    for (int i = inventorysize.min.x; i < inventorysize.max.x; i++)
                    {
                        for (int j = inventorysize.min.y; j < inventorysize.max.y; j++)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(i, j);
                            Console.Write(' ');
                            Console.ResetColor();
                        }
                    }
                    Position titlepos = GetMiddle(inv, inventorysize, Axis.x);
                    Console.SetCursorPosition(titlepos.x, 2);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(inv);
                    Console.ResetColor();
                    //display all items

                    int freeLines = (inventorysize.max.y - inventorysize.min.y) / 2 - 2;

                    List<Program.Point2D> positions = new List<Program.Point2D>();
                    for (int i = 0; i < freeLines; i++)
                    {
                        positions.Add(new Program.Point2D(inventorysize.min.x + 3, inventorysize.min.y + 2 + i * 2));
                    }
                    int v = 0;
                    foreach (Program.Point2D item in positions)
                    {
                        int m_item = v + sidePanelPage * freeLines;
                        Console.SetCursorPosition((int)item.x, (int)item.y);
                        string text = "";
                        if (m_item > playerTile.player.inventory.Count - 1)
                        {
                            lastpage = true;
                            break;
                        }
                        else
                        {
                            lastpage = false;
                            text = playerTile.player.inventory[v + (sidePanelPage * freeLines)].name;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write(text);
                            Console.ResetColor();
                        }

                        v++;
                    }
                    //items end
                    //write page
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    if (sidePanelPage + 1 < 10)
                    {
                        Console.SetCursorPosition(inventorysize.max.x - 1, inventorysize.max.y - 1);
                        Console.Write(sidePanelPage + 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(inventorysize.max.x - 2, inventorysize.max.y - 1);
                        Console.Write(sidePanelPage + 1);
                    }

                    Console.ResetColor();
                    //page end
                    break;
                case SidePanel.Stats:
                default:
                    for (int i = inventorysize.min.x; i < inventorysize.max.x; i++)
                    {
                        for (int j = inventorysize.min.y; j < inventorysize.max.y; j++)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(i, j);
                            Console.Write(' ');
                            Console.ResetColor();
                        }
                    }
                    Position titlepos2 = GetMiddle(sts, inventorysize, Axis.x);
                    Console.SetCursorPosition(titlepos2.x, 2);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(sts);
                    int statLines = (inventorysize.max.y - inventorysize.min.y) / 2 - 2;
                    int maxwordlength = 12;
                    List<Program.Point2D> statPos = new List<Program.Point2D>();
                    for (int i = 0; i < statLines; i++)
                    {
                        statPos.Add(new Program.Point2D(inventorysize.min.x + 8, inventorysize.min.y + 4 + i * 2));
                    }
                    int v2 = 0;
                    foreach (Program.Point2D item in statPos)
                    {
                        int m_item = v2 * statLines;
                        Console.SetCursorPosition((int)item.x, (int)item.y);
                        string text = "";
                        switch (v2)
                        {
                            case 0:
                                if (playerTile.player.str > 9)
                                {
                                    text = "Strength:     " + playerTile.player.str;
                                }
                                else
                                {
                                    text = "Strength:      " + playerTile.player.str;

                                }
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkGreen;

                                break;
                            case 1:
                                if (playerTile.player.str > 9)
                                {
                                    text = "Constitution: " + playerTile.player.str;
                                }
                                else
                                {
                                    text = "Constitution:  " + playerTile.player.str;

                                }
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 2:
                                if (playerTile.player.str > 9)
                                {
                                    text = "Wisdom:       " + playerTile.player.str;
                                }
                                else
                                {
                                    text = "Wisdom:        " + playerTile.player.str;

                                }
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            case 3:
                                text = "";
                                break;
                            case 4: // Health
                                int healthlength = playerTile.player.health.ToString().Length + " / ".Length + playerTile.player.maxHealth.ToString().Length;
                                text = "Health: ";
                                for (int i = 0; i < maxwordlength - healthlength; i++) //adjust to how much health player has
                                {
                                    text = text + " ";
                                }
                                text = text + playerTile.player.health + " / " + playerTile.player.maxHealth;
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 5: // Mana
                                int manalength = playerTile.player.mana.ToString().Length + " / ".Length + playerTile.player.maxMana.ToString().Length;

                                text = "Health: ";
                                for (int i = 0; i < maxwordlength - manalength; i++) //adjust to how much mana player has
                                {
                                    text = text + " ";
                                }
                                text = text + playerTile.player.mana + " / " + playerTile.player.maxMana;
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            default:
                                text = "";
                                break;
                        }
                        //Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write(text);


                        v2++;
                    }
                    Console.ResetColor();

                    break;
            }
        }
        void CreateDrawMap(CaveGenerator c, bool replaceempty = false, bool drawplayer = false, bool addlakes = false, Player p = null)
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
            if (addlakes)
            {
                map = c.addLakes(map, 3, 5);
            }
            Program.Point2D start = c.findEmptyCell(map, true);
            //Program.Print2dIntArray(map, true, start.GetLength(0), start.GetLength(1));
            
            if (p == null)
            {
                map[(int)start.x, (int)start.y] = new Tile('@', new Program.Point2D((int)start.x, (int)start.y), Tile.TileType.Player, ConsoleColor.Blue);
                map[(int)start.x, (int)start.y].standingOn = new Tile(' ', new Program.Point2D((int)start.x, (int)start.y), Tile.TileType.Floor);
                map[(int)start.x, (int)start.y].player = new Player(8, 8, 8);
                playerTile = map[(int)start.x, (int)start.y];
            }
            else
            {

            }
            for (int i = mapsize.min.x; i <= mapsize.max.x; i++)
            {
                for (int j = mapsize.min.y; j < mapsize.max.y; j++)
                {
                    Console.ForegroundColor = map[i, j].color;
                    if (map[i, j].type != Tile.TileType.Floor && map[i, j].type != Tile.TileType.Player)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(map[i, j].symbol);
                        //Thread.Sleep(1);
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


        }
        void DrawMap(Tile[,] map, bool replaceempty = false, bool drawplayer = false)
        {
            Position titlepos = GetMiddle(worldName, mapsize, Axis.x);
            Console.SetCursorPosition(titlepos.x, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(worldName);
            for (int i = mapsize.min.x; i <= mapsize.max.x; i++)
            {
                for (int j = mapsize.min.y; j < mapsize.max.y; j++)
                {
                    Console.ForegroundColor = map[i, j].color;
                    if (map[i, j].type == Tile.TileType.Wall)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(map[i, j].symbol);
                        //Thread.Sleep(1);
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
                    else
                    {
                        Console.SetCursorPosition(i, j);

                        Console.Write(map[i, j].symbol);
                    }
                    Console.ResetColor();
                }
            }
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
    [Serializable]
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
        public int damageboost;
        public bool damagemultiply; //if false, damageboost gets added flat, if true, added as percentage
        public int boosttype;  // 0 == melee, 1 == ranged, 2 == magic

        public void Pickup(Player p)
        {
            p.inventory.Add(this);
        }
    }
    [Serializable]
    public class Weapon : Item
    {
        public int damage;
        public int range; //1 is immediately in front
        public int damageType; // 0 == melee, 1 == ranged, 2 == magic
        public Weapon(string n, string d, int slot, int dam, int slot2 = -1, int abilityindex = 0, int r = 0)
        {
            if (slot2 != -1)
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

        private Weapon() //for XML
        { }
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
    [Serializable]
    public class Tile
    {
        private Tile() //for XML
        {

        }

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
            Object, //can be interacted with
            Water
        }
        public Program.Point2D position;
        public TileType type;

        public void TileDie()
        {
            type = TileType.Floor;
            symbol = ' ';
        }
        public bool IsSwimming()
        {
            return standingOn.type == TileType.Water;
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
        public void MoveTeleport(Tile[,] map, Tile target, bool ignorewalls = false)
        {
            MoveTeleport(map, new Program.Point2D(target.position.intx, target.position.inty), ignorewalls);
        }
        public void MoveTeleport(Tile[,] map, Program.Point2D point, bool ignorewalls = false) //teleport
        {
            Program.Point2D moveto = point;
            if (!(moveto.x > map.GetLength(0) || moveto.x < 0 || moveto.y > map.GetLength(1) || moveto.y < 0)) //borders
            {
                Tile goal = map[moveto.intx, moveto.inty];

                if (!ignorewalls && (map[(int)moveto.x, (int)moveto.y].type == TileType.Floor || map[(int)moveto.x, (int)moveto.y].type == TileType.Item))
                {
                    Console.SetCursorPosition((int)position.x, (int)position.y); //select self
                    map[(int)position.x, (int)position.y] = standingOn; //set tile beneath to standing on
                    if (map[(int)position.x, (int)position.y].color != null)
                    {
                        Console.ForegroundColor = map[(int)position.x, (int)position.y].color; //set color of tile
                    }
                    Console.Write(map[(int)position.x, (int)position.y].symbol); //draw tile beneath
                    Console.ResetColor();

                    standingOn = map[(int)moveto.x, (int)moveto.y]; //set tile beneath to goal
                    if (goal.type == TileType.Item && type == TileType.Player)
                    {
                        map[(int)position.x, (int)position.y].item.Pickup(player); //pickup if we are player & goal is item
                    }

                    map[(int)moveto.x, (int)moveto.y] = this; //move ourselves on map
                    if (this.player != null)
                    {
                        map[(int)moveto.x, (int)moveto.y].player = this.player; //move player?

                    }

                    position = moveto; //update our own position
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition((int)moveto.x, (int)moveto.y); //draw ourselves
                    Console.Write(symbol);
                    Console.ResetColor();
                }
                else if (map[(int)moveto.x, (int)moveto.y].player != null && enemy != null)
                {
                    map[(int)moveto.x, (int)moveto.y].player.TakeDamage(enemy.damage, this);

                    Console.Beep();
                    Thread.Sleep(100);
                }
                Console.SetCursorPosition(0, 0);
            }
            else
            {
                //moving out of the map
                int nil = 0;
            }

        }
        public void DrawSelf(Tile[,] map)
        {
            Console.ForegroundColor = map[(int)position.x, (int)position.y].color;
            Console.SetCursorPosition((int)position.x, (int)position.y);
            Console.Write(map[(int)position.x, (int)position.y].symbol);
            Console.ResetColor();
        }
        public void DrawSelf()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition((int)position.x, (int)position.y);
            Console.Write(symbol);
            Console.ResetColor();
        }
        public void Move(Tile[,] map, Roguelike.Direction dir, bool ignorewalls = false, bool canSwim = true)
        {
            Program.Point2D moveto;
            switch (dir)
            {
                case Roguelike.Direction.n:
                    moveto = new Program.Point2D(position.x, position.y - 1);
                    break;
                case Roguelike.Direction.e:
                    moveto = new Program.Point2D(position.x + 1, position.y);
                    break;
                case Roguelike.Direction.s:
                    moveto = new Program.Point2D(position.x, position.y + 1);
                    break;
                case Roguelike.Direction.w:
                    moveto = new Program.Point2D(position.x - 1, position.y);
                    break;
                default:
                    moveto = new Program.Point2D(position.x, position.y);
                    break;
            }
            if (!(moveto.x > map.GetLength(0) - 1 || moveto.x < 1 || moveto.y > map.GetLength(1) - 1 || moveto.y < 1)) //borders
            {
                Tile goal = map[(int)moveto.x, (int)moveto.y];

                if (!ignorewalls && (map[(int)moveto.x, (int)moveto.y].type == TileType.Floor || map[(int)moveto.x, (int)moveto.y].type == TileType.Item) || (canSwim && map[(int)moveto.x, (int)moveto.y].type == TileType.Water))
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
                    if (this.player != null)
                    {
                        map[(int)moveto.x, (int)moveto.y].player = this.player; //move player?
                    }
                    position = moveto; //update our own position
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition((int)moveto.x, (int)moveto.y); //draw ourselves
                    Console.Write(symbol);
                    Console.ResetColor();
                }
                else if (map[(int)moveto.x, (int)moveto.y].enemy != null)
                {
                    map[(int)moveto.x, (int)moveto.y].enemy.TakeDamage(player.GetPlayerDamage(0), this);
                    //if (map[(int)moveto.x, (int)moveto.y].enemy.health <= 0)
                    //{
                    //    map[(int)moveto.x, (int)moveto.y].enemy.Die(this);
                    //}
                    //melee enemy
                    Console.Beep();
                }
                Console.SetCursorPosition(0, 0);
            }
            else
            {
                //moving out of the map
                player.offscreen = true;
            }
        }
        public Tile TryTarget(Tile[,] map, Roguelike.Direction direction, int range = 1, bool ignoreplayers = false) //returns first enemy or player in that direction
        {
            Program.Point2D dir = Roguelike.directionAsPoint(direction);

            for (int i = 1; i < range; i++)
            {
                Program.Point2D tocheck = new Program.Point2D(position.intx + dir.intx*i, position.inty + dir.inty*i);
                if (map[tocheck.intx, tocheck.inty].enemy != null || map[tocheck.intx, tocheck.inty].player != null)
                {
                    if (ignoreplayers && map[tocheck.intx, tocheck.inty].player != null) //ignore players
                    {
                        
                    }
                    else
                    {
                        return map[tocheck.intx, tocheck.inty]; //return first enemy
                    }
                }

            }

            return null;
        }

    }

    public class Player
    {
        public Item[] equipment = new Item[24];
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
        #endregion
        public List<Item> inventory;

        public bool offscreen; //used to generate a new map
        public List<Roguelike.MapDirections> world;
        public string currentArea;
        public Player(int s, int w, int c)
        {
            str = s;
            wis = w;
            con = c;
            health = (int)(con * hmod);
            maxHealth = health;

            mana = (int)(wis * mmod);
            maxMana = mana;
            inventory = new List<Item>();
            manaregen = w / 8; //max mana is w* mmod (3.6f currently)
            healthregen = 50; //50% chance for health per turn, or average 0.5 health per turn
            world = new List<Roguelike.MapDirections>();
        }
        private Player() //for XML
        { }
        public int maxHealth, maxMana;
        public int health, mana;
        public int manaregen, healthregen; //%chance of regaining 1 resource per turn. each 100 adds a minimum 1 regen per turn.
        public int str, wis, con;
        public bool resourceblocked, manablocked, healthblocked = false;
        private int resourceblockedrounds, manablockedrounds, healthblockedrounds = 0;
        private float hmod = 2.2f;
        private float mmod = 3.6f;
        void Drop(Item i)
        {
            inventory.Remove(i);
        }
        public void GainExp(int exp)
        {
            if (experience + exp >= expToNext)
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

        public void EndOfRound()
        {
            RemoveBlocks();
            RegenMana();
            RegenHealth();
        }
        public void BlockResource(int resource = 2, int rounds = 1) //0 == health, 1 == mana, 2 == all
        {
            switch (resource)
            {
                case 0:
                    healthblocked = true;
                    if (healthblockedrounds != 0)
                    {
                        healthblockedrounds += rounds;

                    }
                    else
                    {
                        healthblockedrounds = rounds;
                    }
                    break;
                case 1:
                    manablocked = true;
                    if (manablockedrounds != 0)
                    {
                        manablockedrounds += rounds;

                    }
                    else
                    {
                        manablockedrounds = rounds;
                    }
                    break;
                case 2:
                    resourceblocked = true;
                    if (resourceblockedrounds != 0)
                    {
                        resourceblockedrounds += rounds;

                    }
                    else
                    {
                        resourceblockedrounds = rounds;
                    }
                    break;
                default:
                    break;
            }
        }

        void RemoveBlocks()
        {
            if (manablockedrounds > 0)
                manablockedrounds--;
            else
                manablocked = false;

            if (healthblockedrounds > 0)
                healthblockedrounds--;
            else
                healthblocked = false;

            if (resourceblockedrounds > 0)
                resourceblockedrounds--;
            else
                resourceblocked = false;
        }
        void RegenMana(int a = 0, int b = 0, int c = 0, int d = 0)
        {
            if (mana < maxMana && !resourceblocked && !manablocked)
            {
                Random rnd = new Random();
                int ch = rnd.Next(1, 100);
                int regen = 0;
                int remain = manaregen;
                while (remain > 100)
                {
                    regen++; 
                    remain -= 100;
                }
                if (ch < remain)
                {
                    regen++;
                }
                mana += regen + a + b + c + d;
            }
            if (mana > maxMana)
            {
                mana = maxMana;
            }
        }
        void RegenHealth(int a = 0, int b = 0, int c = 0, int d = 0)
        {
            if (health < maxHealth && !resourceblocked  && !healthblocked)
            {
                Random rnd = new Random();
                int ch = rnd.Next(1, 100);
                int regen = 0;
                int remain = healthregen;
                while (remain > 100)
                {
                    regen++; //if 
                    remain -= 100;
                }
                if (ch < remain)
                {
                    regen++;
                }
                health += regen + a + b + c + d;
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        public void TakeDamage(int d, Tile attacker) //player takes damage
        {
            if (health - d <= 0)
            {
                health = 0;
                Die();
            }
            else
            {
                health -= d;
            }
        }
        public bool UseMana(int c)
        {
            if (mana - c < 0)
            {
                return false; //not enough mana
            }
            else
            {
                mana -= c;
            }
            return true; //enough mana
        }
        public void GainLoot(Tile source)
        {

        }
        public void GainLoot(Enemy source)
        {
            gold += source.golddrop;
            if (source.drops != null)
            {
                foreach (var item in source.drops)
                {
                    if (item != null)
                    {
                        inventory.Add(item);
                    }
                }
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
            //RIP
        }
        public int GetPlayerDamage(int type = 0, bool useitems = true) // 0 == melee, 1 == ranged, 2 == magic
        {
            int d = 0;
            switch (type)
            {
                case 0:
                    if (equipment[19] != null) //fist left
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type)
                            {
                                d += ((Weapon)equipment[19]).damage + str / 2;
                            }
                        }

                    }
                    else
                    {
                        d += str / 2;
                    }

                    if (equipment[20] != null) //fist right
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                d += ((Weapon)equipment[20]).damage + str / 2;
                            }
                        }

                    }
                    else
                    {
                        d += str / 2;
                    }

                    foreach (Item item in equipment)
                    {
                        if (item != null)
                        {
                            if (item.damageboost != 0)
                            {
                                if (item.boosttype == type)
                                {
                                    if (item.damagemultiply)
                                    {
                                        d += (d / 100) * item.damageboost;
                                    }
                                    else
                                    {
                                        d += item.damageboost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1: //ranged
                    if (equipment[19] != null) //have equip
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type) //is ranged
                            {
                                d += ((Weapon)equipment[19]).damage; //no str added
                            }
                        }

                    }
                    else
                    {
                        d += 0; //fists are not ranged equipment
                    }

                    if (equipment[20] != null) //fist right
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                d += ((Weapon)equipment[20]).damage; //no str added
                            }
                        }
                    }
                    else
                    {
                        d += 0;//fists are not ranged equipment
                    }
                    foreach (Item item in equipment)
                    {
                        if (item != null)
                        {
                            if (item.damageboost != 0)
                            {
                                if (item.boosttype == type)
                                {
                                    if (item.damagemultiply)
                                    {
                                        d += (d / 100) * item.damageboost;
                                    }
                                    else
                                    {
                                        d += item.damageboost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2: //magic
                    if (equipment[19] != null) //fist left
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type) //is ranged
                            {
                                d += ((Weapon)equipment[19]).damage + wis / 2;
                            }
                        }

                    }
                    else
                    {
                        d += 0; //fists are not magic weapons
                    }

                    if (equipment[20] != null) //fist right
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                d += ((Weapon)equipment[20]).damage + wis / 2;
                            }
                        }

                    }
                    else
                    {
                        d += 0;//fists are not magic weapons
                    }

                    foreach (Item item in equipment)
                    {
                        if (item != null)
                        {
                            if (item.damageboost != 0)
                            {
                                if (item.boosttype == type)
                                {
                                    if (item.damagemultiply)
                                    {
                                        d += (d / 100) * item.damageboost;
                                    }
                                    else
                                    {
                                        d += item.damageboost;
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return d;
        }
        public int GetRange(int type = 0) // 0 == melee, 1 == ranged, 2 == magic // if this returns 0, we don't have a wepon of this type
        {
            int range = 0;
            switch (type)
            {
                case 0: //melee
                    if (equipment[19] != null) //have weapon
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type)
                            {
                                range = ((Weapon)equipment[19]).range;
                            }
                        }

                    }
                    else if (equipment[20] != null) //have weapon
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                range = ((Weapon)equipment[20]).range;
                            }
                        }
                    }
                    else
                    {
                        range = 1; //fists
                    }
                    return range;

                case 1: //range
                    if (equipment[19] != null) //have weapon
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type)
                            {
                                range = ((Weapon)equipment[19]).range;
                            }
                        }

                    }
                    else if (equipment[20] != null) //have weapon
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                range = ((Weapon)equipment[20]).range;
                            }
                        }
                    }
                    else
                    {
                        range = 0; //no ranged weapon
                    }
                    return range;
                case 3: //magic
                    if (equipment[19] != null) //have weapon
                    {
                        if (equipment[19].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[19]).damageType == type)
                            {
                                range = ((Weapon)equipment[19]).range;
                            }
                        }

                    }
                    else if (equipment[20] != null) //have weapon
                    {
                        if (equipment[20].GetType() == typeof(Weapon)) //is weapon
                        {
                            if (((Weapon)equipment[20]).damageType == type)
                            {
                                range = ((Weapon)equipment[20]).range;
                            }
                        }
                    }
                    else
                    {
                        range = 0; //no weapon
                    }
                    return range;
                default:
                    return 1;
            }
        }
    }

    [Serializable]
    public class Enemy
    {
        internal Tile tile { get; set; }
        public string name;
        internal int damage;
        internal int maxHealth;
        public int health;
        public List<Item> drops; //for material drops
        internal Item loot; //for direct drop, e.g. weapon
        internal int expdrop;
        public int golddrop;
        internal Tile target;
        internal List<Program.Point2D> path;
        //internal List<Roguelike.Direction> path = new List<Roguelike.Direction>();
        public int sightRadius;
        public float speed; //tiles or attacks per tick, 1 == 1 tile, 3 == 3 tiles
        public enum Types
        {
            ranged,
            melee,
            other
        }
        internal Types type;
        public Enemy(string n, int h, int da, int xp, int g, List<Item> d = null, Item l = null, Types t = Types.melee, Tile tl = null, int sr = 5, float sp = 0.75f)
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
            tile = tl;
            sightRadius = sr;
            speed = sp;
        }
        private Enemy() //for XML saving
        {

        }
        public void TakeDamage(int d, Tile attacker) //enemy takes damage
        {
            if (health - d <= 0)
            {
                health = 0;
                Die(attacker);
            }
            else
            {
                health -= d;
            }

        }
        public void Heal(int h, Tile healer)
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
        public void Die(Tile killer = null)
        {
            if (killer != null && killer.player != null)
            {
                killer.player.GainExp(expdrop);
                killer.player.GainLoot(this);
            }
            if (tile.enemy != null)
            {
                tile.enemy = null;
                tile.type = tile.standingOn.type;
                tile.symbol = tile.standingOn.symbol;
                tile.position = tile.standingOn.position;
                tile.color = tile.standingOn.color;
                tile = tile.standingOn;
                tile.DrawSelf();
            }
        }

        void SetTarget(Tile t)
        {
            target = t;
        }
        void SetTarget(Enemy e)
        {
            target = e.tile;
        }
        void PlanPath(Tile[,] m, Tile t = null)
        {
            if (t == null)
            {
                t = target;
            }
            Pathfinder pf = new Pathfinder();
            path = pf.FindPath(new Program.Point2D(tile.position.x, tile.position.y), new Program.Point2D(t.position.x, t.position.y), pf.makeWalkable(m, t));
            if (path != null)
            {
                path = path.Distinct().ToList();
            }
        }
        public void MoveTowards(Tile[,] m, Tile target)
        {
            Pathfinder pf = new Pathfinder();
            PlanPath(m, target);
            if (path != null)
            {
                tile.MoveTeleport(m, path[0]);
                path.RemoveAt(0);
            }

        }
    }
    public class Boss : Enemy
    {
        public Boss(string n, int h, int da, int xp, int g, List<Item> d = null, Item l = null, Types t = Types.melee) : base(n, h, da, xp, g, d, l, t)
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
    }

    public class Minion : Enemy
    {
        Enemy owner; //owning enemy
        public Minion(string n, int h, int da, int xp, int g, List<Item> d = null, Item l = null, Types t = Types.melee, Enemy boss = null) : base(n, h, da, xp, g, d, l, t)
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
            owner = boss;
        }
    }
}
