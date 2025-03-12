using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class CharecterMoveSystem : ComponentSystem
{
    private EntityQuery _moveQuery;

    protected override void OnCreate()
    {
        // ������ ��� ���������, ������� ���������� InputData, MoveData � Transform.
        _moveQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
                                    ComponentType.ReadOnly<MoveData>(),
                                    ComponentType.ReadOnly<Transform>());
    }

    protected override void OnUpdate()
    {
        // ���������� ��� �������� �� �������.
        Entities.With(_moveQuery).ForEach(
            (Entity entity, Transform transform, ref InputData inputData, ref MoveData move) =>
            {
                // ��������� ������� �� ������ ����� � ��������.
                var pos = transform.position;
                pos += new Vector3(inputData.Move.x * move.Speed, 0, inputData.Move.y * move.Speed);
                transform.position = pos;

                // ���� ���� ����������� �������� (������ �� ����� ����)
                if (inputData.Move.x != 0 || inputData.Move.y != 0)
                {
                    // ������� ������ ����������� �������� � ������������ (���������� ��� Y).
                    Vector3 moveDirection = new Vector3(inputData.Move.x, 0, inputData.Move.y);

                    // ������������ ��������� ���, ����� �� ������� � ����������� ��������.
                    // Quaternion.LookRotation ��������� ������ ������� �� ������� �����������.
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
            });
    }
}