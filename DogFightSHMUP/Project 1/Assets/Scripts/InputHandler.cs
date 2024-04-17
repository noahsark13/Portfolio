using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    [SerializeField]
    MovementHandler movementHandler;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementHandler.MoveDirection(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        gameObject.GetComponent<ProjectileSpawner>().Fire();
    }


}
