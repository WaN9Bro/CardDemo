using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MyGame
{
    public class TableManager : MonoBehaviour, IPreGameService
    {
        public string ConfigPath = Application.dataPath + "/GenerateDatas/Json";
        public Tables Tables { get; private set; }
        public void Init()
        {
            Tables = new Tables(LoadJson);
        }
        private JArray LoadJson(string file)
        {
            return JsonConvert.DeserializeObject(System.IO.File.ReadAllText($"{ConfigPath}/" + file + ".json")) as JArray;
        }
    }
}