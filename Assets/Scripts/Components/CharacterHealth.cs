using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public Settings settings;
    public Text healthText;

    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Max(0, value);
            UpdateHealthUI();

            if (_health <= 0)
                Destroy(gameObject);

            SaveToLocalAndUpload();
        }
    }

    private void Start()
    {
        Health = settings.HeroHealth;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = _health.ToString();
        }
        else
        {
            Debug.LogWarning("Health UI �� ��������!");
        }
    }

    private void SaveToLocalAndUpload()
    {
        try
        {
            var playerStats = new PlayerStats { Health = _health };
            string json = JsonUtility.ToJson(playerStats, true);

            LocalFileSaver.SaveToLocal(json);
            Debug.Log($"������ ��������� ��� ��������� � ������� �������� {_health}");
            GoogleDriveTools.UploadFileToGoogleDrive();
        }
        catch (Exception ex)
        {
            Debug.LogError("������ ��� ���������� ��� �������� �����: " + ex.Message);
        }
    }
}