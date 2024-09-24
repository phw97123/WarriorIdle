using System.IO;
using UnityEngine;

public class DataManager 
{
    public void SaveData<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data,true); 
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json); 
    }

    public T LoadData<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }
        else
        {
            Debug.Log("불러올 데이터가 없습니다"); 
            return default; 
        }
    }

    public void DeleteAllData()
    {
        string path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(path, "*.json");

        foreach(string file in files)
        {
            File.Delete(file);
        }

        PlayerPrefs.DeleteAll(); 
    }
}
