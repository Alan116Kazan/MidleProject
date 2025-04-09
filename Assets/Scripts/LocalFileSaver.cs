using System.IO;
using UnityEngine;

public static class LocalFileSaver
{
    public const string SaveFileName = "GameData.json";

    // ����� ��� ���������� ������ �� ��������� ����������
    public static void SaveToLocal(string obj)
    {
        string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

        // ��������� ���� �� ��������� ����
        File.WriteAllText(filePath, obj);

        Debug.Log("���� �������� �� ��������� ����������: " + filePath);
    }
}