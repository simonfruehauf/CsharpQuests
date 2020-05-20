using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloNamespace
{
    class Cisharpmon
    {
        private const string folder = "files";
        private const string savefile = "cisharpmon save.txt";
        private const string monsterfile = "monsters.txt";
        private const char stop = '#';
        private string[] saveinfo;
        private int saveLength;
        Player currentplayer;
        int x;
        int y;
        bool debug = true;
        Random rnd = new Random();
        List<Monster> starters;

        public void Play()
        {

            Console.Clear();
            DrawScreen();

            Console.SetCursorPosition(GetMiddle("Hello.", x - 2), 5);
            Write.TypeLine("Hello.");
            System.Threading.Thread.Sleep(200);
            Console.WriteLine();

            Console.SetCursorPosition(GetMiddle("Welcome to Cisharpmon.", x - 2), 6);
            Write.TypeLine("Welcome to Cisharpmon.", 60);
            System.Threading.Thread.Sleep(500);

            switch (ReadMonsterFile())
            {
                case 0:
                    //we have monsters
                    List<string> all = new List<string>(File.ReadLines(folder + "//" + monsterfile));
                    List<string> one = new List<string>(all[0].Split(stop));
                    List<string> two = new List<string>(all[1].Split(stop));
                    List<string> three = new List<string>(all[2].Split(stop));
                    string[] starternames = new string[3] {one[0], two[0], three[0]};
                    starters = new List<Monster>();
                    for (int i = 0; i < 3; i++)
                    {
                        Monster m = LoadMonster(starternames[i]);
                        starters.Add(m);
                    }
                    break;
                default:
                    for (int i = 0; i < 3; i++)
                    {
                        SaveMonster(RandomMonster());
                    }
                    goto case 0;
            }
            switch (ReadSavefile())
            {
                case 0: //load
                    LoadPlayer(saveinfo);
                    break;
                case 1: //no save or empty
                    NewPlayer();
                    break;
                case 2: //save corrupted
                    break;
                default:
                    break;
            }

        }
        void NewPlayer()
        {
            Console.SetCursorPosition(GetMiddle("It seems you are new...", x - 2), 7);
            Write.TypeLine("It seems you are new");
            Write.TypeLine("...", 400);
            System.Threading.Thread.Sleep(200);
            Console.SetCursorPosition(GetMiddle("Creating a new save file for you...", x - 2), 8);
            Write.TypeLine("Creating a new save file for you");
            Write.TypeLine("...", 400);
            System.Threading.Thread.Sleep(200);
            bool retry = false;
        retrying:
            Console.SetCursorPosition(GetMiddle(retry ? "Well, what is your name then?" : "What's your name?", x - 2), 9);
            Write.TypeLine(retry ? "Well, what is your name then?" : "What's your name?");
            Console.SetCursorPosition(GetMiddle("What's your name?", x - 2), 10);
            string name = Console.ReadLine();
            Console.SetCursorPosition(GetMiddle("Ah, " + name + ", is that correct? Y/N", x - 2), 11);
            Write.TypeLine("Ah, " + name + ", is that correct? Y/N");

        retryinput:
            ConsoleKey input = Console.ReadKey(true).Key;
            switch (input)
            {
                case ConsoleKey.Y:
                    break;
                case ConsoleKey.N:
                    //delete previous lines
                    Console.SetCursorPosition(2, 9);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(2, 10);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(2, 11);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    retry = true;
                    goto retrying;
                default:
                    goto retryinput;
            }

            Console.Clear();
            DrawScreen();
            currentplayer = new Player(name, null, 0);
            Save(currentplayer);
            SelectStarter();
            Save(currentplayer);
            }

        void SelectStarter()
        {
            DrawScreen();


            //display starters...

            Console.SetCursorPosition(GetMiddle("You can adopt one of these three starter cisharpmons!", x - 2), 4);
            Write.TypeLine("You can adopt one of these three starter cisharpmons!");
            string starter0 = starters[0].name + ", a " + starters[0].type.main + " / " + starters[0].type.secondary + " monster.";
            string starter1 = starters[1].name + ", a " + starters[1].type.main + " / " + starters[1].type.secondary + " monster.";
            string starter2 = starters[2].name + ", a " + starters[2].type.main + " / " + starters[2].type.secondary + " monster.";

            Console.SetCursorPosition(GetMiddle(starter0 , x - 2), 5);
            Write.TypeLine(starter0, 50);

            Console.SetCursorPosition(GetMiddle(starter0, x - 2), 6);
            Write.TypeLine(starter1, 50);

            Console.SetCursorPosition(GetMiddle(starter0, x - 2), 7);
            Write.TypeLine(starter2, 50);


        retrying:
            bool retry = false;
            Console.SetCursorPosition(GetMiddle(retry ? "Try again." : "Which one of them do you want to adopt?", x - 2), 9);
            Write.TypeLine(retry ? "Try again." : "Which one of them do you want to adopt?");
            Console.SetCursorPosition(GetMiddle("          ", x - 2), 10);
            string name = Console.ReadLine();
            if (name != starters[0].name && name != starters[1].name && name != starters[2].name)
            {
                retry = true;
                goto retrying;
            }
            Console.SetCursorPosition(GetMiddle("Oh, the " + name + ", is that correct? Y/N", x - 2), 11);
            Write.TypeLine("Oh, the " + name + ", is that correct? Y/N");
        retryinput:
            ConsoleKey input = Console.ReadKey(true).Key;
            switch (input)
            {
                case ConsoleKey.Y:
                    break;
                case ConsoleKey.N:
                    //delete previous lines
                    Console.SetCursorPosition(2, 9);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(2, 10);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(2, 11);
                    for (int i = 1; i < x - 2; i++)
                    {
                        Console.Write(" ");
                    }
                    retry = true;
                    goto retrying;
                default:
                    goto retryinput;
            }

            currentplayer.Roster = new List<Monster>() { (LoadMonster(name)) };
            Console.SetCursorPosition(GetMiddle("Added " + name + " to your roster.", x - 2), 12);
            Write.TypeLine("Added " + name + " to your roster.");
            System.Threading.Thread.Sleep(200);
            Console.Clear();
        }


        Monster RandomMonster()
        {
            WordMaker wm = new WordMaker();
            Random rnd = new Random();
            System.Threading.Thread.Sleep(1);
            string name = UppercaseFirst(wm.WordFinder(rnd.Next(3, 7)));
            switch (rnd.Next(0,5))
            {
                case 0:
                    name += "mon";
                    break;
                case 1:
                    name += "mon";
                    break;
                case 3:
                    name += "mon";
                    break;
                case 4:
                    name += "mon";
                    break;
                case 5:
                    name += "ken";
                    break;
                default:
                    break;
            }

            List<Attack> attacks = new List<Attack>();
            Array values = Enum.GetValues(typeof(Types));
            Types t = (Types)rnd.Next(1, values.Length);
            Types t2 = (Types)rnd.Next(0, values.Length);
            if (t2 == t)
            {
                t2 = 0;
            }
            Attack a = RandomAttack(t);
            Attack b = RandomAttack(t);
            Attack c = RandomAttack(t);
            Attack d = RandomAttack(t2);
            attacks.Add(a);
            attacks.Add(b);
            attacks.Add(c);
            attacks.Add(d);



            return new Monster(name, rnd.Next(80, 120), rnd.Next(1, 6), rnd.Next(75, 90), 0, 1, new Monster.Type(t, t2), attacks);
        }

        Attack RandomAttack(Types t)
        {
            WordMaker wm = new WordMaker();

            System.Threading.Thread.Sleep(1);
            string name = UppercaseFirst(wm.WordFinder(rnd.Next(3, 7)));
            int damage = rnd.Next(7, 20);
            int healing = 0;
            int defenseboost = 0;
            int accuracyboost = 0;
            switch (rnd.Next(0, 6))
            {
                case 1:
                    healing = damage;
                    damage = 0;
                    break;
                case 2:
                    healing = damage / rnd.Next(2, 3);
                    damage = damage / 2;
                    break;
                case 3:
                    damage = (damage*2)/3;
                    accuracyboost = rnd.Next(5, 10);
                    break;
                case 4:
                    damage = rnd.Next(damage / 3, damage);
                    defenseboost = rnd.Next(5, 10);
                    break;
                default:
                    break;
            }

            return new Attack(name, t, damage, rnd.Next(60, 70), healing, defenseboost, accuracyboost);
        }
        void DrawScreen()
        {

            #region border
            x = Console.WindowWidth;  //120
            y = Console.WindowHeight; //30

            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    if ((i == 0 || i == x - 1 || j == 0 || j == y - 1) && i != x && j != y)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(i, j);
                        Console.Write('#');
                    }
                }
            }
            Console.ResetColor();
            #endregion

        }

        int GetMiddle(string input, int space) //from left
        {

            return space/2 - input.Length / 2;
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
        int ReadMonsterFile()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                var f = File.Create(folder + "\\" + monsterfile);
                f.Close();
                return 1;
            }
            else if (!File.Exists(folder + "\\" + monsterfile))
            {
                var f = File.Create(folder + "\\" + monsterfile);
                f.Close();
                return 1;
            }
            else if (File.Exists(folder + "\\" + monsterfile))
            {
                if (new FileInfo(folder + "\\" + monsterfile).Length == 0)
                {
                    return 1;
                }

                return 0;
            }
            return 2;
        }
        int ReadSavefile()
        {
            
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                var f = File.Create(folder + "\\" + savefile);
                f.Close();
                return 1;
            }
            else if (!File.Exists(folder + "\\" + savefile))
            {
                var f = File.Create(folder + "\\" + savefile);
                f.Close();
                return 1;
            }
            else if (File.Exists(folder + "\\" + savefile))
            {
                if (new FileInfo(folder + "\\" + savefile).Length == 0)
                {
                    return 1;
                }
                //load
                string line = File.ReadLines(folder + "\\" + savefile).First();
                saveinfo = line.Split(stop);

                return 0;
            }
            return 2;
        }


        Player LoadPlayer(string[] saveinfo)
        {
            List<Monster> monsters = new List<Monster>();
            for (int i = 2; i < saveinfo.Length; i++)
            {
                monsters.Add(LoadMonster(saveinfo[i]));
            }

            Player p = new Player(saveinfo[0], monsters, Convert.ToInt32(saveinfo[1]));
            return p;
        }
        void Save(Player p)
        {
            string playersave = p.name; //first is name, then defeated monsters, index 2+ is monster roster names up to 7
            playersave = playersave + stop + p.defeatedMonsters;
            if (p.Roster != null)
            {
                foreach (Monster m in p.Roster)
                {
                    playersave = playersave + stop + m.name;
                }



                foreach (Monster monster in p.Roster)
                {
                    SaveMonster(monster);
                }
            }
            using (TextWriter tw = File.CreateText(folder + "\\" + savefile))
            {
                tw.WriteLine(playersave);
            }
        }
        void SaveMonster(Monster m)
        {
            //check if monster exists
            ReadMonsterFile();
            string[] lines = File.ReadAllLines(folder + "\\" + monsterfile);

            foreach (string item in lines)
            {
                if (item.Contains(m.name))
                {
                    return; //we found the monster already
                }
            }

            string monstervalues = m.name + stop + m.healthTop + stop + m.defense + stop + m.accuracy + stop + m.experience + stop + m.level + stop + m.type.main + stop + m.type.secondary;
            int counter = m.attacks.Count();
            for (int i = 0; i < counter; i++)
            {
                monstervalues += stop + m.attacks[i].name + stop + m.attacks[i].type + stop + m.attacks[i].damage + stop + m.attacks[i].requiredAccuracy + stop + m.attacks[i].heal + stop + m.attacks[i].defenseBoost + stop + m.attacks[i].accuracyBoost;
            }
            using (TextWriter tw = File.AppendText(folder + "\\" + monsterfile))
            {
               tw.WriteLine(monstervalues);
            }
        }
        Monster LoadMonster(string name)
        {
            if (new FileInfo(folder + "\\" + monsterfile).Length != 0)
            {
                string[] lines = File.ReadAllLines(folder + "\\" + monsterfile);
                string monster = "";
                foreach (string s in lines)
                {
                    if (s.StartsWith(name))
                    {
                        monster = s;
                    }
                }
                if (monster == "")
                {

                    //error
                }

                string[] info = monster.Split('#');
                List<Attack> attacks = new List<Attack>() //9, 16.. types
                {
                new Attack(info[8], (Types)Enum.Parse(typeof(Types),info[9]), Convert.ToInt32(info[10]), Convert.ToInt32(info[11]),Convert.ToInt32(info[12]), Convert.ToInt32(info[13]), Convert.ToInt32(info[14])),
                new Attack(info[15], (Types)Enum.Parse(typeof(Types),info[16]),Convert.ToInt32(info[17]), Convert.ToInt32(info[18]),Convert.ToInt32(info[19]), Convert.ToInt32(info[20]), Convert.ToInt32(info[21])),
                new Attack(info[22], (Types)Enum.Parse(typeof(Types),info[23]),Convert.ToInt32(info[24]), Convert.ToInt32(info[25]),Convert.ToInt32(info[26]), Convert.ToInt32(info[27]), Convert.ToInt32(info[28])),
                new Attack(info[29], (Types)Enum.Parse(typeof(Types),info[30]),Convert.ToInt32(info[31]), Convert.ToInt32(info[32]),Convert.ToInt32(info[33]), Convert.ToInt32(info[34]), Convert.ToInt32(info[35]))
                };

                return new Monster(info[0], Convert.ToInt32(info[1]), Convert.ToInt32(info[2]), Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), Convert.ToInt32(info[5]), new Monster.Type((Types)Enum.Parse(typeof(Types),(info[6])), (Types)Enum.Parse(typeof(Types),info[7])), attacks);

            }
            else
            {
                return null;
            }
        }
        enum Types
        {
            generic,
            hot,
            cold,
            light,
            heavy,
            dark,
            shining

        }
        class Monster
        {
            protected internal string name;
            protected internal int health, healthTop, defense, accuracy;
            //assuming level 1
            //health:   ~100
            //defense:  ~5, in % reduction
            //accuracy: ~50
            protected internal int tempDefense, tempAccuracy;
            protected internal int experience;
            protected internal int level;


            protected internal struct Type
            {
                internal Types main, secondary;
                public Type( Types m, Types s)
                {
                    main = m;
                    secondary = s;
                }
            }

            protected internal Type type;
            protected internal List<Attack> attacks; //max 4


            public Monster(string n, int ht, int d, int a, int e, int l, Type t, List<Attack> att)
            {
                name = n;
                health = ht;
                healthTop = ht;
                defense = d;
                accuracy = a;
                experience = e;
                level = l;
                type = t;
                attacks = att;
            }

            void ScaleToLevel(int l)
            {

            }
            void TakeDamage(int d)
            {
                health = (d / 100) * (100 - defense);
                //write "took x damage"
            }

        }

        class Attack
        {
            protected internal string name;
            protected internal Types type;
            protected internal int damage, requiredAccuracy, heal, defenseBoost, accuracyBoost;
            //average damage = 15
            //average required accuracy = 70

            //monster rolls 1d100 + its accuracy, needs to be higher than req. accuracy
            public Attack(string n, Types t, int d, int ar, int h, int db, int ab)
            {
                name = n;
                type = t;
                damage = d;
                requiredAccuracy = ar;
                heal = h;
                defenseBoost = db;
                accuracyBoost = ab;
            }
        }


            class Player
        {
            protected internal string name;
            protected internal List<Monster> Roster;
            protected internal int defeatedMonsters;

            public Player(string n, List<Monster> r, int d)
            {
                name = n;
                Roster = r;
                defeatedMonsters = d;
            }
        }
    }


}
