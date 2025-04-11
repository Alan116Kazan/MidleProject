using System;
using System.IO;
using UnityEngine;

public static class LocalFileSaver
{
    public const string SaveFileName = "GameData.json";

    public static void SaveToLocalAndUpload(PlayerStats playerStats)
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is null. Сохранение невозможно.");
            return;
        }

        try
        {
            var json = JsonUtility.ToJson(playerStats, prettyPrint: true);
            string path = Path.Combine(Application.persistentDataPath, SaveFileName);
            File.WriteAllText(path, json);

            Debug.Log($"Данные сохранены локально: {path}");
            Debug.Log($"Состояние игрока: здоровье = {playerStats.Health}, XP = {playerStats.XP}");

            GoogleDriveUploader.UploadFile(path);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при сохранении и загрузке: {ex}");
        }
    }
}