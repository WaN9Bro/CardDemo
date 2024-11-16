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

        public HeroData Data;

        public float ImmuneTime = 0f;

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


        public bool IsDead => ControlMod.CanDead && Property.Hp <= 0;
        
        public EFaction FactionType { get; private set; }

        public bool IsFrontRowHero => Grid.Y == 1;

        public bool IsRearRowHero => Grid.Y == 2;
        
        public void Init(HeroData data,Grid grid,EFaction faction)
        {
            Data = data;
            Grid = grid;
            FactionType = faction;
            
            SkillCom = ReferencePool.Acquire<HeroSkillCom>();
            BuffCom = ReferencePool.Acquire<HeroBuffCom>();
            EquipmentCom = ReferencePool.Acquire<HeroEquipmentCom>();
            UiCom = ReferencePool.Acquire<HeroUICom>();
            SpineCom = ReferencePool.Acquire<HeroSpineCom>();
            BindCom = ReferencePool.Acquire<HeroBindCom>();
            
            SkillCom.Initialize(this);
            BuffCom.Initialize(this);
            EquipmentCom.Initialize(this);
            UiCom.Initialize(this);
            SpineCom.Initialize(this);
            BindCom.Initialize(this);
            
            InitProperty(Data.BaseProperty);
        }

        private void FixedUpdate()
        {
            if (IsDead) return;
            float time = Time.fixedDeltaTime;
            if (ImmuneTime > 0)
            {
                ImmuneTime -= time;
            }
            
            BuffCom.FixedUpdate(time);
        }

        public void InitProperty(HeroProperty baseProp)
        {
            BaseProperty = baseProp;
            RecheckProperty();
            Health.HP = Property.Hp;
            Health.Shield = 0;
        }

        public void ModifyHealth(DamageInfo damageInfo)
        {
            int calFinalTotalDamage = damageInfo.CalFinalTotalDamage();
            Health.Shield -= calFinalTotalDamage;
            if (Health.Shield < 0)
            {
                calFinalTotalDamage = -Health.Shield;
                Health.Shield = 0;
                Health.HP -= calFinalTotalDamage;
            }
            
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
            if (!ControlMod.CanDead || dmgInfo.IsHealDamage() || !ControlMod.CanBeHurt) return false;

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
            BuffCom.ExecuteBuff(EBuffEventType.OnRound);
            await SkillCom.CastSkill();
        }

        public void RecheckProperty()
        {
            ControlMod = HeroControlMod.Default;
            Property = HeroProperty.Empty;
            Property += GetPropertyFromBuff();
            Property += GetPropertyFromSkill();
            Property += GetPropertyFromEquipment();
        }
        
        public HeroProperty GetPropertyFromBuff()
        {
            return BuffCom.GetProperty();
        }

        public HeroProperty GetPropertyFromSkill()
        {
            return SkillCom.GetProperty();
        }

        public HeroProperty GetPropertyFromEquipment()
        {
            return EquipmentCom.GetProperty();
        }
    }
}