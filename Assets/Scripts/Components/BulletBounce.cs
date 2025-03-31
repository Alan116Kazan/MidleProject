using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; }
    public float Duration = 10f; // ����������� ��������� 10 ������

    public void Execute()
    {
        // ���������� ����������� �������
        BounceAbilityManager.IsBounceActive = true;
        BounceAbilityManager.BounceEndTime = Time.time + Duration;

        Debug.Log($"Perk ����������� �� {Duration} ������");

        // ����� Duration ������ ��������� �����������
        StartCoroutine(DeactivateBounce());

        Destroy(gameObject);
    }

    private IEnumerator DeactivateBounce()
    {
        yield return new WaitForSeconds(Duration);
        BounceAbilityManager.IsBounceActive = false;
    }
}