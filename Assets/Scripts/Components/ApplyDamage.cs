using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour, IAbilityTarget
{
    public int Damage = 10;
    public List<GameObject> Targets { get; set; }

    // ������� ��� ������������ ������� ���������� ����� �� ������ ����
    private Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();

    // ����� Execute ���������� ��� ��������� �����
    public void Execute()
    {
        float currentTime = Time.time;
        foreach (var target in Targets)
        {
            // ���� ���� �� ���� ����� ����������, ��� ������ ������� � ����������� �����
            if (!lastDamageTime.ContainsKey(target) || currentTime - lastDamageTime[target] >= 1f)
            {
                var health = target.GetComponent<CharacterHealth>();
                if (health != null)
                {
                    health.Health -= Damage;
                    // ��������� ����� ���������� ����� �� ���� ����
                    lastDamageTime[target] = currentTime;
                }
            }
        }
    }
}