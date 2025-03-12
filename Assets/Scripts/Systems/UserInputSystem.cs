using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class UserInputSystem : ComponentSystem
{
    private EntityQuery _inputQuery;
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _dashAction; // ����� InputAction ��� �����

    private float2 _moveInput;   // ��������� ������ ��������, ���������� �� ������������
    private float _shootInput;   // ��������� �������� ��� �������� (��������, 1 ��� 0)
    private float _dashInput; // ��������� ���������� ��� �����

    protected override void OnCreate()
    {
        // ������ ��� ���� ���������, ������� ��������� InputData.
        _inputQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>());
    }

    protected override void OnStartRunning()
    {
        // ������� �������� ����� ��� ��������.
        // ������������ ������� (rightStick) � ���������� (�������� Dpad: WASD).
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<keyboard>/w")
            .With("Down", "<keyboard>/s")
            .With("Left", "<keyboard>/a")
            .With("Right", "<keyboard>/d");

        // ������������� �� ������� ��������, ����� ��������� _moveInput.
        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.started += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.Enable();

        // ������� �������� ����� ��� ��������, ��������� ������� "������".
        _shootAction = new InputAction("shoot", binding: "<keyboard>/space");
        _shootAction.performed += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.started += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.canceled += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.Enable();

        // �������� ��� �����(CTRL)
        _dashAction = new InputAction("dash", binding: "<keyboard>/ctrl");
        _dashAction.performed += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.started += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.canceled += context => { _dashInput = 0f; };
        _dashAction.Enable();
    }

    protected override void OnStopRunning()
    {
        // ��������� �������� ����� ��� ��������� �������.
        _moveAction.Disable();
        _shootAction.Disable();
        _dashAction.Disable();
    }

    protected override void OnUpdate()
    {
        // ��������� ��������� InputData � ���� ���������, ����� �������� ������� �������� �����.
        Entities.With(_inputQuery).ForEach(
            (Entity entity, ref InputData inputData) =>
            {
                inputData.Move = _moveInput;
                inputData.Shoot = _shootInput;
                inputData.Dash = _dashInput;
            });
    }
}
