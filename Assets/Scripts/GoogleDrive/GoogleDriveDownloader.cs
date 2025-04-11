using System;
using System.IO;
using UnityEngine;
using UnityGoogleDrive;

public static class GoogleDriveDownloader
{
    public static void DownloadFile(string fileId, Action<string> onDone = null)
    {
        if (string.IsNullOrEmpty(fileId))
        {
            Debug.LogError("File ID is null or empty.");
            return;
        }

        GoogleDriveFiles.Download(fileId).Send().OnDone += downloadedFile =>
        {
            if (downloadedFile?.Content == null)
            {
                Debug.LogError("Не удалось загрузить файл.");
                return;
            }

            string json = System.Text.Encoding.UTF8.GetString(downloadedFile.Content);
            string filePath = Path.Combine(Application.persistentDataPath, LocalFileSaver.SaveFileName);
            File.WriteAllText(filePath, json);
            Debug.Log($"Файл загружен и сохранён: {filePath}");
            onDone?.Invoke(json);
        };
    }
}