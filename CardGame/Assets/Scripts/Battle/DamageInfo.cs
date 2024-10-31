using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DamageInfo
    {
        public HeroObj Attacker;
        
        public HeroObj Defender;

        public DamageInfoTag[] Tags;

        public Damage Damage;

        public float CriticalRate;

        public float HitRate;
        
        public List<AddBuffInfo> Buffs = new List<AddBuffInfo>();

        public DamageInfo(HeroObj attacker, HeroObj defender, DamageInfoTag[] tags, Damage damage, float criticalRate, float hitRate)
        {
            Attacker = attacker;
            Defender = defender;
            Damage = damage;
            CriticalRate = criticalRate;
            HitRate = hitRate;
            Tags = new DamageInfoTag[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                Tags[i] = tags[i];
            }
        }
    }

    public struct Damage
    {
        /// <summary>
        /// 真实伤害
        /// </summary>
        private int _real;
        public int Real
        {
            get => _real;
            set
            {
                _real = Mathf.Max(0, value);
            }
        }
        
        /// <summary>
        /// 物理伤害
        /// </summary>
        private int _physical;
        public int Physical
        {
            get => _physical;
            set
            {
                _physical = Mathf.Max(0, value);
            }
        }

        /// <summary>
        /// 魔法伤害
        /// </summary>
        private int _magic;
        public int Magic
        {
            get => _magic;
            set
            {
                _magic = Mathf.Max(0, value);
            }
        }

        public Damage(int real, int physical, int magic)
        {
            _real = real;
            _physical = physical;
            _magic = magic;
        }

        public static Damage operator +(Damage a, Damage b)
        {
            return new Damage(a.Real + b.Real, a.Physical + b.Physical, a.Magic + b.Magic);
        }

        public static Damage operator -(Damage a, Damage b)
        {
            return new Damage(a.Real - b.Real, a.Physical - b.Physical, a.Magic - b.Magic);
        }
    }

    public enum DamageInfoTag
    {
        /// <summary>
        /// 直接伤害
        /// </summary>
        DirectDamage = 0,
        
        /// <summary>
        /// 间歇性伤害
        /// </summary>
        PeriodDamage = 1,
        
        /// <summary>
        /// 反伤
        /// </summary>
        ReflectDamage = 2,
        
        /// <summary>
        /// 直接治疗
        /// </summary>
        DirectHeal = 10,
        
        /// <summary>
        /// 间歇性治疗
        /// </summary>
        PeriodHeal = 11,
    }
    
}