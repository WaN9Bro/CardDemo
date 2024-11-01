using System;
using UnityEngine;

namespace MyGame
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                if (_instance == null)
                {
                    Debug.LogError($"[MonoSingleton] '{typeof(T).FullName}' instance is not found.");
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}