using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public Transform camOffset;
    public Transform body;
    public Animator anim;

    CharacterController controller;
    Vector3 moveVector;
    Vector2 moveDirection;
    bool hasControl = true;

    public void OnMove(CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    public void ToggleControl(bool _hasControl = true)
    {
        hasControl = _hasControl;
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


        if (hasControl)
        {
            anim.SetFloat("Speed", moveDirection.magnitude);
            controller.Move(movementSpeed * Time.deltaTime * moveVector);

            if (moveVector != Vector3.zero)
            {
                Quaternion newRot = Quaternion.LookRotation(moveVector);
                body.rotation = newRot;
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }
}
