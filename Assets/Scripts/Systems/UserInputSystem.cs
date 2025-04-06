using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class UserInputSystem : ComponentSystem
{
    private EntityQuery _inputQuery;
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _dashAction;

    private float2 _moveInput;
    private float _shootInput;
    private float _dashInput;

    protected override void OnCreate()
    {
        _inputQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>());
    }

    protected override void OnStartRunning()
    {
        InitializeMoveAction();
        InitializeShootAction();
        InitializeDashAction();
    }

    private void InitializeMoveAction()
    {
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<keyboard>/w")
            .With("Down", "<keyboard>/s")
            .With("Left", "<keyboard>/a")
            .With("Right", "<keyboard>/d");

        _moveAction.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _moveAction.started += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _moveAction.canceled += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _moveAction.Enable();
    }

    private void InitializeShootAction()
    {
        _shootAction = new InputAction("shoot", binding: "<keyboard>/space");
        _shootAction.performed += ctx => _shootInput = ctx.ReadValue<float>();
        _shootAction.started += ctx => _shootInput = ctx.ReadValue<float>();
        _shootAction.canceled += ctx => _shootInput = ctx.ReadValue<float>();
        _shootAction.Enable();
    }

    private void InitializeDashAction()
    {
        _dashAction = new InputAction("dash", binding: "<keyboard>/ctrl");
        _dashAction.performed += ctx => _dashInput = ctx.ReadValue<float>();
        _dashAction.started += ctx => _dashInput = ctx.ReadValue<float>();
        _dashAction.canceled += ctx => _dashInput = 0f;
        _dashAction.Enable();
    }

    protected override void OnStopRunning()
    {
        DisableActions();
    }

    private void DisableActions()
    {
        _moveAction.Disable();
        _shootAction.Disable();
        _dashAction.Disable();
    }

    protected override void OnUpdate()
    {
        UpdateInputData();
    }

    private void UpdateInputData()
    {
        Entities.With(_inputQuery).ForEach((Entity entity, ref InputData inputData) =>
        {
            inputData.Move = _moveInput;
            inputData.Shoot = _shootInput;
            inputData.Dash = _dashInput;
        });
    }
}