using System;
using System.Collections.Generic;

namespace MyGame
{
    public struct HeroModel
    {
        public int Id;
        
        public int Level;
        
        public List<EquipmentData> Equipment;
        
        public List<SkillData> Skill;

        public static void Create(int id,int level,GameManager manager)
        {
            if (manager.GetService(out ConfigManager configManager))
            {
                
            }
        }
        
    }

    [Serializable]
    public class EquipmentData
    {
        public int Id;
        
        public int Level;
    }

    public class SkillData
    {
        public int Id;
        
        public int Level;
    }
}