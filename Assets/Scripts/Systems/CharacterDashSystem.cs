using Unity.Entities;
using UnityEngine;

public class CharacterDashSystem : ComponentSystem
{
    private EntityQuery _dashQuery;

    protected override void OnCreate()
    {
        // Запрос для сущностей, имеющих InputData, DashData и Transform.
        _dashQuery = GetEntityQuery(
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<DashData>(),
            ComponentType.ReadOnly<Transform>()
        );
    }

    protected override void OnUpdate()
    {
        float currentTime = (float)World.Time.ElapsedTime; // Получаем текущее время в ECS.

        Entities.With(_dashQuery).ForEach((Entity entity, Transform transform, ref InputData inputData, ref DashData dashData) =>
        {
            // Если кнопка рывка нажата и прошло достаточно времени с прошлого рывка.
            if (inputData.Dash > 0f && currentTime > dashData.LastDashTime + dashData.DashDelay)
            {
                // Перемещаем персонажа вперед на dashDistance.
                transform.position += transform.forward * dashData.DashDistance;
                dashData.LastDashTime = currentTime; // Обновляем время последнего рывка.
            }
            // Сбрасываем значение рывка, чтобы избежать повторного срабатывания до следующего нажатия.
            inputData.Dash = 0f;
        });
    }
}
