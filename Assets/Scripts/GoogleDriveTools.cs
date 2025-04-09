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
            Debug.LogError("���� �� ������: " + localFilePath);
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
                Debug.Log("���� �� ������. �������� ������...");
                CreateOrUpdateFile(fileName, byteContent, mimeType);
            }
            else
            {
                var existingFile = fileList.Files.FirstOrDefault();
                if (existingFile != null)
                {
                    Debug.Log($"���� ������. ID: {existingFile.Id}. ����������...");
                    lastUploadedFileId = existingFile.Id; // ��������� ID
                    CreateOrUpdateFile(fileName, byteContent, mimeType, existingFile.Id);
                }
                else
                {
                    Debug.LogWarning("���� ������ � ������, �� ������ ����� �������� null.");
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
                Debug.Log("���� ��������������� � Google Drive");
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("������ ��� ������������� � Google Drive: " + ex.Message);
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
                Debug.Log($"���� ������ �� Google Drive. ID: {createdFile.Id}");
            };
        }
        else
        {
            var updateRequest = GoogleDriveFiles.Update(fileId, file, mimeType);
            updateRequest.Send().OnDone += _ =>
            {
                Debug.Log("���� ������� �� Google Drive.");
            };
        }
    }

}
