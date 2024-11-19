using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyGame
{
    public class HeroObj : MonoBehaviour
    {
        public Grid Grid { get; private set; }

        /// <summary>
        /// 玩家当前状态
        /// </summary>
        public HeroHealth Health;
        
        /// <summary>
        /// 当前总属性
        /// </summary>
        public HeroProperty Property;

        /// <summary>
        /// 基础属性
        /// </summary>
        public HeroProperty BaseProperty;

        /// <summary>
        /// 当前行动状态
        /// </summary>
        public HeroControlMod ControlMod;

        public HeroData Data { get; private set; }

        public float ImmuneTime = 0f;

        public Transform SpineContainer;

        /// <summary>
        /// 技能组件
        /// </summary>
        public HeroSkillCom SkillCom { get; private set; }
        
        /// <summary>
        /// Buff组件
        /// </summary>
        public HeroBuffCom BuffCom { get; private set; }
        
        /// <summary>
        /// 装备组件
        /// </summary>
        public HeroEquipmentCom EquipmentCom { get; private set; }

        public HeroUICom UiCom { get; private set; } 
        
        public HeroSpineCom SpineCom { get; private set; }
        
        public HeroBindCom BindCom { get; private set; }


        public bool IsDead => Health.HP <= 0;
        
        public EFaction FactionType { get; private set; }

        public bool IsFrontRowHero => Grid.Y == 1;

        public bool IsRearRowHero => Grid.Y == 2;
        
        
        public void Init(HeroData data,Grid grid,EFaction faction)
        {
            Data = data;
            Grid = grid;
            FactionType = faction;
            
            SkillCom = GetComponent<HeroSkillCom>();
            BuffCom = GetComponent<HeroBuffCom>();
            EquipmentCom = GetComponent<HeroEquipmentCom>();
            UiCom = GetComponent<HeroUICom>();
            SpineCom = GetComponent<HeroSpineCom>();
            BindCom = GetComponent<HeroBindCom>();
            
            BuffCom.Initialize(this);
            SkillCom.Initialize(this);
            EquipmentCom.Initialize(this);
            UiCom.Initialize(this);
            SpineCom.Initialize(this);
            BindCom.Initialize(this);
            
            InitProperty(Data.BaseProperty);
        }
        
        public void InitProperty(HeroProperty baseProp)
        {
            BaseProperty = baseProp;
            RecheckProperty();
            Health.HP = Property.TotalHp;
            Health.Shield = 0;
        }

        public void ModifyHealth(DamageInfo damageInfo)
        {
            int calFinalTotalDamage = damageInfo.CalFinalTotalDamage();
            
            if (damageInfo.IsHealDamage())
            {
                Health.HP += calFinalTotalDamage;
                Health.HP = Mathf.Clamp(Health.HP, 0, Property.Hp);
            }
            else
            {
                Health.Shield -= calFinalTotalDamage;
                if (Health.Shield < 0)
                {
                    calFinalTotalDamage = -Health.Shield;
                    Health.Shield = 0;
                    Health.HP -= calFinalTotalDamage;
                }
            }
            
            UiCom.Modify();
            if (Health.HP < 0)
            {
                Health.HP = 0;
                OnKill();
            }
        }

        public void ModifyHealth(HeroHealth health)
        {
            Health += health;
            Health.HP = Mathf.Clamp(Health.HP, 0, Property.Hp);
            Health.Shield = Mathf.Clamp(Health.Shield, 0, Property.Shield);
        }

        public bool CanBeKilledByDamageInfo(DamageInfo dmgInfo)
        {
            if (!ControlMod.CanBeHurt || dmgInfo.IsHealDamage()) return false;

            //TODO: 伤害计算后续优化
            float totalHP = Health.HP + Health.Shield;
            int finalTotalDamage = dmgInfo.CalFinalTotalDamage();
            if (finalTotalDamage >= totalHP)
            {
                return true;
            }

            return false;
        }

        public void OnKill()
        {
            // TODO:播放消融 过几秒Obj删除
        }

        public async UniTask StartBattle()
        {
            if (ImmuneTime > 0)
            {
                ImmuneTime --;

                if (ImmuneTime <= 0)
                {
                    ControlMod.CanBeHurt = true;
                }
            }
            
            BuffCom.OnTick();
            await SkillCom.CastSkill();
        }

        public void RecheckProperty()
        {
            ControlMod = HeroControlMod.Default;
            // TODO: 敌人只能用普攻
            if (FactionType == EFaction.Enemy)
            {
                ControlMod.CanUseSkill = false;
            }

            Property = BaseProperty;
            Property += GetPropertyFromBuff();
            Property += GetPropertyFromEquipment();
        }
        
        public HeroProperty GetPropertyFromBuff()
        {
            return BuffCom.GetProperty();
        }

        public HeroProperty GetPropertyFromEquipment()
        {
            return EquipmentCom.GetProperty();
        }
    }
}