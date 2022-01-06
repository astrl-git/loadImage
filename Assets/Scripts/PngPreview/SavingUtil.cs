using System;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace PngPreview
{
    public class SavedJSONPath<T> where T : class
    {
        public string Value { get; private set; }

        private SavedJSONPath(string path)
        {
            Value = path;
        }

        public static SavedJSONPath<T> Create(string path)
        {
            return new SavedJSONPath<T>(path);
        }

        public override string ToString() => Value;
    }

    public class SavingUtil
    {
        private static readonly string savePath = Application.persistentDataPath + "/.gamefiles";
        private static string jsonFileFormat = savePath + "/{0}.json";

        public static void SaveAsJSON<T>(T data, SavedJSONPath<T> path) where T : class
        {
            var type = typeof(T).ToString();
            CheckSavesDirectory();
            var fullPath = string.Format(jsonFileFormat, path.Value);
            try
            {
                var json = JsonConvert.SerializeObject(data);
                File.WriteAllText(fullPath, json);
                Debug.Log($"[LOG] SaveUtil.SaveAsJSON<{type}> File saved:\n{fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ERR] SaveUtil.SaveAsJSON<{type}> Saving error\n{e}");
            }
        }

        public static T LoadJSON<T>(SavedJSONPath<T> path) where T : class
        {
            var type = typeof(T).ToString();
            CheckSavesDirectory();
            string fullPath = string.Format(jsonFileFormat, path.Value);

            if (File.Exists(fullPath))
            {
                try
                {
                    using var r = new StreamReader(fullPath);
                    var json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (SerializationException e)
                {
                    Debug.LogError($"[ERR] SaveUtil.LoadJSON<{type}> Savings broken\n{e}");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning($"[WAR] SaveUtil.LoadJSON<{type}> No savings found\n{fullPath}");
                return null;
            }
        }

        private static bool CheckSavesDirectory()
        {
            try
            {
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ERR] SaveUtil.CheckSavesDirectory\n{ex}");
                return false;
            }
        }
    }
}