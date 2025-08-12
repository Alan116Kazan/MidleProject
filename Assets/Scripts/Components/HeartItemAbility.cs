using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItemAbility : MonoBehaviour, IAbilityTarget, ICraftable
{
    private List<GameObject> _targets = new List<GameObject>();
    public string _name = "";
    public List<GameObject> Targets
    {
        get => _targets;
        set => _targets = value;
    }

    public string Name => _name;

    public void Execute()
    {
        foreach (var target in Targets)
        {
            var character = target.GetComponent<CharacterHealth>();
            if (character == null) return;
            character.Health += 12;
        }
        Destroy(this.gameObject);
    }
}