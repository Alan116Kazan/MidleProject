using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public Settings settings;
    public ShootAbility ShootAbility;
    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            WriteStatistics();
            if (_health <= 0 ) Destroy(this.gameObject);
        }
    }

    private void WriteStatistics()
    {
        var jsonString = JsonUtility.ToJson(ShootAbility.stats);
        Debug.Log(jsonString);

        Debug.Log("������ �������� ���������� �� Google Drive...");
        GoogleDriveTools.Upload(jsonString, () =>
        {
            Debug.Log("���������� ������ ������� ��������� �� Google Drive.");
        });
        PlayerPrefs.SetString("Stats", jsonString);
    }

    private void Start()
    {
        Health = settings.HeroHealth;
    }
}
