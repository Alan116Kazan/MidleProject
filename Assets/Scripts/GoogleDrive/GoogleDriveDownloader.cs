using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityGoogleDrive;

public static class GoogleDriveDownloader
{
    public static async Task<string> DownloadFileAsync(string fileId)
    {
        if (string.IsNullOrEmpty(fileId))
        {
            Debug.LogError("File ID is null or empty.");
            return null;
        }

        var tcs = new TaskCompletionSource<string>();
        GoogleDriveFiles.Download(fileId).Send().OnDone += downloadedFile =>
        {
            if (downloadedFile?.Content == null)
            {
                Debug.LogError("Не удалось загрузить файл.");
                tcs.SetResult(null);
                return;
            }

            string json = Encoding.UTF8.GetString(downloadedFile.Content);
            string filePath = Path.Combine(Application.persistentDataPath, LocalFileSaver.SaveFileName);
            File.WriteAllText(filePath, json);

            Debug.Log($"Файл загружен и сохранён: {filePath}");
            tcs.SetResult(json);
        };

        return await tcs.Task;
    }
}
