﻿using System;
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
        public List<HeroModel> Heroes = new List<HeroModel>();

        /// <summary>
        /// 拥有的物品
        /// </summary>
        public List<ItemData> Items = new List<ItemData>();
        
        /// <summary>
        /// 阵营索引配置
        /// </summary>
        public int[] Faction = new int[Enum.GetValues(typeof(EStanding)).Length];
        
        public static PlayerData Default = new PlayerData("Default",0,0,100,100,100,null,null,null);

        public PlayerData(string name, int level, int exp, int energy, int money, int coin, List<HeroModel> heroes, List<ItemData> items,int[] faction)
        {
            Name = name;
            Level = level;
            Exp = exp;
            Energy = energy;
            Money = money;
            Coin = coin;
            
            if (heroes.IsNullOrEmpty() == false)
            {
                Heroes.AddRange(heroes);
            }
            
            if (items.IsNullOrEmpty() == false)
            {
                Items.AddRange(items);
            }

            if (faction != null && faction.Length ==  Faction.Length)
            {
                for (var i = 0; i < Faction.Length; i++)
                {
                    Faction[i] = faction[i];
                }
            }
        }
    }
}