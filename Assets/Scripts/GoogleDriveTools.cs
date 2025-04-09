using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;

public static class GoogleDriveTools
{
    private static string lastUploadedFileId;

    public static string LastUploadedFileId => lastUploadedFileId;



    public static void UploadOrUpdateFileFromDisk(string localFilePath, Action onDone = null)
    {
        if (!System.IO.File.Exists(localFilePath))
        {
            Debug.LogError("Файл не найден: " + localFilePath);
            return;
        }

        byte[] byteContent = System.IO.File.ReadAllBytes(localFilePath);
        string fileName = Path.GetFileName(localFilePath);
        string mimeType = "application/json";

        var listRequest = GoogleDriveFiles.List();
        listRequest.Q = $"name = '{fileName}' and trashed = false";

        listRequest.Send().OnDone += fileList =>
        {
            if (fileList == null || fileList.Files == null || fileList.Files.Count == 0)
            {
                Debug.Log("Файл не найден. Создание нового...");
                CreateOrUpdateFile(fileName, byteContent, mimeType);
            }
            else
            {
                var existingFile = fileList.Files.FirstOrDefault();
                if (existingFile != null)
                {
                    Debug.Log($"Файл найден. ID: {existingFile.Id}. Обновление...");
                    lastUploadedFileId = existingFile.Id; // Сохраняем ID
                    CreateOrUpdateFile(fileName, byteContent, mimeType, existingFile.Id);
                }
                else
                {
                    Debug.LogWarning("Файл найден в списке, но объект файла оказался null.");
                }
            }

            onDone?.Invoke();
        };
    }

    public static void UploadFileToGoogleDrive()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, LocalFileSaver.SaveFileName);
            UploadOrUpdateFileFromDisk(filePath, () =>
            {
                Debug.Log("Файл синхронизирован с Google Drive");
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка при синхронизации с Google Drive: " + ex.Message);
        }
    }

    private static void CreateOrUpdateFile(string fileName, byte[] byteContent, string mimeType, string fileId = null)
    {
        var file = new UnityGoogleDrive.Data.File
        {
            Name = fileName,
            Content = byteContent
        };

        if (string.IsNullOrEmpty(fileId))
        {
            var createRequest = GoogleDriveFiles.Create(file);
            createRequest.Send().OnDone += createdFile =>
            {
                lastUploadedFileId = createdFile.Id;
                Debug.Log($"Файл создан на Google Drive. ID: {createdFile.Id}");
            };
        }
        else
        {
            var updateRequest = GoogleDriveFiles.Update(fileId, file, mimeType);
            updateRequest.Send().OnDone += _ =>
            {
                Debug.Log("Файл обновлён на Google Drive.");
            };
        }
    }

}
