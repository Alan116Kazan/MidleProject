using System;
using System.Linq;
using UnityGoogleDrive;

public static class GoogleDriveFinder
{
    public static void FindFileIdByName(string fileName, Action<string> onResult)
    {
        var listRequest = GoogleDriveFiles.List();
        listRequest.Q = $"name = '{fileName}' and trashed = false";

        listRequest.Send().OnDone += fileList =>
        {
            string fileId = fileList?.Files?.FirstOrDefault()?.Id;
            onResult?.Invoke(fileId);
        };
    }
}