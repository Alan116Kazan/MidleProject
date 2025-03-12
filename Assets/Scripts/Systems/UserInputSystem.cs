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
    private InputAction _dashAction; // Новый InputAction для рывка

    private float2 _moveInput;   // Сохраняет вектор движения, полученный от пользователя
    private float _shootInput;   // Сохраняет значение для стрельбы (например, 1 или 0)
    private float _dashInput; // Локальная переменная для рывка

    protected override void OnCreate()
    {
        // Запрос для всех сущностей, имеющих компонент InputData.
        _inputQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>());
    }

    protected override void OnStartRunning()
    {
        // Создаем действие ввода для движения.
        // Используется геймпад (rightStick) и клавиатура (композит Dpad: WASD).
        _moveAction = new InputAction("move", binding: "<Gamepad>/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<keyboard>/w")
            .With("Down", "<keyboard>/s")
            .With("Left", "<keyboard>/a")
            .With("Right", "<keyboard>/d");

        // Подписываемся на события действия, чтобы обновлять _moveInput.
        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.started += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.Enable();

        // Создаем действие ввода для стрельбы, используя клавишу "пробел".
        _shootAction = new InputAction("shoot", binding: "<keyboard>/space");
        _shootAction.performed += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.started += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.canceled += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.Enable();

        // Действие для рывка(CTRL)
        _dashAction = new InputAction("dash", binding: "<keyboard>/ctrl");
        _dashAction.performed += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.started += context => { _dashInput = context.ReadValue<float>(); };
        _dashAction.canceled += context => { _dashInput = 0f; };
        _dashAction.Enable();
    }

    protected override void OnStopRunning()
    {
        // Отключаем действия ввода при остановке системы.
        _moveAction.Disable();
        _shootAction.Disable();
        _dashAction.Disable();
    }

    protected override void OnUpdate()
    {
        // Обновляем компонент InputData у всех сущностей, чтобы отразить текущие значения ввода.
        Entities.With(_inputQuery).ForEach(
            (Entity entity, ref InputData inputData) =>
            {
                inputData.Move = _moveInput;
                inputData.Shoot = _shootInput;
                inputData.Dash = _dashInput;
            });
    }
}
