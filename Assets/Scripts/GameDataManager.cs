using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager
{
    private readonly string _fileName;

    public GameDataManager(string fileName)
    {
        _fileName = fileName;
    }

    public async Task<PlayerStats> LoadPlayerStatsAsync()
    {
        string fileId = await GoogleDriveFinder.FindFileIdByNameAsync(_fileName);
        if (string.IsNullOrEmpty(fileId))
        {
            Debug.Log("Файл не найден на Google Drive.");
            return null;
        }

        string json = await GoogleDriveDownloader.DownloadFileAsync(fileId);
        if (string.IsNullOrEmpty(json)) return null;

        try
        {
            var stats = JsonUtility.FromJson<PlayerStats>(json);
            return stats;
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка при десериализации: " + ex.Message);
            return null;
        }
    }

    public async Task SavePlayerStatsAsync(PlayerStats stats)
    {
        await LocalFileSaver.SaveToLocalAndUpload(stats);
    }
}
