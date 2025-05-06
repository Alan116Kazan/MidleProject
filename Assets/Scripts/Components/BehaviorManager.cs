using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

public class BehaviorManager : MonoBehaviour, IConvertGameObjectToEntity
{
    // —писок дл€ инспектора Ч Unity не поддерживает сериализацию интерфейсов
    public List<MonoBehaviour> behaviours;

    // јктивное поведение (выбираетс€ системой)
    public IBehaviour activeBahaviour;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<AiAgent>(entity);
    }

    void Start()
    {
        // јвтоматически находим все компоненты, реализующие IBehaviour
        behaviours = GetComponents<MonoBehaviour>()
                     .Where(b => b is IBehaviour)
                     .ToList();
    }
}

public struct AiAgent : IComponentData
{

}