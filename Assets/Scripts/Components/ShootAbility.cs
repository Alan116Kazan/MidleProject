using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    [Header("Shooting")]
    [SerializeField] private string _bulletPoolTag = "Bullet";
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _shotDelay = 0.2f;
    [SerializeField] private float _shootingForce = 5f;

    [Header("Effects")]
    [SerializeField] private string _shootEffectPoolTag = "ShootEffect";

    private float _nextShotTime;

    public void Execute()
    {
        if (Time.time < _nextShotTime) return;

        _nextShotTime = Time.time + _shotDelay;

        var spawnPoint = _firePoint ? _firePoint : transform;

        SpawnEffect(spawnPoint);
        SpawnBullet(spawnPoint);
    }

    private void SpawnEffect(Transform spawnPoint)
    {
        ObjectPool.Instance.SpawnFromPool(
            _shootEffectPoolTag,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }

    private void SpawnBullet(Transform spawnPoint)
    {
        var bullet = ObjectPool.Instance.SpawnFromPool(
            _bulletPoolTag,
            spawnPoint.position,
            spawnPoint.rotation
        );

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(spawnPoint.forward * _shootingForce, ForceMode.Impulse);
        }
    }
}
