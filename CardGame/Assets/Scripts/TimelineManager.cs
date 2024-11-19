using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MyGame
{
    public class TimelineManager : IPostGameService , IFixedUpdate
    {
        private readonly List<TimelineObj> _timelines = new List<TimelineObj>();

        public void Init() { }
        
        public void FixedUpdate(float deltaTime)
        {
            if (_timelines.Count <= 0) return;

            int idx = 0;
            while (idx < _timelines.Count)
            {
                float wasTimeElapsed = _timelines[idx].TimeElapsed;
                _timelines[idx].TimeElapsed += deltaTime * _timelines[idx].TimeScale;
                
                //执行时间点内的事情
                for (int i = 0; i < _timelines[idx].Model.Nodes.Length; i++){
                    if (_timelines[idx].Model.Nodes[i].TimeElapsed < _timelines[idx].TimeElapsed && 
                        _timelines[idx].Model.Nodes[i].TimeElapsed >= wasTimeElapsed)
                    {
                        _timelines[idx].Model.Nodes[i].Event(_timelines[idx], _timelines[idx].Model.Nodes[i].EventParameters.ToArray());
                    }
                }

                //判断timeline是否终结
                if (_timelines[idx].Model.Duration <= _timelines[idx].TimeElapsed)
                {
                    TimelineObj timelineObj = _timelines[idx];
                    _timelines.RemoveAt(idx); 
                    timelineObj.Clear();
                    //ReferencePool.Release(timelineObj);
                }
                else
                {
                    idx++;
                }
            }
        }

        public bool AddTimeline(TimelineObj timelineObj)
        {
            if (_timelines.Contains(timelineObj)) return false;
            _timelines.Add(timelineObj);
            return true;
        }
    }
}