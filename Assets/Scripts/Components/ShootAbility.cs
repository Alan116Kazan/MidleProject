using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    public GameObject Bullet;
    public float ShotDelay;
    public float ShootingForce = 5f;
    private float _shootTime = float.MinValue;

    public PlayerStats stats;

    private void Start()
    {
        // �������� ������, ���������� � PlayerPrefs ��� ������ "Stats".
        var jsonString = PlayerPrefs.GetString("Stats");

        // ���� ������ �� �����, ������ ����� ���� ��������� ������ ������.
        if (!jsonString.Equals(string.Empty, System.StringComparison.Ordinal))
        {
            // ����������� JSON-������ � ������ PlayerStats.
            stats = JsonUtility.FromJson<PlayerStats>(jsonString);
        }
        else
        {
            // ���� ���������� ������ ���, ������ ����� ������ ����������.
            stats = new PlayerStats();
        }
    }

    public void Execute()
    {
        // ���������, ������ �� ���������� ������� � ������� ���������� ��������.
        if (Time.time < _shootTime + ShotDelay) return;

        _shootTime = Time.time;

        if (Bullet != null)
        {
            GameObject newBullet = Instantiate(Bullet, transform.position, transform.rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * ShootingForce, ForceMode.Impulse);
            }

            // ����������� ������� ���������.
            stats.shotsCount++;
        }
        else
        {
            Debug.LogError("[Shoot Ability] No bullet prefab link!");
        }
    }

}