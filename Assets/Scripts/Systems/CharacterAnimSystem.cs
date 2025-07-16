using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimSystem : ComponentSystem
{
    private EntityQuery _animQuery;

    protected override void OnCreate()
    {
        _animQuery = GetEntityQuery(ComponentType.ReadOnly<AnimData>(), ComponentType.ReadOnly<Animator>());
    }

    protected override void OnUpdate()
    {
        // Перебираем все сущности из запроса.
        Entities.With(_animQuery).ForEach(
            (Entity entity, ref InputData move, Animator animator, UserInputData inputData) =>
            {
                animator.SetBool(inputData.moveAnimHash, Math.Abs(move.Move.x) > 0.01f || Math.Abs(move.Move.y) > 0.01f);

                if (inputData.moveAnimSpeedHash == String.Empty) return;

                animator.SetFloat(inputData.moveAnimSpeedHash, inputData.CharacterSpeed * math.distancesq(move.Move.x,move.Move.y));
            });
    }
}