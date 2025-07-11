using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; }

    [Header("Effects")]
    public string CollisionEffectTag = "ImpactEffect";

    private Vector3 _lastHitPoint;
    private Vector3 _lastHitNormal;
    private bool _hasCollision;

    public void Execute()
    {
        if (BounceAbilityManager.IsBounceActive && Time.time < BounceAbilityManager.BounceEndTime)
            return;

        if (_hasCollision && !string.IsNullOrEmpty(CollisionEffectTag))
        {
            ObjectPool.Instance.SpawnFromPool(
                CollisionEffectTag,
                _lastHitPoint,
                Quaternion.LookRotation(_lastHitNormal)
            );
        }

        _hasCollision = false; // —брос

        gameObject.SetActive(false); // ¬место Destroy
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        _lastHitPoint = contact.point;
        _lastHitNormal = contact.normal;
        _hasCollision = true;

        Execute();
    }
}
