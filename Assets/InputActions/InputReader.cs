using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private GameInputActions inputActions;

    // Event to handle movement based on player input
    public event Action<Vector2> MoveEvent;

    private void Awake()
    {
        inputActions = new GameInputActions();
    }

    void OnEnable()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
        else
        {
            inputActions.Enable();

            // WASD key inputs (for testing in Unity Editor)
            inputActions.Player.Move.performed += OnMove;
            inputActions.Player.Move.canceled += OnMove;
        }
    }

    void OnDisable()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.DisableDevice(Accelerometer.current);
        }
        else
        {
            inputActions.Player.Move.performed -= OnMove;
            inputActions.Player.Move.canceled -= OnMove;

            inputActions.Disable();
        }
    }

    private void Update()
    {
        // On iOS, use the accelerometer for tilting the board
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        if (Accelerometer.current != null)
        {
            //Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            Vector2 accelerometerInput = new Vector2(Input.acceleration.x, Input.acceleration.y);
            MoveEvent?.Invoke(accelerometerInput);
        }
        // On macOS (or Unity Editor), use the WASD keys for testing
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Vector2 input = new Vector2(0, 0);
            if (Keyboard.current.wKey.isPressed) input.y = 1;
            if (Keyboard.current.sKey.isPressed) input.y = -1;
            if (Keyboard.current.aKey.isPressed) input.x = -1;
            if (Keyboard.current.dKey.isPressed) input.x = 1;

            MoveEvent?.Invoke(input);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Read the vector value from the action context (for other controls, if any)
        Vector2 value = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(value);
    }

}
