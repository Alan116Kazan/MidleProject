using System;
using UnityEngine;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;

public static class GoogleDriveFileEditor
{
    public static void CreateFile(string fileName, byte[] content, string mimeType, Action onDone = null)
    {
        var file = new File { Name = fileName, Content = content };
        GoogleDriveFiles.Create(file).Send().OnDone += createdFile =>
        {
            Debug.Log($"Файл создан: ID = {createdFile.Id}");
            onDone?.Invoke();
        };
    }

    public static void UpdateFile(string fileId, string fileName, byte[] content, string mimeType, Action onDone = null)
    {
        var file = new File { Name = fileName, Content = content };
        GoogleDriveFiles.Update(fileId, file, mimeType).Send().OnDone += _ =>
        {
            Debug.Log("Файл обновлён.");
            onDone?.Invoke();
        };
    }
}