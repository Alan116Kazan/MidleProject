using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Zenject;
using Assets.Scripts.Components.Interfaces;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    [Inject] private IGameSettingsProvider _settingsProvider;

    public MonoBehaviour ShootAction;

    public float dashDelay = 1f;
    public float dashDistance = 1f;

    public float shootingForce = 5f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // В момент конвертации берём скорость из конфигурации
        float speed = _settingsProvider.Speed / 100f;

        dstManager.AddComponentData(entity, new InputData());

        dstManager.AddComponentData(entity, new MoveData
        {
            Speed = speed
        });

        if (ShootAction != null && ShootAction is IAbility)
        {
            dstManager.AddComponentData(entity, new ShootData
            {
                ShootingForce = shootingForce
            });
        }

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