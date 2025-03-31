using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    public GameObject Bullet;      // Префаб пули
    public float ShotDelay;         // Задержка между выстрелами

    public float ShootingForce = 5f;

    private float _shootTime = float.MinValue;  // Время последнего выстрела

    public void Execute()
    {
        // Проверяем, прошло ли достаточно времени с прошлого выстрела.
        if (Time.time < _shootTime + ShotDelay) return;

        _shootTime = Time.time;  // Обновляем время последнего выстрела

        if (Bullet != null)
        {
            GameObject newBullet = Instantiate(Bullet, transform.position, transform.rotation);

            // Добавляем силу выстрела.
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * ShootingForce, ForceMode.Impulse);
            }
        }
        else
        {
            // Если префаб пули не задан, выводим сообщение об ошибке.
            Debug.LogError("[Shoot Ability] No bullet prefab link!");
        }
    }
}
