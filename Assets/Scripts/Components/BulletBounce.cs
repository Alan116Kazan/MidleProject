using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; } = new List<GameObject>();
    public float Duration = 10f;

    public bool IsActive => BounceAbilityManager.IsBounceActive;

    public void Execute()
    {
        if (BounceAbilityManager.IsBounceActive)
        {
            Debug.Log("Bounce уже активен!");
            return;
        }

        BounceAbilityManager.IsBounceActive = true;
        BounceAbilityManager.BounceEndTime = Time.time + Duration;

        Debug.Log($"Perk активирован на {Duration} секунд");

        CoroutineRunner.Instance.StartCoroutine(DeactivateBounce());
        Destroy(gameObject);
    }

    private IEnumerator DeactivateBounce()
    {
        yield return new WaitForSeconds(Duration);
        BounceAbilityManager.IsBounceActive = false;
        Debug.Log("Perk деактивирован");
    }
}
