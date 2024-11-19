namespace MyGame
{
    public static class TimelineHelper
    {
        public static TimelineModel GetTimelineModel(string key,EventWarp[] effectValue)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Timeline timeline = tableManager.Tables.TbTimeline.Get(key);
            TimelineNode[] nodes = new TimelineNode[timeline.Node.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = ConvertTimelineNodeWarpToTimelineNode(timeline.Node[i]);

                if (effectValue != null)
                {
                    foreach (EventWarp warp in effectValue)
                    {
                        if (timeline.Node[i].EventWarp.Event == warp.Event)
                        {
                            nodes[i].EventParameters.AddRange(MObjectHelper.ConvertMObjectToObject(warp.Params));
                        }
                    }
                }
            }
            
            TimelineModel skillModel = new TimelineModel(timeline.Key,nodes,timeline.Duration);
            return skillModel;
        }

        private static TimelineNode ConvertTimelineNodeWarpToTimelineNode(TimelineNodeWarp warp)
        {
            return new TimelineNode(warp.Elapsed,warp.EventWarp.Event, MObjectHelper.ConvertMObjectToObject(warp.EventWarp.Params));
        }
    }
}