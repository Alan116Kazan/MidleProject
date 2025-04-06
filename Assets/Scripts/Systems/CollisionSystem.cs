using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static CollisionAbility;

public class CollisionSystem : ComponentSystem
{
    private EntityQuery _collisionQuerry;
    private Collider[] _results = new Collider[50];

    protected override void OnCreate()
    {
        _collisionQuerry = GetEntityQuery(
            ComponentType.ReadOnly<ActorColliderData>(),
            ComponentType.ReadOnly<Transform>()
        );
    }

    protected override void OnUpdate()
    {
        var dtsManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entities.With(_collisionQuerry).ForEach((Entity entity, CollisionAbility abilityCollision, ref ActorColliderData colliderData) =>
        {
            if (abilityCollision == null || abilityCollision.gameObject == null)
                return; // Прерываем выполнение, если объект был уничтожен

            var gameObject = abilityCollision.gameObject;
            float3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;

            int size = 0;

            switch (colliderData.ColliderType)
            {
                case ColliderType.Sphere:
                    size = Physics.OverlapSphereNonAlloc(colliderData.SphereCenter + position,
                        colliderData.SphereRadius, _results);
                    break;
                case ColliderType.Capsule:
                    var center = ((colliderData.CapsuleStart + position) + (colliderData.CapsuleEnd + position)) / 2f;
                    var point1 = colliderData.CapsuleStart + position;
                    var point2 = colliderData.CapsuleEnd + position;
                    point1 = (float3)(rotation * (point1 - center)) + center;
                    point2 = (float3)(rotation * (point2 - center)) + center;
                    size = Physics.OverlapCapsuleNonAlloc(point1, point2, colliderData.CapsuleRadius, _results);
                    break;
                case ColliderType.Box:
                    size = Physics.OverlapBoxNonAlloc(colliderData.BoxCenter + position,
                        colliderData.BoxHalfExtents, _results, colliderData.BoxOrientation * rotation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (size > 0)
            {
                // Если список collisions не инициализирован, создаём его
                if (abilityCollision.collisions == null)
                    abilityCollision.collisions = new List<Collider>();
                else
                    abilityCollision.collisions.Clear();

                // Добавляем только непустые коллайдеры из первых size элементов массива _results.
                foreach (var result in _results.Take(size))
                {
                    if (result != null)
                        abilityCollision.collisions.Add(result);
                }
                abilityCollision.Execute();
            }
        });
    }
}
