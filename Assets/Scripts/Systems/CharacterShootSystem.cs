using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterShootSystem : ComponentSystem
{
    private EntityQuery _shootQuery;

    protected override void OnCreate()
    {
        _shootQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
                                     ComponentType.ReadOnly<ShootData>(),
                                     ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnUpdate()
    {
        Entities.With(_shootQuery).ForEach(
            (Entity entity, UserInputData inputData, ref InputData input) =>
            {
                if (input.Shoot > 0f)
                {
                    if (inputData.ShootAction == null)
                    {
                        Debug.LogWarning($"ShootAction равен нулю для сущности {entity}");
                        return;
                    }

                    if (inputData.ShootAction is IAbility ability)
                    {
                        ability.Execute();
                    }
                    else
                    {
                        Debug.LogWarning($"ShootAction не реализует интерфейс IAbility для сущности {entity}");
                    }
                }
            });
    }
}