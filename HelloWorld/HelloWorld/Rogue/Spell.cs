using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloNamespace.Rogue
{
    abstract class Spell
    {
        public int cost; //manacost
        public int range; //range from caster in tiles
        public bool aoe; //area of effect spell
        public int width; //width of aoe
        public bool circle; //true = circle, false = box;

        public int damage; //damage applied
        public int heal; //health healed
        abstract public void Cast(Tile caster, int cost);
    }

    #region Spells
    class Fireball : Spell
    {
        public override void Cast(Tile caster, int cost)
        {
            if (caster.player != null)
            {
                if (caster.player.UseMana(cost))
                {
                    //cast

                }
            }
        }
    }
    class Teleport : Spell
    {
        public override void Cast(Tile caster, int cost)
        {
            if (caster.player != null)
            {
                if (caster.player.UseMana(cost))
                {
                    //cast

                }
            }

        }
    }
    class Blessing : Spell
    {
        public override void Cast(Tile caster, int cost)
        {
            if (caster.player != null)
            {
                if (caster.player.UseMana(cost))
                {
                    //cast

                }
            }
        }
    }

    #endregion

}
