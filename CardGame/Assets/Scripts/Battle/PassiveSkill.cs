using System.Collections.Generic;

namespace MyGame
{
    public class PassiveSkillObj : IReference
    {
        public int Id;
        public List<AddBuffInfo> AddBuffInfos = new List<AddBuffInfo>();

        public void Init(int id, List<AddBuffInfo> addBuffInfos)
        {
            Id = id;
            AddBuffInfos = addBuffInfos;
        }
        
        public void Clear()
        {
            Id = 0;
            AddBuffInfos.Clear();
        }
    }
}