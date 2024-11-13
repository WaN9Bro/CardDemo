﻿using System.Collections.Generic;
using MyGame;

namespace MyGame
{
    public class BuffObj
    {
        public BuffModel Model { get; private set; }
        
        public float Duration { get; private set; }
        
        public bool Permanent { get; private set; }
        
        public int Stack { get; private set; }
        
        public HeroObj Caster { get; private set; }

        public HeroObj Target { get; private set; }
        
        public float TimeElapsed { get; private set; }
        
        public int Ticked { get; private set; }
        
        public Dictionary<string,object> BuffParams { get; private set; }
    }

    public struct BuffModel
    {
        public string Id { get;private set; }
        
        public int Priority { get;private set; }
        
        public int MaxStack { get;private set; }
        
        public string[] Tags { get;private set; }
        
        public float TickTime { get;private set; }
        
        public HeroProperty PropMod { get;private set; }
        
        public ControlMod ModMod { get;private set; }
        
        public BuffOnOccur OnOccur { get;private set; }
        
        public object[] OnOccurParams { get;private set; }
        
        public BuffOnTick OnTick { get;private set; }
        
        public object[] OnTickParams { get;private set; }
        
        public BuffOnRemoved OnRemoved { get;private set; }
        
        public object[] OnRemovedParams { get;private set; }
        
        public BuffOnCast OnCast { get;private set; }
        
        public object[] OnCastParams { get;private set; }
        
        public BuffOnHit OnHit { get;private set; }
        
        public object[] OnHitParams { get;private set; }
        
        public BuffOnBeHurt OnBeHurt { get;private set; }
        
        public object[] OnBeHurtParams { get;private set; }
        
        public BuffOnKill OnKill { get;private set; }
        
        public object[] OnKillParams { get;private set; }
        
        public BuffOnBeKilled OnBeKilled { get;private set; }
        
        public object[] OnBeKilledParams { get;private set; }
        
        public BuffOnRound OnRound { get;private set; }
        
        public object[] OnRoundParams { get;private set; }

        public BuffModel(string id, int priority, int maxStack, string[] tags, float tickTime, HeroProperty propMod, ControlMod modMod, 
            string onOccur, object[] onOccurParams, 
            string onTick, object[] onTickParams, 
            string onRemoved, object[] onRemovedParams, 
            string onCast, object[] onCastParams, 
            string onHit, object[] onHitParams, 
            string onBeHurt, object[] onBeHurtParams, 
            string onKill, object[] onKillParams, 
            string onBeKilled, object[] onBeKilledParams,
            string onRound, object[] onRoundParams
            )
        {
            Id = id;
            Priority = priority;
            MaxStack = maxStack;
            Tags = tags;
            TickTime = tickTime;
            PropMod = propMod;
            ModMod = modMod;
            OnOccur = string.IsNullOrEmpty(onOccur) ? null : BuffFunction.OnOccurFunc[onOccur];
            OnOccurParams = onOccurParams;
            OnTick = string.IsNullOrEmpty(onTick) ? null : BuffFunction.OnTickFunc[onTick];
            OnTickParams = onTickParams;
            OnRemoved = string.IsNullOrEmpty(onRemoved) ? null : BuffFunction.OnRemovedFunc[onRemoved];
            OnRemovedParams = onRemovedParams;
            OnCast = string.IsNullOrEmpty(onCast) ? null : BuffFunction.OnCastFunc[onCast];
            OnCastParams = onCastParams;
            OnHit = string.IsNullOrEmpty(onHit) ? null : BuffFunction.OnHitFunc[onHit];
            OnHitParams = onHitParams;
            OnBeHurt = string.IsNullOrEmpty(onBeHurt) ? null : BuffFunction.OnBeHurtFunc[onBeHurt];
            OnBeHurtParams = onBeHurtParams;
            OnKill = string.IsNullOrEmpty(onKill) ? null : BuffFunction.OnKillFunc[onKill];
            OnKillParams = onKillParams;
            OnBeKilled = string.IsNullOrEmpty(onBeKilled) ? null : BuffFunction.OnBeKilledFunc[onBeKilled];
            OnBeKilledParams = onBeKilledParams;
            OnRound = string.IsNullOrEmpty(onRound) ? null : BuffFunction.OnRoundFunc[onBeKilled];
            OnRoundParams = onRoundParams;
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
        
        public Dictionary<string,object> BuffParams { get; private set; }

        public AddBuffInfo(HeroObj caster, HeroObj target, BuffModel model, int addStack, bool timeModify, bool permanent, float duration, Dictionary<string, object> buffParams)
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
    
    public delegate void BuffOnOccur(BuffObj buff, int modifyStack);
    public delegate void BuffOnRemoved(BuffObj buff);
    public delegate void BuffOnTick(BuffObj buff);
    public delegate void BuffOnHit(BuffObj buff, ref DamageInfo damageInfo, HeroObj target);
    public delegate void BuffOnBeHurt(BuffObj buff, ref DamageInfo damageInfo, HeroObj attacker);
    public delegate void BuffOnKill(BuffObj buff, DamageInfo damageInfo, HeroObj target);
    public delegate void BuffOnBeKilled(BuffObj buff, DamageInfo damageInfo, HeroObj attacker);
    public delegate TimelineObj BuffOnCast(BuffObj buff, SkillObj skill, TimelineObj timeline);
    public delegate void BuffOnRound(BuffObj buff);
}
