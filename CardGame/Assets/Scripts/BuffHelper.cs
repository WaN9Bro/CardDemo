﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGame
{
    public static class BuffHelper
    {
        public static BuffModel ConvertWarpToModel(this AddBuffWarp self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Buff config = tableManager.Tables.TbBuff.Get(self.BuffKey);
            Dictionary<EBuffEventType, BuffEventWarp> eventWarps = new Dictionary<EBuffEventType, BuffEventWarp>();
            
            if (self.EventValueWarp != null)
            {
                foreach (BuffEventValueWarp warp in self.EventValueWarp)
                {
                    eventWarps.Add(warp.BuffEventType,new BuffEventWarp(BuffFunction.Functions[warp.BuffEventType][warp.Event],new List<object>()));
                }
            }

            return new BuffModel(config.Id, config.Priority, config.MaxStack, config.TickTime, config.Tags,
                config.PropMod.ConvertWarpToHeroProperty() + self.PropMod.ConvertWarpToHeroProperty(),
                config.ControlMod.ConvertWarpToHeroControlMod() + self.ControlMod.ConvertWarpToHeroControlMod(), eventWarps);
        }

        public static List<BuffModel> ConvertWarpToModel(this AutoAddBuffWarp[] self)
        {
            List<BuffModel> models = new List<BuffModel>(self.Length);
            foreach (AutoAddBuffWarp war in self)
            {
                models.Add(war.ConvertWarpToModel());
            }

            return models;
        }
        

        // 学习技能后自动学的buff属于被动技能buff
        public static BuffModel ConvertWarpToModel(this AutoAddBuffWarp self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Buff config = tableManager.Tables.TbBuff.Get(self.BuffKey);
            Dictionary<EBuffEventType,BuffEventWarp> eventWarps = new Dictionary<EBuffEventType, BuffEventWarp>();

            if (self.EventValueWarp != null)
            {
                foreach (BuffEventValueWarp warp in self.EventValueWarp)
                {
                    eventWarps.Add(warp.BuffEventType,new BuffEventWarp(BuffFunction.Functions[warp.BuffEventType][warp.Event],new List<object>()));
                }
            }

            return new BuffModel(config.Id, config.Priority, config.MaxStack, config.TickTime, config.Tags,
                self.PropMod.ConvertWarpToHeroProperty() + self.PropMod.ConvertWarpToHeroProperty(),
                self.ControlMod.ConvertWarpToHeroControlMod() + self.ControlMod.ConvertWarpToHeroControlMod(), eventWarps);
        }

        public static AddBuffInfo ConvertWarpToAddBuffInfo(this AddBuffWarp self)
        {
            return new AddBuffInfo(null, null, self.ConvertWarpToModel(), self.AddStack,
                self.TimeModify, self.Permanent, self.Duration, new Dictionary<string, object>());
        }

        public static HeroProperty ConvertWarpToHeroProperty(this Dictionary<EPropertyModType, mObject> self)
        {
            HeroProperty heroProperty = new HeroProperty();
            foreach (KeyValuePair<EPropertyModType, mObject> kv in self)
            {
                switch (kv.Key)
                {
                    case EPropertyModType.HP:
                        heroProperty.Hp = ((mInt)kv.Value).Data;
                        break;
                    case EPropertyModType.HPRatio:
                        heroProperty.HpRatio = ((mFloat)kv.Value).Data;
                        break;
                    case EPropertyModType.Attack:
                        heroProperty.Attack = ((mInt)kv.Value).Data;
                        break;
                    case EPropertyModType.AttackRatio:
                        heroProperty.AttackRatio = ((mFloat)kv.Value).Data;
                        break;
                    case EPropertyModType.PhysicalDefense:
                        heroProperty.PhysicalDefense = ((mInt)kv.Value).Data;
                        break;
                    case EPropertyModType.PhysicalDefenseRatio:
                        heroProperty.PhysicalDefenseRatio = ((mFloat)kv.Value).Data;
                        break;
                    case EPropertyModType.MagicDefense:
                        heroProperty.MagicDefense = ((mInt)kv.Value).Data;
                        break;
                    case EPropertyModType.MagicDefenseRatio:
                        heroProperty.MagicDefenseRatio = ((mFloat)kv.Value).Data;
                        break;
                    case EPropertyModType.CriticalRate:
                        heroProperty.CriticalRate = ((mFloat)kv.Value).Data;
                        break;
                    case EPropertyModType.Shiled:
                        heroProperty.Shield = ((mInt)kv.Value).Data;
                        break;
                }
            }

            return heroProperty;
        }

        public static HeroControlMod ConvertWarpToHeroControlMod(this Dictionary<EControlModType, mObject> self)
        {
            HeroControlMod heroControlMod = new HeroControlMod();
            foreach (KeyValuePair<EControlModType, mObject> kv in self)
            {
                switch (kv.Key)
                {
                    case EControlModType.CanAttack:
                        heroControlMod.CanAttack = ((mBool)kv.Value).Data;
                        break;
                    case EControlModType.CanSkill:
                        heroControlMod.CanUseSkill = ((mBool)kv.Value).Data;
                        break;
                    case EControlModType.CanDead:
                        heroControlMod.CanDead = ((mBool)kv.Value).Data;
                        break;
                    case EControlModType.CanBeHurt:
                        heroControlMod.CanBeHurt = ((mBool)kv.Value).Data;
                        break;
                    case EControlModType.CanHeal:
                        heroControlMod.CanHeal = ((mBool)kv.Value).Data;
                        break;
                }
            }

            return heroControlMod;
        }


        public static List<AddBuffInfo> ConvertWarpToAddBuffInfos(this AutoAddBuffWarp[] self)
        {
            List<AddBuffInfo> addBuffInfos = new List<AddBuffInfo>(self.Length);
            foreach (AutoAddBuffWarp warp in self)
            {
                addBuffInfos.Add(warp.ConvertWarpToAddBuffInfo());
            }

            return addBuffInfos;
        }

        public static AddBuffInfo ConvertWarpToAddBuffInfo(this AutoAddBuffWarp self)
        {
            return new AddBuffInfo(null,null,self.ConvertWarpToModel(),self.AddStack,
                self.TimeModify,self.Permanent,self.Duration,new Dictionary<string, object>());
        }
    }
}