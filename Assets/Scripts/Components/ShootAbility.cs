using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    [Header("Shooting")]
    public string BulletPoolTag = "Bullet";
    public Transform FirePoint;
    public float ShotDelay = 0.2f;
    public float ShootingForce = 5f;
    private float _shootTime = float.MinValue;

    [Header("Effects")]
    public string ShootEffectPoolTag = "ShootEffect";

    public void Execute()
    {
        if (Time.time < _shootTime + ShotDelay) return;

        _shootTime = Time.time;

        Transform spawnPoint = FirePoint != null ? FirePoint : transform;

        // Воспроизведение эффекта выстрела из пула
        GameObject effect = ObjectPool.Instance.SpawnFromPool(ShootEffectPoolTag, spawnPoint.position, spawnPoint.rotation);

        // Получение пули из пула
        GameObject bullet = ObjectPool.Instance.SpawnFromPool(BulletPoolTag, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // сброс скорости
            rb.AddForce(spawnPoint.forward * ShootingForce, ForceMode.Impulse);
        }
    }
}