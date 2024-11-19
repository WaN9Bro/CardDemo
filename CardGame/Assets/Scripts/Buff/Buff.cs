using System.Collections.Generic;
using System.Threading;
using MyGame;
using UnityEngine;

namespace MyGame
{
    /// <summary>
    /// 真正运行的Buff对象
    /// </summary>
    public class BuffObj
    {
        /// <summary>
        /// 程序可使用的Config
        /// </summary>
        public BuffModel Model { get; private set; }

        /// <summary>
        /// 持续回合
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 过了几回合了
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// 是否永久
        /// </summary>
        public bool Permanent { get; set; }

        /// <summary>
        /// 当前层数
        /// </summary>
        public int Stack { get; set; }

        /// <summary>
        /// 施法者
        /// </summary>
        public HeroObj Caster { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public HeroObj Target { get; set; }

        /// <summary>
        /// 程序逻辑参数
        /// </summary>
        public Dictionary<string, object> BuffParams { get; set; }

        public BuffObj(BuffModel model, int duration, bool permanent, int stack, HeroObj caster, HeroObj target,
            Dictionary<string, object> buffParams)
        {
            Model = model;
            Duration = duration;
            Permanent = permanent;
            Stack = stack;
            Caster = caster;
            Target = target;
            BuffParams = buffParams;
            Times = 0;
        }
        
        public void ExecuteBuff(EBuffEventType type,BuffObj buff, params object[] args)
        {
            if (Model.EventWarps.TryGetValue(type,out BuffEventWarp warp))
            {
                warp.Event(buff,args);
            }
        }
    }

    public struct BuffEventWarp
    {
        /// <summary>
        /// 时机方法
        /// </summary>
        public BuffEvent Event { get; set; }
        /// <summary>
        /// 配置表里的参数，AddBuffInfo里的
        /// </summary>
        public List<object> EventParameters { get; set; }

        public BuffEventWarp(BuffEvent @event, List<object> eventParameters)
        {
            Event = @event;
            EventParameters = eventParameters;
        }
    }

    /// <summary>
    /// 程序使用的Config
    /// </summary>
    public struct BuffModel
    {
        /// <summary>
        /// Buff的Key
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; private set; }
        
        /// <summary>
        /// 优先级
        /// </summary>
        public int MaxStack { get; private set; }
        
        /// <summary>
        /// 工作周期：每几回合执行一次Tick
        /// </summary>
        public int TickTimes { get; private set; }
        
        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 属性修改
        /// </summary>
        public HeroProperty PropMod { get; set; }
        
        /// <summary>
        /// 行为修改
        /// </summary>
        public HeroControlMod ControlMod { get; set; }
        
        /// <summary>
        /// buff的所有时机方法和参数
        /// </summary>
        public Dictionary<EBuffEventType,BuffEventWarp> EventWarps { get; set; }
        
        public BuffModel(string id, int priority, int maxStack, int tickTimes,string[] tags,HeroProperty propMod,
            HeroControlMod controlMod, Dictionary<EBuffEventType,BuffEventWarp> @event)
        {
            Id = id;
            Priority = priority;
            MaxStack = maxStack;
            TickTimes = tickTimes;
            Tags = tags;
            PropMod = propMod;
            ControlMod = controlMod;
            
            EventWarps = new Dictionary<EBuffEventType, BuffEventWarp>();
            if (!@event.IsNullOrEmpty())
            {
                foreach (KeyValuePair<EBuffEventType, BuffEventWarp> kv in @event)
                {
                    EventWarps.Add(kv.Key,kv.Value);
                }
            }
        }
    }

    /// <summary>
    /// 添加Buff的中间件
    /// </summary>
    public struct AddBuffInfo
    {
        /// <summary>
        /// 施法者
        /// </summary>
        public HeroObj Caster { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public HeroObj Target { get; set; }

        /// <summary>
        /// 程序使用的Config
        /// </summary>
        public BuffModel Model { get; private set; }

        /// <summary>
        /// 添加的层数
        /// </summary>
        public int AddStack { get; private set; }

        /// <summary>
        /// 时间修改方式： true=直接设置时间，false=累加时间
        /// </summary>
        public bool TimeModify { get; private set; }

        /// <summary>
        /// 是否永久
        /// </summary>
        public bool Permanent { get; private set; }

        /// <summary>
        /// 持续多少回合
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// 程序逻辑参数
        /// </summary>
        public Dictionary<string, object> BuffParams { get; private set; }

        public AddBuffInfo(HeroObj caster, HeroObj target, BuffModel model, int addStack, bool timeModify,
            bool permanent, int duration, Dictionary<string, object> buffParams)
        {
            Caster = caster;
            Target = target;
            Model = model;
            AddStack = addStack;
            TimeModify = timeModify;
            Permanent = permanent;
            Duration = duration;
            BuffParams = buffParams;
        }
    }

    public delegate void BuffEvent(BuffObj buff, params object[] args);
    
    // public delegate void BuffOnOccur(BuffObj buff, int modifyStack);
    // public delegate void BuffOnRemoved(BuffObj buff);
    // public delegate void BuffOnTick(BuffObj buff);
    // public delegate void BuffOnHit(BuffObj buff, ref DamageInfo damageInfo, GameObject target);
    // public delegate void BuffOnBeHurt(BuffObj buff, ref DamageInfo damageInfo, GameObject attacker);
    // public delegate void BuffOnKill(BuffObj buff, DamageInfo damageInfo, GameObject target);
    // public delegate void BuffOnBeKilled(BuffObj buff, DamageInfo damageInfo, GameObject attacker);
    // public delegate TimelineObj BuffOnCast(BuffObj buff, SkillObj skill, TimelineObj timeline);
}