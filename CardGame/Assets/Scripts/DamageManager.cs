using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DamageManager : IPostGameService, IFixedUpdate
    {
        private readonly List<DamageInfo> _damageInfos = new List<DamageInfo>();

        public void Init()
        {
        }

        public void FixedUpdate(float deltaTime)
        {
            int i = 0;
            while (i < _damageInfos.Count)
            {
                DealWithDamage(_damageInfos[i]);
                _damageInfos.RemoveAt(0);
            }
        }

        private void DealWithDamage(DamageInfo dmgInfo)
        {
            if (!dmgInfo.Defender) return;
            if (dmgInfo.Defender.IsDead) return;
            if (!dmgInfo.Defender.ControlMod.CanBeHurt) return;
            
            // 先跑一遍攻击时可能会修改伤害的buff
            if (dmgInfo.Attacker)
            {
                for (int i = 0; i < dmgInfo.Attacker.BuffCom.Buffs.Count; i++)
                {
                    dmgInfo.Attacker.BuffCom.Buffs[i].ExecuteBuff(EBuffEventType.OnHit,dmgInfo.Attacker.BuffCom.Buffs[i], dmgInfo, dmgInfo.Defender);
                }
            }
            
            // 再跑一边敌人被攻击会修改伤害的buff
            for (int i = 0; i < dmgInfo.Defender.BuffCom.Buffs.Count; i++)
            {
                dmgInfo.Defender.BuffCom.Buffs[i].ExecuteBuff(EBuffEventType.OnBeHurt,dmgInfo.Defender.BuffCom.Buffs[i],dmgInfo, dmgInfo.Attacker);
            }
            
            dmgInfo.ApplyCriticalToFinalDamage();
            // 如果敌人会被这次伤害击杀，那么要执行攻击者的OnKill(比如击杀敌人会增加攻击力的buff)和被攻击者的OnBeKilled（比如被杀的时候可以复活）
            if (dmgInfo.Defender.CanBeKilledByDamageInfo(dmgInfo))
            {
                if (dmgInfo.Attacker)
                {
                    for (int i = 0; i < dmgInfo.Attacker.BuffCom.Buffs.Count; i++)
                    {
                        dmgInfo.Attacker.BuffCom.Buffs[i].ExecuteBuff(EBuffEventType.OnKill,dmgInfo.Attacker.BuffCom.Buffs[i], dmgInfo, dmgInfo.Defender);
                    }
                }

                for (int i = 0; i < dmgInfo.Defender.BuffCom.Buffs.Count; i++)
                {
                    dmgInfo.Defender.BuffCom.Buffs[i].ExecuteBuff(EBuffEventType.OnBeKilled,dmgInfo.Defender.BuffCom.Buffs[i], dmgInfo, dmgInfo.Attacker);
                }
            }

            // 死不了的受伤要播放受伤动画
            if (!dmgInfo.Defender.CanBeKilledByDamageInfo(dmgInfo) && dmgInfo.IsHealDamage())
            {
                dmgInfo.Defender.SpineCom.PlayAnim("Injured");
            }
            
            dmgInfo.Defender.ModifyHealth(dmgInfo);
        }

        public void AddDamage(HeroObj attacker, HeroObj defender, Damage damage, float criticalRate)
        {
            _damageInfos.Add(new DamageInfo(attacker, defender, damage, criticalRate));
        }
    }
}