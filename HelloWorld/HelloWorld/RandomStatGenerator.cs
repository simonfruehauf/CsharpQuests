using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class RandomStatGenerator
    {
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        int power;

        struct Stats
        {
            //can go from 0 to 10
            int gumption, chutzpah, moxy, childlike_wonder, a_certain_je_ne_sais_quoi;
        }
        void GenerateStats()
        {
            int amount_stats = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Stats));
            for (int i = 0; i < amount_stats; i++)
            {
                power += RollDX(1, 5);
                power += RollDX(1, 5);
            }
        }
        int RollDX(int dice_max)
        {
            Random diceRND = new Random(Guid.NewGuid().GetHashCode());
            return rnd.Next(1, dice_max);

        }
        int RollDX(int dice_min, int dice_max)
        {
            Random diceRND = new Random(Guid.NewGuid().GetHashCode());
            return rnd.Next(dice_min, dice_max);

        }
    }
}
