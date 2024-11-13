using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

namespace MyGame
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private IReadOnlyDictionary<Type,IPreGameService> _preGameServices = new Dictionary<Type,IPreGameService>();
        private IReadOnlyDictionary<Type,IPostGameService> _postGameServices = new Dictionary<Type,IPostGameService>();
        
        private readonly List<IFixedUpdate> _fixedUpdateServices = new List<IFixedUpdate>();
        
        private void Awake()
        {
            _preGameServices = GetComponentsInChildren<IPreGameService>().ToDictionary(service => service.GetType(), service => service);
            _postGameServices = GetComponentsInChildren<IPostGameService>().ToDictionary(service => service.GetType(), service => service);
            
            // 初始化前服务
            foreach (var service in _preGameServices.Values)
            {
                service.Init();
            }
            
            // 初始化后服务
            foreach (var service in _postGameServices.Values)
            {
                service.Init();
            }

            GetService(out PlayerManager playerManager);
            HeroData testHero = Helper.Create(1001);
            playerManager.AddHero(testHero);
            Faction playerFaction = FactionHelper.CreatePlayerFaction(playerManager.PlayerData.Faction);
            Faction enemyFaction = FactionHelper.CreateEnemyFaction(1);

            GetService(out BattleManager battleManager);
            battleManager.RunBattle(playerFaction, enemyFaction);
        }

        private void FixedUpdate()
        {
            foreach (IFixedUpdate service in _fixedUpdateServices)
            {
                service.FixedUpdate();
            }
        }

        public bool GetService<T>(out T service) where T : IGameService
        {
            if (_preGameServices.TryGetValue(typeof(T), out IPreGameService preGameService))
            {
                service = (T)preGameService;
                return true;
            }

            if (_postGameServices.TryGetValue(typeof(T), out IPostGameService postGameService))
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
