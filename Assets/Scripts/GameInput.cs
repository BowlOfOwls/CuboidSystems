using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnCutAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInventoryAction;
    public event EventHandler OnInventoryClosedAction;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.KeyBoard.Enable();
        playerInput.KeyBoard.Cut.performed += Cut_performed;
        playerInput.KeyBoard.Interact.performed += Interact_performed;
        playerInput.KeyBoard.Inventory.performed += Inventory_performed;
        playerInput.KeyBoard.InventoryClose.performed += Inventory_Closed_performed;
    }

    private void Inventory_Closed_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInventoryClosedAction?.Invoke(this, EventArgs.Empty);
    }

    private void Inventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInventoryAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Cut_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCutAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {

        Vector2 inputVector = playerInput.KeyBoard.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;


        return inputVector;
    }
}
