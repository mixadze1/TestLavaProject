using System.IO;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class SafeData
    {
        public string FileResource = "ResourceData.json";
        public string FilePlayer = "PlayerData.json";

        public void SafeGame<T>(T data, string fileName)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            string path = Path.Combine(Application.persistentDataPath, fileName);
#else
            string path = Path.Combine(Application.dataPath, fileName);
#endif
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(path, jsonData);
        }

        public T LoadData<T>(string fileName)
        {

#if UNITY_ANDROID || UNITY_EDITOR      
            string path = Path.Combine(Application.persistentDataPath, fileName);
#else
            string path = Path.Combine(Application.dataPath, fileName);
#endif

            if (!File.Exists(path))
            {
                Debug.Log("No saved data found.");
                return default(T);
            }

            string jsonData = File.ReadAllText(path);

            return JsonUtility.FromJson<T>(jsonData);
        }

        public void DeleteFile(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("File deleted: " + filePath);
            }
            else
            {
                Debug.Log("File not found: " + filePath);
            }
        }
    }
}