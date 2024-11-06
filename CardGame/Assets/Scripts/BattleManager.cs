using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.UI;
using UnityEngine;

namespace MyGame
{
    public class BattleManager : MonoBehaviour,IPreGameService
    {
        private Faction _playerFaction;
        
        private Faction _enemyFaction;
        
        private CancellationTokenSource cts;

        private int _round;
        
        public EBattleState BattleState { get; private set; }
        public bool IsPause { get; private set; }

        public List<BattlePlace> PlayerBattlePlaces = new List<BattlePlace>();
        
        public List<BattlePlace> EnemtyBattlePlaces = new List<BattlePlace>();
        
        
        public void Init()
        {
            cts = new CancellationTokenSource();
            BattleState = EBattleState.None;
        }
        
        public void RunBattle(Faction playerFaction,Faction enemyFaction)
        {
            _round = 0;
            _playerFaction = playerFaction;
            _enemyFaction = enemyFaction;
            PrepareBattle();
        }
        
        private void PrepareBattle()
        {
            _playerFaction.PrepareBattle();
            _playerFaction.SetHeroTransfrom(PlayerBattlePlaces);
            _enemyFaction.PrepareBattle();
            _enemyFaction.SetHeroTransfrom(EnemtyBattlePlaces);
            
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
                    _round++;
                    
                    // 玩家阵营 攻击 敌方阵营
                    await _playerFaction.StartBattle(_enemyFaction);
                    if (!_enemyFaction.HasEntityAlive)
                    {
                        EndBattle(true);
                        return;
                    }
                    
                    await _playerFaction.StartBattle(_enemyFaction);
                    
                    if (!_enemyFaction.HasEntityAlive)
                    {
                        EndBattle(false);
                        return;
                    }
                    
                }
            }
            catch (OperationCanceledException e)
            {
                EndBattle(false);
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