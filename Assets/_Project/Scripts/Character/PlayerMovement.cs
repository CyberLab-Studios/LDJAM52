using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public Transform camOffset;
    public Transform body;

    CharacterController controller;
    Vector3 moveVector;
    Vector2 moveDirection;
    public void OnMove(CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var x = camOffset.right * moveDirection.x;
        var z = camOffset.forward * moveDirection.y;

        moveVector = (x + z).normalized;
        moveVector.y = 0;
        controller.Move(movementSpeed * Time.deltaTime * moveVector);

        if (moveVector != Vector3.zero)
        {
            Quaternion newRot = Quaternion.LookRotation(moveVector);
            body.rotation = newRot;
        }
    }
}
