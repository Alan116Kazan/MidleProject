using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Zenject;
using Assets.Scripts.Components.Interfaces;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    [Inject] private IGameSettingsProvider _settingsProvider;

    public MonoBehaviour ShootAction;


    public float CharacterSpeed => _speed;

    public float dashDelay = 1f;
    public float dashDistance = 1f;

    public string moveAnimHash;
    public string moveAnimSpeedHash;
    public string shootAnimTriggerHash;

    public float shootingForce = 5f;

    private float _speed;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Задаём скорость из конфигурации
        _speed = _settingsProvider.Speed;

        dstManager.AddComponentData(entity, new InputData());

        dstManager.AddComponentData(entity, new MoveData
        {
            Speed = _speed / 100f
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

        if (!string.IsNullOrEmpty(moveAnimHash))
        {
            dstManager.AddComponentData(entity, new AnimData());
        }
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

public struct AnimData : IComponentData
{

}