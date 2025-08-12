using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpHealth : MonoBehaviour, ILevelUp
{
    public int minLevel { get; set; }

    private CharacterHealth _health;
    public void LevelUp(CharacterData data, int level)
    {
        if (_health == null)
        {
            _health = GetComponent<CharacterHealth>();
            if (_health == null ) return;
        }

        if (data.CurrentLevel >= minLevel)
        {
            _health.Health += 10;
        }
    }

}
