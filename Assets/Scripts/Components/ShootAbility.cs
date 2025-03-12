using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    public GameObject _bullet;      // Префаб пули
    public float shotDelay;         // Задержка между выстрелами

    public float shootingForce = 5f;

    private float _shootTime = float.MinValue;  // Время последнего выстрела

    public void Exicute()
    {
        // Проверяем, прошло ли достаточно времени с прошлого выстрела.
        if (Time.time < _shootTime + shotDelay) return;

        _shootTime = Time.time;  // Обновляем время последнего выстрела

        if (_bullet != null)
        {
            GameObject newBullet = Instantiate(_bullet, transform.position, transform.rotation);

            // Добавляем силу выстрела.
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * shootingForce, ForceMode.Impulse);
            }
        }
        else
        {
            // Если префаб пули не задан, выводим сообщение об ошибке.
            Debug.LogError("[Shoot Ability] No bullet prefab link!");
        }
    }
}
