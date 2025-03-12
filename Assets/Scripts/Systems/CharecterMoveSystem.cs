using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class CharecterMoveSystem : ComponentSystem
{
    private EntityQuery _moveQuery;

    protected override void OnCreate()
    {
        // Запрос для сущностей, имеющих компоненты InputData, MoveData и Transform.
        _moveQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
                                    ComponentType.ReadOnly<MoveData>(),
                                    ComponentType.ReadOnly<Transform>());
    }

    protected override void OnUpdate()
    {
        // Перебираем все сущности из запроса.
        Entities.With(_moveQuery).ForEach(
            (Entity entity, Transform transform, ref InputData inputData, ref MoveData move) =>
            {
                // Обновляем позицию на основе ввода и скорости.
                var pos = transform.position;
                pos += new Vector3(inputData.Move.x * move.Speed, 0, inputData.Move.y * move.Speed);
                transform.position = pos;

                // Если есть направление движения (вектор не равен нулю)
                if (inputData.Move.x != 0 || inputData.Move.y != 0)
                {
                    // Создаем вектор направления движения в пространстве (игнорируем ось Y).
                    Vector3 moveDirection = new Vector3(inputData.Move.x, 0, inputData.Move.y);

                    // Поворачиваем персонажа так, чтобы он смотрел в направлении движения.
                    // Quaternion.LookRotation вычисляет нужную ротацию по вектору направления.
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
            });
    }
}