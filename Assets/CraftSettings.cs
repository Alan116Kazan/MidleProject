using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftSettings", menuName = "Create Craft Data")]
public class CraftSettings : ScriptableObject
{
    public List<CraftCombination> combinations;
}

[Serializable]
public class CraftCombination
{
    public List<string> sources;
    public GameObject result;
}
