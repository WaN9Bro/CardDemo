using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MyGame
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private IReadOnlyDictionary<Type,IPreGameService> preGameServices = new Dictionary<Type,IPreGameService>();
        private IReadOnlyDictionary<Type,IPostGameService> postGameServices = new Dictionary<Type,IPostGameService>();
        
        private void Awake()
        {
            preGameServices = GetComponentsInChildren<IPreGameService>().ToDictionary(service => service.GetType(), service => service);
            postGameServices = GetComponentsInChildren<IPostGameService>().ToDictionary(service => service.GetType(), service => service);
            
            // 初始化前服务
            foreach (var service in preGameServices.Values)
            {
                service.Init();
            }
            
            // 初始化后服务
            foreach (var service in postGameServices.Values)
            {
                service.Init();
            }

            GetService(out PlayerManager playerManager);
            
            Faction playerFaction = FactionHelper.CreatePlayerFaction(playerManager.PlayerData.Faction);
            Faction enemyFaction = FactionHelper.CreateEnemyFaction(1);

            GetService(out BattleManager battleManager);
            battleManager.RunBattle(playerFaction, enemyFaction);
        }
        
        public bool GetService<T>(out T service) where T : IGameService
        {
            if (preGameServices.TryGetValue(typeof(T), out IPreGameService preGameService))
            {
                service = (T)preGameService;
                return true;
            }

            if (postGameServices.TryGetValue(typeof(T), out IPostGameService postGameService))
            {
                service = (T)postGameService;
                return true;
            }
            Debug.LogError($"[GameManager] No game service '{typeof(T)}' found]");
            service = default;
            return false;
        }
    }

}
