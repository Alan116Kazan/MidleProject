using Assets.Scripts.Components.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public List<MonoBehaviour> levelUpActions;

    public GameObject InventoryUiRoot;

    private int currentLevel = 1;
    public int score = 0;
    public int scoreToNextLevel = 20;

    private List<IItem> _items;
 
    public int CurrentLevel => currentLevel;

    public void Score(int scoreAmount)
    {
        score += scoreAmount;
        if (score >= scoreToNextLevel) LevelUp();
    }

    private void LevelUp()
    {
        currentLevel++;
        scoreToNextLevel *= 2;

        foreach (var action in levelUpActions)
        {
            if (!(action is ILevelUp levelUp)) return;
            levelUp.LevelUp(this, currentLevel);
        }
    }
}
