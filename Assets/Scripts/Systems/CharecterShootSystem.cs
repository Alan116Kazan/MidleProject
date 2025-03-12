using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class CharecterShootSystem : ComponentSystem
{
    private EntityQuery _shootQuery;

    protected override void OnCreate()
    {
        // Создаем запрос сущностей, у которых есть:
        // - InputData: для проверки ввода стрельбы
        // - ShootData: чтобы убедиться, что сущность может стрелять
        // - UserInputData: для доступа к ссылке на компонент способности стрельбы
        _shootQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
                                     ComponentType.ReadOnly<ShootData>(),
                                     ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnUpdate()
    {
        // Перебираем все сущности, удовлетворяющие запросу.
        Entities.With(_shootQuery).ForEach(
            (Entity entity, UserInputData inputData, ref InputData input) =>
            {
                // Если нажата кнопка стрельбы и компонент способности стрельбы не равен null,
                // приводим его к IAbility и вызываем метод Exicute().
                if (input.Shoot > 0f && inputData.ShootAction != null && inputData.ShootAction is IAbility ability)
                {
                    ability.Exicute();
                }
            });
    }
}