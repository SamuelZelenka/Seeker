using System.IO;
using System.Text.Json;
using UnityEngine;

public static class SaveSystem
{
    private static string GetPath(string fileName) => Application.persistentDataPath + "/" + fileName + ".stuff";


	public static void SaveData(string encryptionKey, string fileName, object data)
    {
        string json = JsonUtility.ToJson(data);

        string encryptedData = StringEncryption.Encrypt(json, encryptionKey);
        File.WriteAllText(GetPath(fileName), encryptedData);
    }

    public static T? LoadData<T>(string encryptionKey, string fileName)
    {
        string data = GetSessionDataString(encryptionKey, fileName);
        return JsonUtility.FromJson<T>(data);
	}

    public static string GetSessionDataString(string encryptionKey, string fileName)
    {
        if (File.Exists(GetPath(fileName)))
        {
            string rawData = File.ReadAllText(GetPath(fileName));
            string decryptedData = StringEncryption.Decrypt(rawData, encryptionKey);
            return decryptedData;
        }
        return null;
    }
    public static bool DeleteFile(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }
}
