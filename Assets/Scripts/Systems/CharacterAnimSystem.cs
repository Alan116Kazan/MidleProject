using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CharacterAnimSystem : ComponentSystem
{
    private EntityQuery _animQuery;

    protected override void OnCreate()
    {
        _animQuery = GetEntityQuery(
            ComponentType.ReadOnly<AnimData>(),
            ComponentType.ReadOnly<Animator>(),
            ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<UserInputData>()
        );
    }

    protected override void OnUpdate()
    {
        Entities.With(_animQuery).ForEach(
            (Entity entity, ref InputData move, Animator animator, UserInputData inputData) =>
            {
                // Анимация движения
                animator.SetBool(inputData.moveAnimHash, math.lengthsq(move.Move) > 0.01f);

                if (!string.IsNullOrEmpty(inputData.moveAnimSpeedHash))
                {
                    animator.SetFloat(inputData.moveAnimSpeedHash, inputData.CharacterSpeed * math.lengthsq(move.Move));
                }

                // Анимация стрельбы
                if (move.Shoot > 0f && !string.IsNullOrEmpty(inputData.shootAnimTriggerHash))
                {
                    animator.SetTrigger(inputData.shootAnimTriggerHash);
                }
            });
    }
}
