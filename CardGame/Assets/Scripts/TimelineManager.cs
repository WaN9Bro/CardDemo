﻿using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TimelineManager : IPostGameService , IFixedUpdate
    {
        private readonly List<TimelineObj> _timelines = new List<TimelineObj>();

        public void Init()
        {
            
        }
        
        public void FixedUpdate()
        {
            if (_timelines.Count <= 0) return;

            int idx = 0;
            while (idx < _timelines.Count)
            {
                float wasTimeElapsed = _timelines[idx].TimeElapsed;
                _timelines[idx].TimeElapsed += Time.fixedDeltaTime * _timelines[idx].TimeScale;
                
                //执行时间点内的事情
                for (int i = 0; i < _timelines[idx].Model.Nodes.Length; i++){
                    if (_timelines[idx].Model.Nodes[i].TimeElapsed < _timelines[idx].TimeElapsed && 
                        _timelines[idx].Model.Nodes[i].TimeElapsed >= wasTimeElapsed)
                    {
                        _timelines[idx].Model.Nodes[i].Event(_timelines[idx], _timelines[idx].Model.Nodes[i].EventParameters);
                    }
                }

                //判断timeline是否终结
                if (_timelines[idx].Model.Duration <= _timelines[idx].TimeElapsed)
                {
                    _timelines.RemoveAt(idx);
                }
                else
                {
                    idx++;
                }
            }
        }

        public void AddTimeline(TimelineObj timelineObj)
        {
            if (_timelines.Contains(timelineObj)) return;
            _timelines.Add(timelineObj);
        }
    }
}