using UnityEngine;
using UnityEngine.InputSystem;

public class CarPlayerDriver : CarDriver
{
    private CarInput carInput;

    private void Awake()
    {
        carInput = new CarInput();
    }


    private void OnEnable()
    {
        carInput.Enable();
        carInput.Main.Move.performed += ctx => OnMove(ctx);
    }

    private void OnDisable()
    {
        carInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        desiredDirection = inputValue;
    }








}
