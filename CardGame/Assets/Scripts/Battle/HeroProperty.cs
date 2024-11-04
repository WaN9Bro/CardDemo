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
        public int Hp;

        public int Shield;

        public HeroResource(int hp, int shield)
        {
            Hp = hp;
            Shield = shield;
        }

        public bool Enought(HeroResource requireResource)
        {
            return Hp >= requireResource.Hp &&
                   Shield >= requireResource.Shield;
        }
        
        public static HeroResource Empty = new HeroResource(0, 0);
        
        public static HeroResource Normal = new HeroResource(0, 0);

        public static HeroResource operator +(HeroResource a, HeroResource b)
        {
            return new HeroResource(a.Hp + b.Hp,a.Shield + b.Shield);
        }
        
        public static HeroResource operator -(HeroResource a, HeroResource b)
        {
            return new HeroResource(a.Hp - b.Hp,a.Shield - b.Shield);
        }

        public static HeroResource operator *(float a, HeroResource b)
        {
            return new HeroResource(Mathf.FloorToInt(b.Hp * a), Mathf.FloorToInt(b.Shield * a));
        }

 
    }

    public struct SkillCastCondition
    {
        public int FirstRound;
        public int CdRound;

        public static SkillCastCondition Default = new SkillCastCondition(1, 2);

        public SkillCastCondition(int firstRound, int cdRound)
        {
            FirstRound = firstRound;
            CdRound = cdRound;
        }
    }
}