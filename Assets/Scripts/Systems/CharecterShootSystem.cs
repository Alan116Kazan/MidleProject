using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class CharecterShootSystem : ComponentSystem
{
    private EntityQuery _shootQuery;

    protected override void OnCreate()
    {
        // ������� ������ ���������, � ������� ����:
        // - InputData: ��� �������� ����� ��������
        // - ShootData: ����� ���������, ��� �������� ����� ��������
        // - UserInputData: ��� ������� � ������ �� ��������� ����������� ��������
        _shootQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
                                     ComponentType.ReadOnly<ShootData>(),
                                     ComponentType.ReadOnly<UserInputData>());
    }

    protected override void OnUpdate()
    {
        // ���������� ��� ��������, ��������������� �������.
        Entities.With(_shootQuery).ForEach(
            (Entity entity, UserInputData inputData, ref InputData input) =>
            {
                // ���� ������ ������ �������� � ��������� ����������� �������� �� ����� null,
                // �������� ��� � IAbility � �������� ����� Exicute().
                if (input.Shoot > 0f && inputData.ShootAction != null && inputData.ShootAction is IAbility ability)
                {
                    ability.Exicute();
                }
            });
    }
}