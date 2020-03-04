using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class Warehouse
    {
        static Random rnd = new Random();
        static int[] products = new int[] { 1, 3, 5, 7, 9, 23, 39, 12, 39, 20, 13, 13, 14 };
        static float[] money_value = new float[] { 0.99f, 3.0f, 5.99f, 2.99f, 9.99f, 2.22f, 3.9f, 1.2f, 3.9f, 2.01f, 1.3f, 1.3f, 1.4f };
        static int[] warehouseItems = new int[] { 10, 20, 125, -2, 43, 10020, 232 };
        static int[] inventory = new int[20];
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
}
