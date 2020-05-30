using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HelloNamespace
{
    public class Warehouse
    {
        public Dictionary<string, float> Inventory = new Dictionary<string, float>(){
            {"bow", 2f},
            {"dagger", 1f},
            {"sword", 4f},
            {"crossbow", 3f}
        };

        static Random rnd = new Random();
        static int[] products = new int[] { 1, 3, 5, 7, 9, 23, 39, 12, 39, 20, 13, 13, 14 };
        static float[] money_value = new float[] { 0.99f, 3.0f, 5.99f, 2.99f, 9.99f, 2.22f, 3.9f, 1.2f, 3.9f, 2.01f, 1.3f, 1.3f, 1.4f };
        static int[] warehouseItems = new int[] { 10, 20, 125, -2, 43, 10020, 232 };
        static int[] inventory = new int[20];
        static int[] values = new int[20];
        static string file = "files\\warehouse.txt";
        public static void MainWarehouse()
        {
            int a = 0;
            DisplayAmountOfProducts(products);
            foreach (int item in products)
            {
                DisplayItemValue(products, money_value, a);
                a++;
            }
            DisplayTotalValue(products, money_value);
            FindHighestOfArray(warehouseItems);

            ReadFile();
            Console.ReadKey();
        }
        public static void LookAtInventory(Dictionary<string, float> Inv)
        {
            Console.WriteLine("These items are the ones that are available:");
            foreach (KeyValuePair<string, float> i in Inv)
            {
                Console.WriteLine(i.Key);
            }
            string input = Read.String("What would you like to look at?");

            if (Inv.ContainsKey(input))
            {
                Console.WriteLine("The " + input + " is worth " + Inv[input] + " schmeckles.");
            }
            else
            {
                Console.WriteLine("Sorry, we don't have " + input + "s.");
            }
        }

        public static void ReadFile(string directoryfile = "files\\warehouse.txt")
        {
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
                Console.WriteLine("Missing Directory, now created. Cannot read file.");
                var t_file = File.Create(file);
                t_file.Close();
            }
            else
            {
                if (!File.Exists(file))
                {
                    var t_file = File.Create(file);
                    t_file.Close();
                    Console.WriteLine("Missing File, now created.");
                }
                else
                {
                    if (new FileInfo(file).Length == 0)
                    {
                        Console.WriteLine("Emtpy file.");
                    }
                    else
                    { 
                        //main code
                        string t = File.ReadLines(file).Skip(0).Take(1).First();
                        string[] t_string = t.Split(' ');
                        List<int> v = new List<int>(); //item
                        List<int> w = new List<int>(); //value
                        foreach (string item in t_string)
                        {
                            v.Add(Convert.ToInt32(item.Split(',')[0]));
                            w.Add(Convert.ToInt32(item.Split(',')[1]));
                        }
                        inventory = v.ToArray();
                        values = w.ToArray();
                    }

                }
            }

        }

        public static void DisplayAmountOfProducts(int[] products)
        {
            Console.WriteLine("There are " + products.Length + " items in the warehouse.");
        }
        public static void DisplayItemValue(int[] products, float[] money_value, int index)
        {
            Console.WriteLine("Item #" + products[index] + " is worth " + money_value[index] + ".");
        }
        public static void DisplayTotalValue(int[] products, float[] money_value)
        {
            int index = 0;
            float totalValue = 0;
            foreach (int item in products)
            {
                totalValue += money_value[index];
                index++;
            }
            Console.WriteLine("The value of the warehouse inventory is " + totalValue + ".");
        }
        public static void FindHighestOfArray(int[] m_array)
        {
            int highestValue = m_array.Max();
            int index = m_array.ToList().IndexOf(highestValue);
            Console.WriteLine("The highest value in the array is " + highestValue + " at the index #" + index); //starting from 0
        }
        public static void PopulateArrayRandom(out int[] m_array, int length = 20)
        {
            m_array = new int[length];
            for (int i = 0; i < length; i++)
            {
                m_array[i] = rnd.Next(1, 3000);
            }
        }
        public static void PopulateArray(out int[] m_array, int[] other_array)
        {
            m_array = new int[other_array.Length];
            for (int i = 0; i < other_array.Length; i++)
            {
                m_array[i] = other_array[i];
            }
        }
    }
    public class Sorter
    {
        public static int[] items;
        public static int[] values;
        static string file = "files\\sorter.txt";

        public static int ReadFile(int min = 20)
        {
            items = null;
            values = null;
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
                Console.WriteLine("Missing Directory, now created. Cannot read file.");
                var t_file = File.Create(file);
                t_file.Close();
                return 1;
            }
            else
            {
                if (!File.Exists(file))
                {
                    var t_file = File.Create(file);
                    t_file.Close();
                    Console.WriteLine("Missing File, now created.");
                    return 2;
                }
                else
                {
                    if (new FileInfo(file).Length == 0)
                    {
                        Random rnd = new Random();
                        Console.WriteLine("Emtpy file. Filling file with random ints.");
                        items = new int[min];
                        values = new int[min];
                        for (int i = 0; i < min; i++)
                        {
                            items[i] = rnd.Next(1, 100);
                        }
                        for (int j = 0; j < min; j++)
                        {
                            values[j] = rnd.Next(1, 100);
                        }
                        WriteFile();
                        return 3;

                    }
                    else
                    {
                        
                        string t = File.ReadAllLines(file).Skip(0).Take(1).First();
                        string[] t_string = t.Split('.');
                        List<int> v = new List<int>(); //item
                        List<int> w = new List<int>(); //value
                        foreach (string item in t_string)
                        {
                            v.Add(Convert.ToInt32(item.Split(',')[0]));
                            w.Add(Convert.ToInt32(item.Split(',')[1]));
                        }
                        items = v.ToArray();
                        values = w.ToArray();
                        return 0;
                    }

                }
            }
        }

        public static void WriteFile()
        {
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
                Console.WriteLine("Missing Directory, now created. Cannot read file.");
                var t_file = File.Create(file);
                t_file.Close();
            }
            else
            {
                if (!File.Exists(file))
                {
                    var t_file = File.Create(file);
                    t_file.Close();
                    Console.WriteLine("Missing File, now created.");
                }
                else if (items != null && values != null)
                {
                    if (items.Length == values.Length)
                    {
                        string text = "";
                        for (int i = 0; i < items.Length; i++)
                        {
                            if (i == items.Length-1)
                            {
                                text += items[i].ToString() + "," + values[i].ToString();
                            }
                            else
                                    {
                                text += items[i].ToString() + "," + values[i].ToString() + ".";
                            }
                        }
                        File.WriteAllText(file, text);
                        Console.WriteLine("Written down.");
                    }
                    else
                    {
                        Console.WriteLine("We are missing some values or items. Please check again.");
                    }
                }

            }
        }

        public static void Sort(int[] array_a, int[] array_b, bool reverse = false, bool write = true)
        {
            int[] temp_a = array_a;
            int[] temp_b = array_b;
            if (reverse)
            {
                Array.Sort(temp_a, array_b);
                Array.Reverse(array_b);
                Array.Sort(temp_a, array_a);
                Array.Reverse(array_a);

            }
            else
            {
                Array.Sort(temp_a, array_b);
                Array.Sort(temp_a, array_a);
            }
            if (write)
            {
                WriteFile();
            }
            //
        }
    }
}
