using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTarget : IAbility
{
    List<GameObject> Targets { get; set; }
}
