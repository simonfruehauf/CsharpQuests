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
        private int currentline = 0;
        Player currentplayer;
        int x;
        int y;
        bool debug = true;
        Random rnd = new Random();
        List<Monster> starters;
        List<Tuple<int, int>> AttackPos = new List<Tuple<int, int>>();
        int selectIndex;
        string arrow = "<--";
        string healthString = "█";
        Monster enemy;
        enum HitResult
        {
            hitdamage,
            hitdamageheal,
            hitheal,
            hitdamageacc,
            hitdamagedef,
            miss
        }

        public void Play()
        {

            Console.Clear();
            DrawScreen(true);





            currentline = 5;
            WriteText("Hello.", true, currentline, false);
            WriteText("Welcome to Cisharpmon.", true, currentline, false);
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
                    currentplayer = LoadPlayer(saveinfo);
                    currentplayer.activeMonster = currentplayer.Roster[0];
                    enemy = RandomMonster();
                    DeleteLinesUp(3, true);
                    Combat();
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
        void Combat()
        {
            int turn = 0;
            bool endcombat = false;
            while (!endcombat)
            {
                turn++;
                DrawScreen();
                DrawHealth(currentplayer.activeMonster, false);
                DrawHealth(enemy, true);
                DrawAttacks(currentplayer.activeMonster);
                Attack a = AttackSelector();
                ClearAttackScreen();
                WriteUsingAttack(currentplayer.activeMonster, a);
                WaitEnter();
                WriteResult(currentplayer.activeMonster.TryAttack(a, enemy), currentplayer.activeMonster);
                WaitEnter();
               
                a = enemy.SelectAttack();
                ClearAttackScreen();

                WriteUsingAttack(enemy, a, true);
                WaitEnter();
                WriteResult(enemy.TryAttack(a, currentplayer.activeMonster), enemy);
                WaitEnter();
                ClearAttackScreen();
                if (enemy.health <= 0 || currentplayer.activeMonster.health <= 0)
                {
                    endcombat = true;
                }
            }
            if (currentplayer.activeMonster.health <= 0)
            {
                ClearAttackScreen();
                Console.SetCursorPosition(3, AttackSpace(2));
                WriteText("Your cisharpmon died...", false, 0, false);
            }
            else
            {
                ClearAttackScreen();
                Console.SetCursorPosition(3, AttackSpace(2));
                WriteText("You defeated the enemy and your monster gained some experience!", true, 0, false);
                currentplayer.defeatedMonsters++;
                SaveMonster(currentplayer.activeMonster);
                Save(currentplayer);
                System.Threading.Thread.Sleep(3000);
            #region fightagain
                retrying:
                WriteText("Would you like to fight again? Y/N", true, currentline, true, 100);

                retryinput:
                ConsoleKey input = Console.ReadKey(true).Key;
                switch (input)
                {
                    case ConsoleKey.Y:
                        enemy = RandomMonster();
                        enemy.ScaleToLevel(currentplayer.activeMonster.level);
                        Console.Clear();
                        Combat();
                        break;
                    case ConsoleKey.N:
                        Console.Clear();
                        break;
                    default:
                        goto retryinput;
                }
                #endregion
            }
        }
        void WaitEnter()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            switch (input.Key)
            {
                case ConsoleKey.Enter:
                    break;
                default:
                    WaitEnter();
                    break;
            }
        }
        void WriteUsingAttack(Monster m, Attack a, bool isenemy = false)
        {
            string text = "";
            if (isenemy)
            {
                text = "The enemy ";
            }
            text += m.name + " is trying to use " + a.name + "!";
            ClearAttackScreen();
            Console.SetCursorPosition(3, AttackSpace(2));
            WriteText(text, false, 0, false);
        }
        void WriteResult(HitResult r, Monster m)
        {
            DrawHealth(currentplayer.activeMonster, false);
            DrawHealth(enemy, true);
            string text = "Nothing happened";
            ClearAttackScreen();
            Console.SetCursorPosition(3, AttackSpace(2));
            switch (r)
            {
                case HitResult.hitdamage:
                    text = m.name + " hit the attack and dealt some damage!";
                    break;
                case HitResult.hitdamageheal:
                    text = m.name + " healed itself and dealt damage at the same time!";
                    break;
                case HitResult.hitheal:
                    text = m.name + " played it safe and recovered some health.";
                    break;
                case HitResult.hitdamageacc:
                    text = m.name + " dealt damage and boosted its accuracy!";
                    break;
                case HitResult.hitdamagedef:
                    text = m.name + " dealt damage and bolstered its defense!";
                    break;
                case HitResult.miss:
                    text = m.name + " failed to attack...";
                    break;
                default:
                    break;
            }
            WriteText(text, false, 0, false);
        }
        void DrawHealth(Monster m, bool enemy)
        {
            int barlength = 30;
            string hlong = m.health + " / " + m.healthTop;

            if (enemy) //then set cursor top left
            {
                hlong = hlong + "   " + m.name + " LVL " + m.level;
                Console.SetCursorPosition(4, 3);
                Console.Write("                 ");
                Console.SetCursorPosition(4, 3);
            }
            else //bottom right
            {
                hlong = "LVL " + m.level + " "+ m.name + "   " + hlong;

                Console.SetCursorPosition(x - 1 - hlong.Length - 10, AttackSpace(-3));
                Console.Write("                  ");

                Console.SetCursorPosition(x-1-hlong.Length - 3, AttackSpace(-3));
            }
            Console.Write(hlong);


            if (enemy) //then set cursor top left
            {
                Console.SetCursorPosition(2, 2);
            }
            else //bottom right
            {
                Console.SetCursorPosition(x-1- barlength - 3, AttackSpace(-2));
            }
            
            //20 length, left to right. calculate missing health, draw empty, then draw full
                       
            int missing = m.healthTop - m.health;
            missing = missing * barlength / m.healthTop; //normalize to 0-20 range, missing out of 20 are empty
            Console.Write("|");
            if (enemy)
            {
                missing = barlength - missing;
            }
            for (int i = 0; i < barlength; i++)
            {
                if (enemy)
                {
                    if (i > missing)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(healthString);
                        Console.ResetColor();
                    }
                }
                else
                {
                    if (i < missing)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(healthString);
                        Console.ResetColor();
                    }
                }
            }
            Console.Write("|");
        }
        void FightMonster(Monster m)
        {
            Console.Clear();
            DrawScreen(true);

            x = Console.WindowWidth;  //120, attacks are from 0+3 to x-3
            y = Console.WindowHeight; //30, inventory is from y-1 to y/6+1
        }

        void ClearAttackScreen()
        {
            int size = (y - 1) - (((y / 3) * 2 + 1)); // in our case 8
            for (int i = 0; i < size; i++)
            {
                Console.SetCursorPosition(1, ((y / 3) * 2) + 1 +i);
                for (int j = 0; j < x-2; j++)
                {
                    Console.Write(" ");

                }
            }
        }
        Attack DrawAttacks(Monster m)
        {
            x = Console.WindowWidth;  //120, attacks are from 0+3 to x-3
            y = Console.WindowHeight; //30, inventory is from y-1 to ((y/3)*2)+1, in this case 29 to 19

            //int size = (y - 1) - (((y / 3)*2 + 1)); // in our case 8

            //1
            //2                             Attacks
            //3 Text                                                    Text
            //4 Text                                                    Text
            //5
            //6 Text                                                    Text
            //7 Text                                                    Text
            //8


            //careful, hardcoded numbers following
            Console.SetCursorPosition(GetMiddle("Attacks", x), AttackSpace(2));
            Console.Write("Attacks:");
            int a = 0;
            AttackPos.Clear();
            foreach (Attack attack in m.attacks)
            {
                switch (a)
                {
                    case 0:
                        Console.SetCursorPosition(GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(3));
                        break;
                    case 1:
                        Console.SetCursorPosition(GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(6));
                        break;
                    case 2:
                        Console.SetCursorPosition(x/2+ GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(3));
                        break;
                    case 3:
                        Console.SetCursorPosition(x/2+ GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(6));
                        break;
                    default:
                        break;
                }
                Console.Write(attack.name);
                AttackPos.Add(new Tuple<int, int>(Console.CursorLeft + 1, Console.CursorTop));
                switch (a)
                {
                    case 0:
                        Console.SetCursorPosition(GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(4));
                        break;
                    case 1:
                        Console.SetCursorPosition(GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(7));
                        break;
                    case 2:
                        Console.SetCursorPosition(x / 2 + GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(4));
                        break;
                    case 3:
                        Console.SetCursorPosition(x / 2 + GetMiddle("", GetMiddle("", GetMiddle("", x))), AttackSpace(7));
                        break;
                    default:
                        break;
                }

                //switch (rnd.Next(0, 6))
                //{
                //    case 1:
                //        healing = damage;
                //        damage = 0;
                //        break;
                //    case 2:
                //        healing = damage / rnd.Next(2, 3);
                //        damage = damage / 2;
                //        break;
                //    case 3:
                //        damage = (damage * 2) / 3;
                //        accuracyboost = rnd.Next(5, 10);
                //        break;
                //    case 4:
                //        damage = rnd.Next(damage / 3, damage);
                //        defenseboost = rnd.Next(5, 10);
                //        break;
                //    default:
                //        break;
                //}

                if (attack.damage == 0  && attack.heal != 0)
                {
                    Console.Write(UppercaseFirst(attack.type.ToString()) + " type healing move.");
                }
                else if (attack.damage != 0 && attack.heal != 0)
                {
                    Console.Write(UppercaseFirst(attack.type.ToString()) + " type attack, also heals.");
                }
                else if (attack.damage != 0 && attack.accuracyBoost != 0)
                {
                    Console.Write(UppercaseFirst(attack.type.ToString()) + " type attack. Raises accuracy.");
                }
                else if (attack.damage != 0 && attack.defenseBoost != 0)
                {
                    Console.Write(UppercaseFirst(attack.type.ToString()) + " type attack. Raises defense.");
                }
                else 
                {
                    Console.Write(UppercaseFirst(attack.type.ToString())+ " type attack.");
                }
                a++;
            }
            return AttackSelector();
        }
        int AttackSpace(int linesdown)
        {
            return (((y / 3) * 2) + linesdown /*x lines down*/);
        }


        Attack AttackSelector()
        {

            bool exit = false;
            do
            {
                Console.SetCursorPosition(AttackPos[selectIndex].Item1, AttackPos[selectIndex].Item2);
                Console.Write(arrow);

                ConsoleKeyInfo input = Console.ReadKey(true);
                Console.SetCursorPosition(AttackPos[selectIndex].Item1, AttackPos[selectIndex].Item2);
                Console.Write("   ");
                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                    case ConsoleKey.UpArrow:
                        selectIndex = ((selectIndex - 1) % 4 + 4) % 4; // keeps it between 0 and 3 and not -3 and 3
                        break;
                    case ConsoleKey.DownArrow:
                        selectIndex = (selectIndex + 1) % 4;
                        break;
                    default:
                        break;
                }
                //redraw arrow at index
                Console.SetCursorPosition(AttackPos[selectIndex].Item1, AttackPos[selectIndex].Item2);
                Console.Write(arrow);

            } while (!exit);

            Attack a = SelectAttack(currentplayer.activeMonster);
            return a;
            
        }
        Monster SelectMonster()
        {
            return null;
        }

        Attack SelectAttack(Monster m, int index = -1)
        {
            if (m == null)
            {
                return null;
            }
            Attack a; 
            if (index == -1)
            {
                try
                {
                  a = m.attacks[selectIndex];
                }
                catch (Exception)
                {
                    a = null;
                }
            }
            else
            {
                try
                {
                    a = m.attacks[index];

                }
                catch (Exception)
                {
                    a = null;
                }
            }
            return a;
        }


        public void WriteText(string text, bool middle, int line, bool newLine, int sleep = 100, bool increaseline = true)
        {
            if (middle)
            {
                Console.SetCursorPosition(GetMiddle(text, x - 2), line);
            }
            Write.TypeLine(text);
            System.Threading.Thread.Sleep(sleep);
            if (newLine)
            {
                Console.WriteLine();
            }
            if (increaseline)
            {
                currentline++;
            }
        }
        void NewPlayer()
        {
            WriteText("It seems you are new...", true, currentline, true, 100);
            System.Threading.Thread.Sleep(200);
            WriteText("Creating a new save file for you...", true, currentline, true, 100);
            System.Threading.Thread.Sleep(200);
            bool retry = false;
        retrying:
            WriteText(retry ? "Well, what is your name then?" : "What's your name?", true, currentline, true, 100);

            Console.SetCursorPosition(GetMiddle("What's your name?", x - 2), 10);
            string name = Console.ReadLine();
            currentline++;

            WriteText("Ah, " + name + ", is that correct? Y/N", true, currentline, true, 100);

        retryinput:
            ConsoleKey input = Console.ReadKey(true).Key;
            switch (input)
            {
                case ConsoleKey.Y:
                    break;
                case ConsoleKey.N:
                    //delete previous lines
                    DeleteLinesUp(3, true);
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
        void DeleteLinesUp(int ex, bool useline)
        {
            int cl = currentline;
            for (int i = cl; i > cl - ex; i--)
            {
                Console.SetCursorPosition(2, currentline);
                for (int j = 1; j < x - 2; j++)
                {
                    Console.Write(" ");
                }
                if (useline)
                {
                    currentline--;
                }
            }
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



            return new Monster(name, rnd.Next(40, 60), rnd.Next(0, 6), rnd.Next(45, 70), 0, 1, new Monster.Type(t, t2), attacks);
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
        void DrawScreen(bool drawattacks = false)
        {

            #region border
            x = Console.WindowWidth;  //120
            y = Console.WindowHeight; //30, inventory is from y-1 to y/6+1

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
                    if ((j == y/3*2 && drawattacks) && i != x && j != y)
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
            m.health = m.healthTop;
            ReadMonsterFile();
            string monstervalues = m.name + stop + m.healthTop + stop + m.defense + stop + m.accuracy + stop + m.experience + stop + m.level + stop + m.type.main + stop + m.type.secondary;

            int counter = m.attacks.Count();
            for (int i = 0; i < counter; i++)
            {
                monstervalues += stop + m.attacks[i].name + stop + m.attacks[i].type + stop + m.attacks[i].damage + stop + m.attacks[i].requiredAccuracy + stop + m.attacks[i].heal + stop + m.attacks[i].defenseBoost + stop + m.attacks[i].accuracyBoost;
            }


            string[] lines = File.ReadAllLines(folder + "\\" + monsterfile);
            int c = 1; //1 based indexing if reading a file line for some reason???
            foreach (string item in lines)
            {
                if (item.Contains(m.name))
                {
                    // Write the new data over old data
                    using (StreamWriter writer = new StreamWriter(folder + "\\" + monsterfile))
                    {
                        for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                        {
                            if (currentLine == c)
                            {
                                writer.WriteLine(monstervalues);
                            }
                            else
                            {
                                writer.WriteLine(lines[currentLine - 1]);
                            }
                        }
                    }
                    return; //overrode the monster
                }
                c++;
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
            float damagemod = 1;
            //assuming level 1
            //health:   ~100
            //defense:  ~5, in % reduction
            //accuracy: ~50
            protected internal Boost tempDefense, tempAccuracy;

            protected internal struct Boost
                {
                public int strength;
                public int remainingturns;
                public Boost(int s, int t)
                {
                    strength = s;
                    remainingturns = t;
                }
                };
            protected internal int experience;
            float xp;
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


            public Monster(string n, int ht, int def, int acc, int e, int l, Type t, List<Attack> att)
            {
                name = n;
                health = ht;
                healthTop = ht;
                defense = def;
                accuracy = acc;
                experience = e;
                level = l;
                type = t;
                attacks = att;
                xp = 100;
            }

            public void ScaleToLevel(int l)
            {
                float scaler = (l / 50) + 1;

                defense = (int)(defense * scaler);
                healthTop = (int)(healthTop * scaler);
                accuracy += 5;
                damagemod = scaler;
            }
            public void CheckTemps()
            {
                tempAccuracy.remainingturns--;
                tempDefense.remainingturns--;

                if (tempAccuracy.remainingturns <= 0)
                {
                    tempAccuracy.strength = 0;
                }
                if (tempDefense.remainingturns <= 0)
                {
                    tempDefense.strength = 0;
                }
            }
            public void TakeDamage(float d, Monster attacker)
            {
                health -= (int)((d / 100f) * (100f - (float)defense - (float)tempDefense.strength));
                if (health <= 0)
                {
                    Die(attacker);
                }
                //write "took x damage"
            }
            public void Heal(float h)
            {
                health = (int)(health + h);
                if (health > healthTop)
                {
                    health = healthTop;
                }
            }
            void Die(Monster killer)
            {
                killer.gainXP(level * 5);
            }

            public HitResult TryAttack(Attack a, Monster t)
            {
                HitResult value;
                Random r = new Random();
                int chance = r.Next(0, 50);
                if (chance + accuracy + tempAccuracy.strength > a.requiredAccuracy)
                {
                    if (a.damage != 0 && a.heal != 0)
                    {
                        t.TakeDamage(a.damage * damagemod, this);
                        Heal(a.heal * damagemod);
                        value = HitResult.hitdamageheal;
                    }
                    else if (a.damage == 0 && a.heal != 0)
                    {
                        Heal(a.heal * damagemod);
                        value = HitResult.hitheal;
                    }
                    else
                    {
                        t.TakeDamage(a.damage * damagemod, this);
                        value = HitResult.hitdamage;
                    }
                    if (a.accuracyBoost != 0)
                    {
                        tempAccuracy = new Monster.Boost((int)(a.accuracyBoost *damagemod), 1);
                        value = HitResult.hitdamageacc;

                    }
                    if (a.defenseBoost != 0)
                    {
                        tempDefense = new Monster.Boost((int)(a.defenseBoost * damagemod), 1);
                        value = HitResult.hitdamagedef;
                    }

                }
                else
                {
                    value = HitResult.miss;
                }
                return value;
            }
            public Attack SelectAttack()
            {
                Random r = new Random();
                bool canheal;
                List<Attack> healingattacks = new List<Attack>();
                foreach (Attack item in attacks)
                {
                    if (item.heal > 0)
                    {
                        healingattacks.Add(item);
                    }
                }
                if (healingattacks.Count > 0)
                {
                    canheal = true;
                }
                else
                {
                    canheal = false;
                }
                int healchance = 10;
                if (health < healthTop/3)
                {
                    healchance = 75;
                }
                else if (health < health/2)
                {
                    healchance = 40;
                }
                else if(health == healthTop)
                {
                    healchance = 0;
                }

                if (canheal)
                {
                    if (r.Next(0, 100) < healchance)
                    {
                        return healingattacks[r.Next(0, healingattacks.Count - 1)];
                    }
                    else
                    {
                        return attacks[r.Next(0, attacks.Count - 1)];
                    }
                }
                else
                {
                    return attacks[r.Next(0, attacks.Count - 1)];
                }
            }

            public void gainXP(int x)
            {
                health = healthTop;
                experience += x;
                if (experience > xp)
                {
                    xp *= 1.3f;
                    experience = 0;
                    level++;
                    ScaleToLevel(level);
                }
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
            protected internal Monster activeMonster;

            public Player(string n, List<Monster> r, int d)
            {
                name = n;
                Roster = r;
                defeatedMonsters = d;
            }
        }
    }


}
