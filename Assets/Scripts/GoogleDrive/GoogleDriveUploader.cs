using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class GoogleDriveUploader
{
    public static async Task UploadFileAsync(string localFilePath, Action onDone = null)
    {
        if (!File.Exists(localFilePath))
        {
            Debug.LogError("Файл не найден: " + localFilePath);
            return;
        }

        byte[] byteContent = File.ReadAllBytes(localFilePath);
        string fileName = Path.GetFileName(localFilePath);
        string mimeType = "application/json";

        string fileId = await GoogleDriveFinder.FindFileIdByNameAsync(fileName);
        if (string.IsNullOrEmpty(fileId))
        {
            Debug.Log("Файл не найден. Создание нового...");
            GoogleDriveFileEditor.CreateFile(fileName, byteContent, mimeType, onDone);
        }
        else
        {
            Debug.Log($"Файл найден. ID: {fileId}. Обновление...");
            GoogleDriveFileEditor.UpdateFile(fileId, fileName, byteContent, mimeType, onDone);
        }
    }
}
