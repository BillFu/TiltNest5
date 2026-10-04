using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReaderWASD : MonoBehaviour
{
    private GameInputActions inputActions;

    //  
    public event Action<Vector2> MoveEvent;

    private void Awake()
    {
        inputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        //  
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(value);
        
        //Debug.Log(value);
    }
}
