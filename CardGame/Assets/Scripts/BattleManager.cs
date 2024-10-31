using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.Exceptions;

namespace MyGame
{
    public class BattleManager : MonoBehaviour,IPreGameService
    {
        private GameManager _gameManager;
        private Faction _playerFaction;
        private Faction _enemyFaction;

        private CancellationTokenSource cts;
        public EBattleState BattleState { get; private set; }
        public bool IsPause { get; private set; }
        
        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            cts = new CancellationTokenSource();
            BattleState = EBattleState.None;
        }
        
        public void StartBattle(Faction playerFaction,Faction enemyFaction)
        {
            _playerFaction = playerFaction;
            _enemyFaction = enemyFaction;
            PreBattle();
        }
        
        private void PreBattle()
        {
            _playerFaction.PreBattle();
            _enemyFaction.PreBattle();
            BattleState = EBattleState.Running;
            InternalStartBattle(cts.Token).Forget();
            
        }

        private async UniTask InternalStartBattle(CancellationToken token)
        {
            try
            {
                //直到战斗结束才停止：  我方或者敌方 全部阵亡， 或者 点击跳过直接进行整体伤害计算，不走动画那一套
                while (!token.IsCancellationRequested || (_playerFaction.HasEntityAlive && _enemyFaction.HasEntityAlive))
                {
                    if (IsPause) continue;
                    
                    // 玩家阵营 攻击 敌方阵营

                    await _playerFaction.FactionAciton(_enemyFaction);
                    if (!_enemyFaction.HasEntityAlive)
                    {
                        EndBattle(true);
                        return;
                    }
                    
                    await _playerFaction.FactionAciton(_enemyFaction);
                    
                    if (!_enemyFaction.HasEntityAlive)
                    {
                        EndBattle(false);
                        return;
                    }
                    
                }
            }
            catch (OperationCanceledException e)
            {
                
            }
            
        }

        private void EndBattle(bool win)
        {
            cts.Cancel();
            cts.Dispose();
            BattleState = EBattleState.Ended;
        }
        
        public void ClearBattle()
        {
            
        }
    }
}