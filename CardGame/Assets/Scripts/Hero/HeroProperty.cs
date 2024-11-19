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

        public float HpRatio;

        public int TotalHp
        {
            get
            {
                return Mathf.RoundToInt(Hp + Hp * HpRatio);
            }
        }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack;

        public float AttackRatio;

        public int TotalAttack
        {
            get
            {
                return Mathf.RoundToInt(Attack + Attack * AttackRatio);
            }
        }
        
        /// <summary>
        /// 物理防御力
        /// </summary>
        public int PhysicalDefense;
        
        public float PhysicalDefenseRatio;

        public int TotalPhysicalDefense
        {
            get
            {
                return Mathf.RoundToInt(PhysicalDefense + PhysicalDefense * PhysicalDefenseRatio);
            }
        }
        
        /// <summary>
        /// 魔法防御力
        /// </summary>
        public int MagicDefense;
        
        public float MagicDefenseRatio;

        public int TotalMagicDefense
        {
            get
            {
                return Mathf.RoundToInt(MagicDefense + MagicDefense * MagicDefenseRatio);
            }
        }
        
        /// <summary>
        /// 暴击率
        /// </summary>
        public float CriticalRate;
        
        /// <summary>
        /// 护盾值
        /// </summary>
        public int Shield;
        
        
        public static HeroProperty Empty = new HeroProperty(0,0,0,0,0,0,0,0,0,0); 

        public HeroProperty(int hp, float hpRatio, int attack, float attackRatio, int physicalDefense, float physicalDefenseRatio, int magicDefense,
            float magicDefenseRatio, float criticalRate, int shield)
        {
            Hp = hp;
            HpRatio = hpRatio;
            Attack = attack;
            AttackRatio = attackRatio;
            PhysicalDefense = physicalDefense;
            PhysicalDefenseRatio = physicalDefenseRatio;
            MagicDefense = magicDefense;
            MagicDefenseRatio = magicDefenseRatio;
            CriticalRate = criticalRate;
            Shield = shield;
        }


        public HeroProperty(int hp, int attack, int physicalDefense, int magicDefense, float criticalRate, int shield)
        {
            Hp = hp;
            Attack = attack;
            PhysicalDefense = physicalDefense;
            MagicDefense = magicDefense;
            CriticalRate = criticalRate;
            Shield = shield;

            HpRatio = 0;
            AttackRatio = 0;
            PhysicalDefenseRatio = 0;
            MagicDefenseRatio = 0;
            
        }

        public static HeroProperty operator +(HeroProperty a, HeroProperty b)
        {
            
            return new HeroProperty(
                a.Hp + b.Hp,
                a.HpRatio+ b.HpRatio,
                a.Attack+b.Attack,
                a.AttackRatio + b.AttackRatio,
                a.PhysicalDefense + b.PhysicalDefense,
                a.PhysicalDefenseRatio + b.PhysicalDefenseRatio,
                a.MagicDefense + b.MagicDefense,
                a.MagicDefenseRatio + b.MagicDefenseRatio,
                a.CriticalRate + b.CriticalRate,
                a.Shield + b.Shield
                );
        }
        
        public static HeroProperty operator -(HeroProperty a, HeroProperty b)
        {
            return new HeroProperty(
                a.Hp - b.Hp,
                a.HpRatio- b.HpRatio,
                a.Attack-b.Attack,
                a.AttackRatio - b.AttackRatio,
                a.PhysicalDefense - b.PhysicalDefense,
                a.PhysicalDefenseRatio - b.PhysicalDefenseRatio,
                a.MagicDefense - b.MagicDefense,
                a.MagicDefenseRatio - b.MagicDefenseRatio,
                a.CriticalRate - b.CriticalRate,
                a.Shield - b.Shield
            );
        }
    }

    public struct HeroHealth
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

        public HeroHealth(int hp, int shield)
        {
            HP = hp;
            Shield = shield;
        }
        
        
        public static HeroHealth operator +(HeroHealth a, CostResource b)
        {
            return new HeroHealth(a.HP + b.HP,a.Shield + b.Shield);
        }
        
        public static HeroHealth operator -(HeroHealth a, CostResource b)
        {
            return new HeroHealth(a.HP - b.HP,a.Shield - b.Shield);
        }
        
        public static HeroHealth operator +(HeroHealth a, HeroHealth b)
        {
            return new HeroHealth(a.HP + b.HP,a.Shield + b.Shield);
        }
        
        public static HeroHealth operator -(HeroHealth a, HeroHealth b)
        {
            return new HeroHealth(a.HP - b.HP,a.Shield - b.Shield);
        }
    }
}