using System;
using UnityEngine;

namespace MyGame
{
    public class MoveableSightEffect : MonoBehaviour
    {
        public string targetName;

        public void Init()
        {
            if (targetName == "EnemyCenter")
            {
                GameManager.Instance.GetService(out BattleManager battleManager);
                transform.position = battleManager.enemyCenterTrans.position;
            }
        }
    }
}