using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.Build;
using UnityEngine;

namespace MyGame
{
    public class TimelineObj : IReference
    {
        public TimelineModel Model { get; private set; }
        
        public HeroObj Caster { get; private set; }
        
        public float TimeScale { get; private set; }
        
        public object Param { get; private set; }
        
        public float TimeElapsed { get; set; }

        public readonly Dictionary<string, object> LogicParams = new Dictionary<string, object>();

        private SemaphoreSlim _semaphore;

        public void Init(TimelineModel model, HeroObj caster,bool needSemaphore)
        {
            Model = model;
            Caster = caster;
            TimeScale = 1.0f;
            if (needSemaphore)
                _semaphore = new SemaphoreSlim(1, 1);
        }
        
        public void Clear()
        {
            Caster = null;
            LogicParams.Clear();
            TimeElapsed = 0f;
            Param = null;
            _semaphore?.Release();
            _semaphore = null;
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

        public async UniTask Await()
        {
            if (_semaphore == null) return;
            await _semaphore.WaitAsync();
        }
    }
    
    public struct TimelineModel
    {
        public string Id { get; private set; }
        
        public TimelineNode[] Nodes { get;set; }
        
        public float Duration { get; private set; }

        public TimelineModel(string id, TimelineNode[] nodes, float duration)
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
        
        public List<object> EventParameters { get; set; }
        
        public TimelineNode(float timeElapsed, string @event, List<object> eventParameters)
        {
            TimeElapsed = timeElapsed;
            Event = string.IsNullOrEmpty(@event) ? null : TimelineFunction.Functions[@event];
            EventParameters = eventParameters;
        }
    }
    
    public delegate void TimelineEvent(TimelineObj obj, params object[] args);
}