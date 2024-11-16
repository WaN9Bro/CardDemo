using System.Collections.Generic;
using System.Threading;
using MyGame;

namespace MyGame
{
    public class BuffObj
    {
        public BuffModel Model { get; private set; }

        public float Duration { get; set; }

        public bool Permanent { get; set; }

        public int Stack { get; set; }

        public HeroObj Caster { get; set; }

        public HeroObj Target { get; set; }

        public float TimeElapsed { get; set; }

        public int Ticked { get; set; }

        public Dictionary<string, object> BuffParams { get; set; }

        public BuffObj(BuffModel model, float duration, bool permanent, int stack, HeroObj caster, HeroObj target,
            Dictionary<string, object> buffParams)
        {
            Model = model;
            Duration = duration;
            Permanent = permanent;
            Stack = stack;
            Caster = caster;
            Target = target;
            BuffParams = buffParams;
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
        public BuffEvent Event { get; set; }
        public List<object> EventParameters { get; set; }

        public BuffEventWarp(BuffEvent @event, List<object> eventParameters)
        {
            Event = @event;
            EventParameters = eventParameters;
        }
    }

    public struct BuffModel
    {
        public string Id { get; private set; }

        public int Priority { get; private set; }

        public int MaxStack { get; private set; }

        public float TickTime { get; private set; }
        
        public string[] Tags { get; set; }

        public HeroProperty PropMod { get; set; }
        
        public HeroControlMod ControlMod { get; set; }
        
        public Dictionary<EBuffEventType,BuffEventWarp> EventWarps { get; set; }
        
        public BuffModel(string id, int priority, int maxStack, float tickTime,string[] tags,HeroProperty propMod,
            HeroControlMod controlMod, Dictionary<EBuffEventType,BuffEventWarp> @event)
        {
            Id = id;
            Priority = priority;
            MaxStack = maxStack;
            TickTime = tickTime;
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

    public struct AddBuffInfo
    {
        public HeroObj Caster { get; set; }

        public HeroObj Target { get; set; }

        public BuffModel Model { get; private set; }

        public int AddStack { get; private set; }

        public bool TimeModify { get; private set; }

        public bool Permanent { get; private set; }

        public float Duration { get; private set; }

        public Dictionary<string, object> BuffParams { get; private set; }

        public AddBuffInfo(HeroObj caster, HeroObj target, BuffModel model, int addStack, bool timeModify,
            bool permanent, float duration, Dictionary<string, object> buffParams)
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