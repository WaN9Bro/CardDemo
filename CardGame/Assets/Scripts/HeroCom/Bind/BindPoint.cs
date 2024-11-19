using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BindPoint : MonoBehaviour
    {
        private class BindInfo
        {
            public GameObject Obj;

            public float Duration;

            public bool Permanent;

            public BindInfo(GameObject obj, float duration)
            {
                Obj = obj;
                Duration = duration;
                Permanent = duration <= 0;
            }
        }

        public string Key;

        public Vector3 Offset;

        private readonly Dictionary<string, BindInfo> _bindInfos = new Dictionary<string, BindInfo>();

        private void FixedUpdate()
        {
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, BindInfo> goInfo in _bindInfos)
            {
                if (!goInfo.Value.Obj)
                {
                    toRemove.Add(goInfo.Key);
                    continue;
                }

                if (!goInfo.Value.Permanent)
                {
                    goInfo.Value.Duration -= Time.fixedDeltaTime;
                    if (goInfo.Value.Duration <= 0)
                    {
                        Destroy(goInfo.Value.Obj);
                        toRemove.Add(goInfo.Key);
                    }
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _bindInfos.Remove(toRemove[i]);
            }
        }

        public void AddBindGameObject(string goPath, string key)
        {
            if (key != "" && _bindInfos.ContainsKey(key) == true) return; //已经存在，加不成

            GameObject effectGO = Instantiate<GameObject>(
                Resources.Load<GameObject>(goPath),
                Vector3.zero,
                Quaternion.identity,
                this.gameObject.transform
            );

            effectGO.transform.localPosition = this.Offset;
            effectGO.transform.localRotation = Quaternion.identity;
            if (!effectGO) return;
            SightEffect se = effectGO.GetComponent<SightEffect>();
            if (!se)
            {
                Destroy(effectGO);
                return;
            }

            se.Init();
            BindInfo bindGameObjectInfo = new BindInfo(effectGO, se.Duration);
            if (key != "")
            {
                _bindInfos.Add(key, bindGameObjectInfo);
            }
            else
            {
                _bindInfos.Add(Time.frameCount * Random.Range(1.00f, 9.99f) + "_" + Random.Range(1, 9999),
                    bindGameObjectInfo);
            }
        }


        public void AddPopText(EFaction faction,string goPath,string text)
        {
            PopText popText = Instantiate<PopText>(
                Resources.Load<PopText>(goPath),
                Vector3.zero,
                Quaternion.identity,
                this.gameObject.transform
            );
            if (!popText) return;
            
            popText.transform.localPosition = this.Offset;
            popText.transform.localRotation = Quaternion.identity;

            if (faction == EFaction.Player)
            {
                popText.transform.localScale = Vector3.one;
            }
            else
            {
                popText.transform.localScale = new Vector3(-1, 1, 1);
            }
            popText.InitText(text);
            BindInfo bindGameObjectInfo = new BindInfo(popText.gameObject, popText.Duration);
            _bindInfos.Add(Time.frameCount * Random.Range(1.00f, 9.99f) + "_" + Random.Range(1, 9999),
                bindGameObjectInfo);
        }
        
        public void RemoveBindGameObject(string key){
            if (_bindInfos.ContainsKey(key) == false) return;
            if (_bindInfos[key].Obj){
                Destroy(_bindInfos[key].Obj);
            }
            _bindInfos.Remove(key);
        }
    }
}