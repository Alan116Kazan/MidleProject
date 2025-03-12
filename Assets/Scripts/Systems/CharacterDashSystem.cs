using Unity.Entities;
using UnityEngine;

public class CharacterDashSystem : ComponentSystem
{
    private EntityQuery _dashQuery;

    protected override void OnCreate()
    {
        // ������ ��� ���������, ������� InputData, DashData � Transform.
        _dashQuery = GetEntityQuery(
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<DashData>(),
            ComponentType.ReadOnly<Transform>()
        );
    }

    protected override void OnUpdate()
    {
        float currentTime = (float)World.Time.ElapsedTime; // �������� ������� ����� � ECS.

        Entities.With(_dashQuery).ForEach((Entity entity, Transform transform, ref InputData inputData, ref DashData dashData) =>
        {
            // ���� ������ ����� ������ � ������ ���������� ������� � �������� �����.
            if (inputData.Dash > 0f && currentTime > dashData.LastDashTime + dashData.DashDelay)
            {
                // ���������� ��������� ������ �� dashDistance.
                transform.position += transform.forward * dashData.DashDistance;
                dashData.LastDashTime = currentTime; // ��������� ����� ���������� �����.
            }
            // ���������� �������� �����, ����� �������� ���������� ������������ �� ���������� �������.
            inputData.Dash = 0f;
        });
    }
}
