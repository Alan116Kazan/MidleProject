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
        // Получаем строку, сохранённую в PlayerPrefs под ключом "Stats".
        var jsonString = PlayerPrefs.GetString("Stats");

        // Если строка не пуста, значит ранее были сохранены данные игрока.
        if (!jsonString.Equals(string.Empty, System.StringComparison.Ordinal))
        {
            // Преобразуем JSON-строку в объект PlayerStats.
            stats = JsonUtility.FromJson<PlayerStats>(jsonString);
        }
        else
        {
            // Если сохранённой строки нет, создаём новый объект статистики.
            stats = new PlayerStats();
        }
    }

    public void Execute()
    {
        // Проверяем, прошло ли достаточно времени с момента последнего выстрела.
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

            // Увеличиваем счетчик выстрелов.
            stats.shotsCount++;
        }
        else
        {
            Debug.LogError("[Shoot Ability] No bullet prefab link!");
        }
    }

}