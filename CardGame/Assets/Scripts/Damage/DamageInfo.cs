using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DamageInfo
    {
        public HeroObj Attacker;
        
        public HeroObj Defender;

        public Damage Damage;

        public float CriticalRate;
        
        public Damage FinalDamage;

        public string BehurtEffect;

        public string Source;
        
        public DamageInfo(HeroObj attacker, HeroObj defender, Damage damage, float criticalRate,string behurtEffect,string source = "")
        {
            Attacker = attacker;
            Defender = defender;
            Damage = damage;
            CriticalRate = criticalRate;
            BehurtEffect = behurtEffect;
            Source = source;
        }
    }

    public struct Damage
    {
        /// <summary>
        /// 物理伤害
        /// </summary>
        private float _physical;

        public float Physical
        {
            get => _physical;
            set { _physical = Mathf.Max(0, value); }
        }

        /// <summary>
        /// 魔法伤害
        /// </summary>
        private float _magic;

        public float Magic
        {
            get => _magic;
            set { _magic = Mathf.Max(0, value); }
        }
        
        /// <summary>
        /// 生命回复
        /// </summary>
        private float _heal;

        public float Heal
        {
            get => _heal;
            set { _heal = Mathf.Max(0, value); }
        }


        public Damage(float physical, float magic,float heal)
        {
            _physical = physical;
            _magic = magic;
            _heal = heal;
        }

        // public static Damage operator +(Damage a, Damage b)
        // {
        //     return new Damage(a.Physical + b.Physical, a.Magic + b.Magic);
        // }
        //
        // public static Damage operator -(Damage a, Damage b)
        // {
        //     return new Damage(a.Physical - b.Physical, a.Magic - b.Magic);
        // }
        //
        public static Damage operator *(Damage a, float b)
        {
            return new Damage(Mathf.RoundToInt(a.Physical * b),
                Mathf.RoundToInt(a.Magic * b),a.Heal);
        }
    }
}