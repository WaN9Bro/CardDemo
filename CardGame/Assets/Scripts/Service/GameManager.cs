using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace MyGame
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private readonly Dictionary<Type,IPreGameService> _preGameServices = new Dictionary<Type,IPreGameService>();
        private readonly Dictionary<Type,IPostGameService> _postGameServices = new Dictionary<Type,IPostGameService>();
        
        private readonly List<IFixedUpdate> _fixedUpdateServices = new List<IFixedUpdate>();
        
        private void Awake()
        {
            _preGameServices.AddRange(GetComponentsInChildren<IPreGameService>().ToDictionary(service => service.GetType(), service => service));
            _preGameServices.AddRange(CreateInstance<IPreGameService>());
            
            _postGameServices.AddRange(GetComponentsInChildren<IPostGameService>().ToDictionary(service => service.GetType(), service => service));
            _postGameServices.AddRange(CreateInstance<IPostGameService>());

            foreach (IPreGameService service in _preGameServices.Values.ToList())
            {
                if (service is IFixedUpdate)
                {
                    _fixedUpdateServices.Add((IFixedUpdate)service);
                }
            }
            
            foreach (IPostGameService service in _postGameServices.Values.ToList())
            {
                if (service is IFixedUpdate)
                {
                    _fixedUpdateServices.Add((IFixedUpdate)service);
                }
            }
            
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
            HeroData testHero = HeroHelper.CreateHeroData(1001);
            playerManager.AddHero(testHero);
            playerManager.SetPlace(testHero.Uid,new Grid(0,1));
            Faction playerFaction = FactionHelper.CreatePlayerFaction(playerManager.PlayerData.Faction);
            Faction enemyFaction = FactionHelper.CreateEnemyFaction(1);

            GetService(out BattleManager battleManager);
            battleManager.RunBattle(playerFaction, enemyFaction).Forget();
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            foreach (IFixedUpdate service in _fixedUpdateServices)
            {
                service.FixedUpdate(deltaTime);
            }
        }

        private Dictionary<Type,T> CreateInstance<T>()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type interfaceType = typeof(T);
            Type monoBehaviourType = typeof(MonoBehaviour);

            Dictionary<Type, T> instances = assembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract &&
                            !monoBehaviourType.IsAssignableFrom(t))
                .Select(t => (T)Activator.CreateInstance(t))
                .ToDictionary(service => service.GetType(), service => service);
            
            return instances;
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
