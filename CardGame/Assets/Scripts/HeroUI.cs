﻿using UnityEngine;

namespace MyGame
{
    public class HeroUI : MonoBehaviour,IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj, GameManager gameManager)
        {
            throw new System.NotImplementedException();
        }

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }
    }
}