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
        // ���������� ���������� ��� �������� ������ �����.
        dstManager.AddComponentData(entity, new InputData());

        // ���������� ���������� ��� �������� ������ ��������.
        dstManager.AddComponentData(entity, new MoveData
        {
            Speed = speed / 100            // ����� �������� ��� ���������������
        });

        // ���� ������ ShootAction � �� ��������� IAbility,
        // ��������� ������-��������� ��� ��������.
        if (ShootAction != null && ShootAction is IAbility)
        {
            dstManager.AddComponentData(entity, new ShootData
            {
                ShootingForce = shootingForce
            });
        }

        // ��������� ��������� � ����������� �����.
        dstManager.AddComponentData(entity, new DashData
        {
            DashDelay = dashDelay,
            DashDistance = dashDistance,
            LastDashTime = float.MinValue
        });
    }
}

// ��������� ��� �������� ������ �����.
public struct InputData : IComponentData
{
    public float2 Move;
    public float Shoot;
    public float Dash;
}

// ��������� ��� �������� ������ ��������.
public struct MoveData : IComponentData
{
    public float Speed;
}

public struct ShootData : IComponentData
{
    public float ShootingForce;  // ����, � ������� ���� ����� ��������� (�������)
}

public struct DashData : IComponentData
{
    public float DashDelay;
    public float DashDistance;
    public float LastDashTime;
}