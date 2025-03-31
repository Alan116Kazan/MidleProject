using System.Collections.Generic;
using UnityEngine;

public class ApplyDamage : MonoBehaviour, IAbilityTarget
{
    public int Damage = 10;
    public List<GameObject> Targets { get; set; }

    // Словарь для отслеживания времени последнего удара по каждой цели
    private Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();

    // Метод Execute вызывается для нанесения урона
    public void Execute()
    {
        float currentTime = Time.time;
        foreach (var target in Targets)
        {
            // Если цель не была ранее повреждена, или прошла секунда с предыдущего удара
            if (!lastDamageTime.ContainsKey(target) || currentTime - lastDamageTime[target] >= 1f)
            {
                var health = target.GetComponent<CharacterHealth>();
                if (health != null)
                {
                    health.Health -= Damage;
                    // Обновляем время последнего удара по этой цели
                    lastDamageTime[target] = currentTime;
                }
            }
        }
    }
}