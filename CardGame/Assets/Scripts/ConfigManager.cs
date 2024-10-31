using System.IO;
using cfg;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using UnityEngine;

namespace MyGame
{
    public class ConfigManager : MonoBehaviour, IPreGameService
    {
        public string ConfigPath = Application.dataPath + "/GenerateDatas/Json";
        public Tables Tables { get; private set; }
        public void Init(GameManager gameManager)
        {
            Tables = new cfg.Tables(LoadJson);
        }
        private JArray LoadJson(string file)
        {
            return JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{ConfigPath}/" + file + ".json")) as JArray;
        }
    }
}