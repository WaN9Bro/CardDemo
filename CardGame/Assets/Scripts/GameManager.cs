using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MyGame
{
    public class GameManager : MonoBehaviour
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
                service.Init(this);
            }
            
            // 初始化后服务
            foreach (var service in postGameServices.Values)
            {
                service.Init(this);
            }
            
            if (GetService<PlayerManager>(out PlayerManager playerManager))
            {
                HeroModel heroModel = new HeroModel();
                playerManager.
            }
            
            BattleManager.StartBattle();
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
