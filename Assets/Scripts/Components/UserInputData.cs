using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    public float speed;

    public MonoBehaviour ShootAction;

    public float dashDelay = 1f;
    public float dashDistance = 1f;

    public float shootingForce = 5f;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Добавление компонента для хранения данных ввода.
        dstManager.AddComponentData(entity, new InputData());

        // Добавление компонента для хранения данных движения.
        dstManager.AddComponentData(entity, new MoveData
        {
            Speed = speed / 100            // Делим скорость для масштабирования
        });

        // Если указан ShootAction и он реализует IAbility,
        // добавляем маркер-компонент для стрельбы.
        if (ShootAction != null && ShootAction is IAbility)
        {
            dstManager.AddComponentData(entity, new ShootData
            {
                ShootingForce = shootingForce
            });
        }

        // Добавляем компонент с параметрами рывка.
        dstManager.AddComponentData(entity, new DashData
        {
            DashDelay = dashDelay,
            DashDistance = dashDistance,
            LastDashTime = float.MinValue
        });
    }
}

// Компонент для хранения данных ввода.
public struct InputData : IComponentData
{
    public float2 Move;
    public float Shoot;
    public float Dash;
}

// Компонент для хранения данных движения.
public struct MoveData : IComponentData
{
    public float Speed;
}

public struct ShootData : IComponentData
{
    public float ShootingForce;  // Сила, с которой пуля будет выброшена (импульс)
}

public struct DashData : IComponentData
{
    public float DashDelay;
    public float DashDistance;
    public float LastDashTime;
}