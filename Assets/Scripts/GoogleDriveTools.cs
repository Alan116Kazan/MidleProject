using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;

public class GoogleDriveTools
{
    public static List<File> FileList()
    {
        List<File> output = new List<File>();
        GoogleDriveFiles.List().Send().OnDone += fileList => { output = fileList.Files; };
        return output;
    }

    public static File Upload(string obj, Action onDone)
    {
        var file = new UnityGoogleDrive.Data.File
        {
            Name = "GameData.json",
            Content = Encoding.ASCII.GetBytes(obj)
        };

        GoogleDriveFiles.Create(file).Send().OnDone += createdFile =>
        {
            Debug.Log("Файл создан на Google Drive.");
            onDone?.Invoke();
        };

        return file;
    }

    public static File Download(string fileId)
    {
        File output = new File();
        // Исправлено лямбда-выражение с использованием оператора =>
        GoogleDriveFiles.Download(fileId).Send().OnDone += downloadedFile => { output = downloadedFile; };
        return output;
    }
}