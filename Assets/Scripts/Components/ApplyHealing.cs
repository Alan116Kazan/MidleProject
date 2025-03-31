using System.Collections.Generic;
using UnityEngine;

public class ApplyHealing : MonoBehaviour, IAbilityTarget
{
    public int HealAmount = 10;
    public List<GameObject> Targets { get; set; }

    public void Execute()
    {
        foreach (var target in Targets)
        {
            var health = target.GetComponent<CharacterHealth>();
            if (health != null) health.Health += HealAmount;
        }

        Destroy(gameObject);
    }
}