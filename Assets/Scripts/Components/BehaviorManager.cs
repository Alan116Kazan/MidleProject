using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BehaviorManager : MonoBehaviour, IConvertGameObjectToEntity
{
    public List<MonoBehaviour> behaviours;

    public IBehaviour activeBahaviour;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<AiAgent>(entity);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

public struct AiAgent : IComponentData
{

}