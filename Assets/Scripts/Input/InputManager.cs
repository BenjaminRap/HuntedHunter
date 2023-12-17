using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMotor motor;
    PlayerInputActions   input;

    private void    Awake()
    {
        input = new PlayerInputActions();
        motor = GetComponent<PlayerMotor>();
        input.OnFoot.Enable();
        input.Sleeping.WakeUp.performed += WakeUp;
        input.OnFoot.Jump.started += JumpStart;
        input.OnFoot.Jump.canceled += JumpEnd;
    }

    public void OnFootInput()
    {
        input.OnFoot.Enable();
        input.Sleeping.Disable();
    }

    public void SleepingInput()
    {
        input.OnFoot.Disable();
        input.Sleeping.Enable();
    }

    private void    WakeUp(InputAction.CallbackContext context)
    {
        GameObject.FindGameObjectsWithTag("Levels")[0].GetComponent<Level>().Space();
    }

    private void    JumpStart(InputAction.CallbackContext context)
    {
        StartCoroutine(motor.JumpStart());
    }

    private void    JumpEnd(InputAction.CallbackContext context)
    {
        motor.JumpEnd();
    }

    private void    FixedUpdate()
    {
        Vector2 move_input;

        move_input = input.OnFoot.Movement.ReadValue<Vector2>();
        motor.ProcessMove(move_input);
    }
}
