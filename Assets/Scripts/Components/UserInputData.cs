using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    public float _speed;

    public MonoBehaviour ShootAction;

    public float dashDelay = 1f;      // Задержка между рывками (в секундах)
    public float dashDistance = 1f;   // Расстояние рывка (например, 1 метр)

    public float shootingForce = 5f;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Добавление компонента для хранения данных ввода.
        dstManager.AddComponentData(entity, new InputData());

        // Добавление компонента для хранения данных движения.
        dstManager.AddComponentData(entity, new MoveData
        {
            Speed = _speed / 100            // Делим скорость для масштабирования (настройка по требованию)
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
    public float2 Move;     // Вектор направления движения (x и y компоненты)
    public float Shoot;     // Значение для стрельбы (например, 1 при нажатии кнопки)
    public float Dash; // Если значение > 0, значит нажата кнопка рывка (CTRL)
}

// Компонент для хранения данных движения.
public struct MoveData : IComponentData
{
    public float Speed;     // Скорость перемещения объекта
}

// Пустой маркер-компонент для обозначения возможности стрельбы.
public struct ShootData : IComponentData
{
    public float ShootingForce;  // Сила, с которой пуля будет выброшена (импульс)
}

public struct DashData : IComponentData
{
    public float DashDelay;      // Задержка между рывками (в секундах)
    public float DashDistance;   // Расстояние, на которое производится рывок
    public float LastDashTime;   // Время последнего выполненного рывка
}