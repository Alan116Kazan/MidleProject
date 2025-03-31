using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; }
    public float Duration = 10f; // Способность действует 10 секунд

    public void Execute()
    {
        // Активируем способность отскока
        BounceAbilityManager.IsBounceActive = true;
        BounceAbilityManager.BounceEndTime = Time.time + Duration;

        Debug.Log($"Perk активирован на {Duration} секунд");

        // После Duration секунд отключаем способность
        StartCoroutine(DeactivateBounce());

        Destroy(gameObject);
    }

    private IEnumerator DeactivateBounce()
    {
        yield return new WaitForSeconds(Duration);
        BounceAbilityManager.IsBounceActive = false;
    }
}