using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TimelineObj
    {
        public TimelineModel Model { get; private set; }
        
        public HeroObj Caster { get; private set; }
        
        public float TimeScale { get; private set; }
        
        public object Param { get; private set; }
        
        public float TimeElapsed { get; private set; }
        
        public Dictionary<string,object> LogicParams { get; private set; }

        public TimelineObj(TimelineModel model, HeroObj caster, object param)
        {
            Model = model;
            Caster = caster;
            TimeScale = 1.0f;
            Param = param;
        }

        public object GetParam(string key)
        {
            if (LogicParams.TryGetValue(key, out object param))
            {
                return param;
            }

            Debug.LogError($"[TimelineObj] Logic param '{key}' not found: ]");
            return null;
        }
        
    }
    
    public struct TimelineModel
    {
        public string Id { get; private set; }
        
        public TimelineNode Nodes { get;private set; }
        
        public float Duration { get; private set; }
        
        public static TimelineModel Default = new TimelineModel("Default", TimelineNode.Default, 0);

        public TimelineModel(string id, TimelineNode nodes, float duration)
        {
            Id = id;
            Nodes = nodes;
            Duration = duration;
        }
    }

    public struct TimelineNode
    {
        public float TimeElapsed { get; private set; }
        
        public TimelineEvent Event { get; private set; }
        
        public object[] EventParameters { get; private set; }

        public static TimelineNode Default = new TimelineNode(0, null, null);

        public TimelineNode(float timeElapsed, TimelineEvent @event, object[] eventParameters)
        {
            TimeElapsed = timeElapsed;
            Event = @event;
            EventParameters = eventParameters;
        }
    }
    
    public delegate void TimelineEvent(TimelineModel model, params object[] args);
}