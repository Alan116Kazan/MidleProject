using System.IO;
using UnityEngine;

public static class LocalFileSaver
{
    public const string SaveFileName = "GameData.json";

    // Метод для сохранения данных на локальном компьютере
    public static void SaveToLocal(string obj)
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

        // Сохраняем файл на локальный диск
        File.WriteAllText(filePath, obj);

        Debug.Log("Файл сохранен на локальном компьютере: " + filePath);
    }
}