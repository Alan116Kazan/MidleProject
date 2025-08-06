using System.Linq;
using System.Threading.Tasks;
using UnityGoogleDrive;

public static class GoogleDriveFinder
{
    public static async Task<string> FindFileIdByNameAsync(string fileName)
    {
        var tcs = new TaskCompletionSource<string>();
        var listRequest = GoogleDriveFiles.List();
        listRequest.Q = $"name = '{fileName}' and trashed = false";

        listRequest.Send().OnDone += fileList =>
        {
            string fileId = fileList?.Files?.FirstOrDefault()?.Id;
            tcs.SetResult(fileId);
        };

        return await tcs.Task;
    }
}
