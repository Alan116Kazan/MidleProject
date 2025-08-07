using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager
{
    private readonly string _fileName;

    public GameDataManager(string fileName)
    {
        _fileName = fileName;
    }

    public async Task<PlayerStats> LoadPlayerStatsAsync(CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            string fileId = await GoogleDriveFinder.FindFileIdByNameAsync(_fileName);
            if (string.IsNullOrEmpty(fileId))
            {
                Debug.Log("Файл не найден на Google Drive.");
                return null;
            }

            ct.ThrowIfCancellationRequested();

            string json = await GoogleDriveDownloader.DownloadFileAsync(fileId);
            if (string.IsNullOrEmpty(json)) return null;

            var stats = JsonUtility.FromJson<PlayerStats>(json);
            return stats;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Загрузка файла отменена по таймауту.");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка при загрузке или десериализации: " + ex);
            return null;
        }
    }

    public async Task SavePlayerStatsAsync(PlayerStats stats)
    {
        await LocalFileSaver.SaveToLocalAndUpload(stats);
    }
}
