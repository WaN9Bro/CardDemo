using System.Collections.Generic;

namespace MyGame
{
    public class PlayerData
    {
        public string Name;
        
        public int Level;

        public int Exp;

        public int Energy;

        /// <summary>
        /// 金钱
        /// </summary>
        public int Money;
        
        /// <summary>
        /// 硬币
        /// </summary>
        public int Coin;
        
        /// <summary>
        /// 拥有的英雄列表
        /// </summary>
        public List<HeroModel> Heroes;
        
        /// <summary>
        /// 拥有的物品
        /// </summary>
        public List<ItemData> Items;
        
        public static PlayerData Default = new PlayerData("Default",0,0,100,100,100,new List<HeroModel>(),new List<ItemData>());

        public PlayerData(string name, int level, int exp, int energy, int money, int coin, List<HeroModel> heroes, List<ItemData> items)
        {
            Name = name;
            Level = level;
            Exp = exp;
            Energy = energy;
            Money = money;
            Coin = coin;
            Heroes = heroes;
            Items = items;
        }
    }
}