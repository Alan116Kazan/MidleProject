using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GiveItemPickUpAbility : MonoBehaviour, IAbilityTarget, IConvertGameObjectToEntity, IItem
{

    public GameObject _UIItem;
    public List<GameObject> Targets { get; set; }

    public GameObject UIItem => _UIItem;

    private Entity _entity;
    private EntityManager _dstmanager;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        _entity = entity;
        _dstmanager = dstManager;
    }

    public void Execute()
    {
        foreach (var target in Targets)
        {
            var character = target.GetComponent<CharacterData>();

            if (character == null) return;
            var item = Object.Instantiate(UIItem, character.InventoryUiRoot.transform, false);
            var ability = item.GetComponent<IAbilityTarget>();

            ability?.Targets.Add(target);

            Destroy(this.gameObject);
            _dstmanager.DestroyEntity(_entity);
        }
    }
}
