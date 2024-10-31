using System.Collections.Generic;

namespace MyGame.Data.DesignerScripts
{
    //  回调点的函数具体实现定义
    public class Timeline
    {
        public static Dictionary<string, TimelineEvent> Functions = new Dictionary<string, TimelineEvent>
        {
            { ",",Test }
        };

        private static void Test(TimelineModel model, params object[] args)
        {
            
        }
}
}