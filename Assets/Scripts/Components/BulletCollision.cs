using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; }

    public void Execute()
    {
        // ���� ����������� ������� �������, ���� �� ������������
        if (BounceAbilityManager.IsBounceActive && Time.time < BounceAbilityManager.BounceEndTime)
        {
            return;
        }

        // ����� ���������� ����
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���������� ���� ��� ������������ � ����� ��������
        Execute();
    }
}