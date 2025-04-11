using System;
using UnityEngine;
public class GameDataManager
{
    private readonly string _fileName;

    public GameDataManager(string fileName)
    {
        _fileName = fileName;
    }

    public void LoadPlayerStats(Action<PlayerStats> onLoaded, Action onError = null)
    {
        GoogleDriveFinder.FindFileIdByName(_fileName, fileId =>
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                Debug.Log("���� ������ �� Google Drive. ���������...");
                GoogleDriveDownloader.DownloadFile(fileId, jsonContent =>
                {
                    try
                    {
                        var stats = JsonUtility.FromJson<PlayerStats>(jsonContent);
                        if (stats != null)
                            onLoaded?.Invoke(stats);
                        else
                            onError?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("������ ��� ��������������: " + ex.Message);
                        onError?.Invoke();
                    }
                });
            }
            else
            {
                Debug.Log("���� �� ������ �� Google Drive.");
                onError?.Invoke();
            }
        });
    }

    public void SavePlayerStats(PlayerStats stats)
    {
        LocalFileSaver.SaveToLocalAndUpload(stats);
    }
}