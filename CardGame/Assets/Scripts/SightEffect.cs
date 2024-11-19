using System;
using UnityEngine;

namespace MyGame
{
    public class SightEffect : MonoBehaviour
    {
        public float Duration;
        public void Init()
        {
            MoveableSightEffect[] componentsInChildren = GetComponentsInChildren<MoveableSightEffect>();
            foreach (var moveableSightEffect in componentsInChildren)
            {
                moveableSightEffect.Init();
            }
        }
    }
}