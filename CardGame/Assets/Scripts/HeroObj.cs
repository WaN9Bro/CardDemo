using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyGame
{
    public class HeroObj : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }
        public EStanding Standing { get; private set; }
        
        private Dictionary<string, IHeroComponent> _components;

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
        private HeroControlState _controlState;
        public HeroControlState ControlState
        {
            get =>_controlState; 
            private set => _controlState = value;
        }

        /// <summary>
        /// 技能组件
        /// </summary>
        public HeroSkill SkillComponent { get; private set; }
        
        /// <summary>
        /// Buff组件
        /// </summary>
        public HeroBuffCom BuffComComponent { get; private set; }
        
        /// <summary>
        /// 装备组件
        /// </summary>
        public HeroEquipmentCom EquipmentComComponent { get; private set; }
        
        
        public bool IsDead => Property.Hp <= 0;
        
        private void Awake()
        {
            _components = GetComponentsInChildren<IHeroComponent>().ToDictionary(com => nameof(com), com => com);
            SkillComponent = GetEntityComponent<HeroSkill>();
            BuffComComponent = GetEntityComponent<HeroBuffCom>();
            EquipmentComComponent = GetEntityComponent<HeroEquipmentCom>();
        }

        public void InitProperty(HeroProperty baseProp)
        {
            BaseProperty = baseProp;
            Resource.Hp = Property.Hp;
            Resource.Angry = 0;
            Resource.Shield = 0;
        }

        private void RefreshData()
        {
            ControlState = HeroControlState.Default;
            Property = HeroProperty.Default;
            Property += GetPropertyFromBuff();
            Property += GetPropertyFromSkill();
            Property += GetPropertyFromEquipment();
        }

        public HeroProperty GetPropertyFromBuff()
        {
            return BuffComComponent.GetProperty();
        }

        public HeroProperty GetPropertyFromSkill()
        {
            return SkillComponent.GetProperty();
        }

        public HeroProperty GetPropertyFromEquipment()
        {
            return EquipmentComComponent.GetProperty();
        }

        public T GetEntityComponent<T>() where T : IHeroComponent
        {
            if (_components.TryGetValue(nameof(T), out IHeroComponent component))
            {
                return (T)component;
            }

            Debug.LogError($"[Entity]component '{nameof(T)}' not found");
            return default;
        }
    }
}