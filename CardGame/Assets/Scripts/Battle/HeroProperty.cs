using Spine;
using UnityEngine;

namespace MyGame
{
    public struct HeroProperty
    {
        /// <summary>
        /// 生命值
        /// </summary>
        public int Hp;

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack;
        
        /// <summary>
        /// 物理防御力
        /// </summary>
        public int PhysicalDefense;
        
        /// <summary>
        /// 魔法防御力
        /// </summary>
        public int MagicDefense;
        
        public static HeroProperty Default = new HeroProperty(0, 0, 0,0);

        public HeroProperty(int hp, int attack, int physicalDefense, int magicDefense)
        {
            Hp = hp;
            Attack = attack;
            PhysicalDefense = physicalDefense;
            MagicDefense = magicDefense;
        }

        public static HeroProperty operator +(HeroProperty a, HeroProperty b)
        {
            return new HeroProperty(a.Hp + b.Hp,a.Attack + b.Attack,a.PhysicalDefense + b.PhysicalDefense,a.MagicDefense + b.MagicDefense);
        }
        
        public static HeroProperty operator -(HeroProperty a, HeroProperty b)
        {
            return new HeroProperty(a.Hp - b.Hp,a.Attack - b.Attack,a.PhysicalDefense - b.PhysicalDefense,a.MagicDefense - b.MagicDefense);
        }
    }

    public class HeroResource
    {
        public int HP;

        public int Shield;
        public bool Enough(CostResource requireResource)
        {
            return HP >= requireResource.HP && 
                   Shield >= requireResource.Shield;
        }
        
        public bool Enough(CastCondition condition)
        {
            return HP >= condition.HP && 
                   Shield >= condition.Shield;
        }

        public HeroResource(int hp, int shield)
        {
            HP = hp;
            Shield = shield;
        }
        
        public static HeroResource operator +(HeroResource a, CostResource b)
        {
            return new HeroResource(a.HP + b.HP,a.Shield + b.Shield);
        }
        
        public static HeroResource operator -(HeroResource a, CostResource b)
        {
            return new HeroResource(a.HP - b.HP,a.Shield - b.Shield);
        }
    }
}