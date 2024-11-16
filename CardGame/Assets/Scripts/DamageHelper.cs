using System;
using UnityEngine;

namespace MyGame
{
    public static class DamageHelper
    {
        public static Damage ConvertWarpToDamage(this CreateDamageWarp self, int targetAttack)
        {
            Damage damage = new Damage();
            int value = self.DamageType == EDamageType.Fixed ? self.FixedValue : Mathf.RoundToInt(self.Ratio * targetAttack);

            switch (self.DamageAttr)
            {
                case EDamageAttr.PhysicsDamage:
                    damage.Physical = value;
                    break;
                case EDamageAttr.MagicDamage:
                    damage.Magic = value;
                    break;
                case EDamageAttr.Health:
                    damage.Heal = value;
                    break;
            }

            return damage;
        }

        
        public static bool IsHealDamage(this DamageInfo self)
        {
            return self.Damage.Heal > 0 && self.Damage.Physical <= 0 && self.Damage.Magic <= 0;
        }
        
        public static void ApplyCriticalToFinalDamage(this DamageInfo self)
        {
            bool isCrit = UnityEngine.Random.Range(0.00f, 1.00f) <= self.CriticalRate;
            if (isCrit)
            {
                self.FinalDamage = self.Damage * 2f;
            }
            self.FinalDamage = self.Damage;
        }

        public static int CalFinalTotalDamage(this DamageInfo self)
        {
            float pyDmg = self.FinalDamage.Physical - self.FinalDamage.Physical * self.Defender.Property.PhysicalDefense;
            float mgDmg = self.FinalDamage.Magic - self.FinalDamage.Magic * self.Defender.Property.MagicDefense;
            return Mathf.RoundToInt(pyDmg + mgDmg - self.FinalDamage.Heal);
        }
    }
}