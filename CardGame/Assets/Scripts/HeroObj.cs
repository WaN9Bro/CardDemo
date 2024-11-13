using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyGame
{
    public class HeroObj : MonoBehaviour
    {
        public EStanding Standing { get; private set; }
        
        /// <summary>
        /// 玩家当前资源
        /// </summary>
        private HeroResource _resource;
        public HeroResource Resource
        {
            get => _resource;
            private set => _resource = value;
        }

        /// <summary>
        /// 当前总属性
        /// </summary>
        private HeroProperty _property;
        public HeroProperty Property
        {
            get => _property;
            private set => _property = value;
        }
        /// <summary>
        /// 基础属性
        /// </summary>
        public HeroProperty BaseProperty { get; private set; }

        /// <summary>
        /// 当前行动状态
        /// </summary>
        private HeroControlMod _controlMod;
        public HeroControlMod ControlMod
        {
            get =>_controlMod; 
            private set => _controlMod = value;
        }

        private HeroData _data;

        public HeroData Data
        {
            get => _data;
            private set => _data = value;
        }
        
        /// <summary>
        /// 战斗组件
        /// </summary>
        public HeroBattleCom BattleCom { get; private set; }

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


        public bool IsDead => Property.Hp <= 0;
        
        public void Init(HeroData data,EStanding standing)
        {
            _data = data;
            Standing = standing;
            
            BattleCom = ReferencePool.Acquire<HeroBattleCom>();
            SkillCom = ReferencePool.Acquire<HeroSkillCom>();
            BuffCom = ReferencePool.Acquire<HeroBuffCom>();
            EquipmentCom = ReferencePool.Acquire<HeroEquipmentCom>();
            UiCom = ReferencePool.Acquire<HeroUICom>();
            SpineCom = ReferencePool.Acquire<HeroSpineCom>();
            
            BattleCom.Initialize(this);
            SkillCom.Initialize(this);
            BuffCom.Initialize(this);
            EquipmentCom.Initialize(this);
            UiCom.Initialize(this);
            SpineCom.Initialize(this);
            
            InitProperty(_data.BaseProperty);
        }

        public void InitProperty(HeroProperty baseProp)
        {
            BaseProperty = baseProp;
            RecheckProperty();
            _resource.HP = Property.Hp;
            Resource.Shield = 0;
        }

        public async UniTask StartBattle(Faction selfFaction, Faction otherFaction)
        {
            await BattleCom.StartBattle(selfFaction, otherFaction);
        }

        private void RecheckProperty()
        {
            ControlMod = HeroControlMod.Default;
            Property = HeroProperty.Default;
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