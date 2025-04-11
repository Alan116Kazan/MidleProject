using System;
using System.IO;
using UnityEngine;

public static class GoogleDriveUploader
{
    public static void UploadFile(string localFilePath, Action onDone = null)
    {
        if (!File.Exists(localFilePath))
        {
            Debug.LogError("���� �� ������: " + localFilePath);
            return;
        }

        byte[] byteContent = File.ReadAllBytes(localFilePath);
        string fileName = Path.GetFileName(localFilePath);
        string mimeType = "application/json";

        GoogleDriveFinder.FindFileIdByName(fileName, fileId =>
        {
            if (string.IsNullOrEmpty(fileId))
            {
                Debug.Log("���� �� ������. �������� ������...");
                GoogleDriveFileEditor.CreateFile(fileName, byteContent, mimeType, onDone);
            }
            else
            {
                Debug.Log($"���� ������. ID: {fileId}. ����������...");
                GoogleDriveFileEditor.UpdateFile(fileId, fileName, byteContent, mimeType, onDone);
            }
        });
    }
}