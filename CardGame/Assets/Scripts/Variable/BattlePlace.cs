using System;
using UnityEngine;

namespace MyGame
{
    
    [Serializable]
    public struct Grid
    {
        public int X;
        public int Y;
        
        public Grid(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}