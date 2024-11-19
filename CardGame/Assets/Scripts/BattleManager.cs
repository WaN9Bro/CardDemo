using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyGame
{
    public class BattleManager : MonoBehaviour,IPreGameService
    {
        private Faction _playerFaction;
        
        private Faction _enemyFaction;
        
        private CancellationTokenSource cts;

        public int Round { get; private set; }
        
        public EBattleState BattleState { get; private set; }
        public bool IsPause { get; private set; }

        public Transform[] PlayerPlaces;
        public Transform[] EnemyPlaces;

        public Transform enemyCenterTrans;

        private  Transform[,] _playerPlaces;
        private Transform[,] _enemyPlaces;
        
        
        public void Init()
        {
            cts = new CancellationTokenSource();
            BattleState = EBattleState.None;
            
            _playerPlaces = ConvertTo2DArray(PlayerPlaces, 3, 2);
            _enemyPlaces = ConvertTo2DArray(EnemyPlaces, 3, 2);
        }

        private Transform[,] ConvertTo2DArray(Transform[] array, int rows, int cols)
        {
            if (array.Length != rows * cols)
            {
                throw new ArgumentException("数组长度与指定的行列数不匹配");
            }

            Transform[,] result = new Transform[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = array[i * cols + j];
                }
            }

            return result;
        }

        
        public async UniTask RunBattle(Faction playerFaction,Faction enemyFaction)
        {
            Round = 1;
            _playerFaction = playerFaction;
            _enemyFaction = enemyFaction;
            await PrepareBattle();
        }
        
        private async UniTask PrepareBattle()
        {
            _playerFaction.PrepareBattle();
            _enemyFaction.PrepareBattle();
            
            BattleState = EBattleState.Running;
            await InternalStartBattle(cts.Token);
        }

        public Transform GetPlace(EFaction faction,Grid grid)
        {
            return faction == EFaction.Player ? _playerPlaces[grid.X, grid.Y] : _enemyPlaces[grid.X, grid.Y];
        }

        private async UniTask InternalStartBattle(CancellationToken token)
        {
            try
            {
                //直到战斗结束才停止：  我方或者敌方 全部阵亡， 或者 点击跳过直接进行整体伤害计算，不走动画那一套
                while (!token.IsCancellationRequested || (_playerFaction.HasEntityAlive && _enemyFaction.HasEntityAlive))
                {
                    GameManager.Instance.GetService(out UIManager uiManager);
                    uiManager.Modify();
                    if (IsPause) continue;
                    // 玩家阵营 攻击 敌方阵营
                    await _playerFaction.StartBattle(_enemyFaction);
                    if (!_enemyFaction.HasEntityAlive)
                    {
                        EndBattle(true);
                        return;
                    }
                    
                    await _enemyFaction.StartBattle(_playerFaction);
                    if (!_playerFaction.HasEntityAlive)
                    {
                        EndBattle(false);
                        return;
                    }
                    Round++;
                    await UniTask.Yield();
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
            _playerFaction.Clear();
            _enemyFaction.Clear();
            
            Debug.LogError("游戏结束！");
        }

        public List<HeroObj> GetFilterTargets(TargetWarp warp,HeroObj caster)
        {
            Faction targetFaction = warp.TargetCamp == ETargetCamp.My ?
                caster.FactionType == EFaction.Player ? _playerFaction : _enemyFaction
                :
                caster.FactionType == EFaction.Player
                    ? _enemyFaction
                    : _playerFaction;
            return targetFaction.GetFilterHeroObjs(warp,caster);
        }

        public List<HeroObj> GetNoHasBuffHeroObjs(string key,EFaction faction)
        {
            if (faction == EFaction.Player)
            {
                return _playerFaction.GetNoHasBuffHeroObjs(key);
            }

            return _enemyFaction.GetNoHasBuffHeroObjs(key);
        }
    }
}