using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float gravity = -9.8f;
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

    private void Start()
    {
        GameEvents.Instance.onAttack += IsAttacking;
        GameEvents.Instance.onDialogueStart += NotControl;
        GameEvents.Instance.onDialogueEnd += GiveControl;
        GameEvents.Instance.onLookAt += RotateTo;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onAttack -= IsAttacking;
        GameEvents.Instance.onDialogueStart -= NotControl;
        GameEvents.Instance.onDialogueEnd -= GiveControl;
        GameEvents.Instance.onLookAt -= RotateTo;
    }

    private void GiveControl()
    {
        ToggleControl(true);
    }

    private void NotControl()
    {
        ToggleControl(false);
    }

    private void IsAttacking(float time)
    {
        StopAllCoroutines();
        StartCoroutine(StopControlsFor(time));
    }

    IEnumerator StopControlsFor(float time)
    {
        hasControl = false;
        yield return new WaitForSeconds(time);
        hasControl = true;
    }

    private void Update()
    {
        var x = camOffset.right * moveDirection.x;
        var z = camOffset.forward * moveDirection.y;

        moveVector = (x + z).normalized;

        if (!controller.isGrounded)
            moveVector.y = gravity * Time.deltaTime;
        else
            moveVector.y = 0;


        if (hasControl)
        {
            anim.SetFloat("Speed", moveDirection.magnitude);
            controller.Move(movementSpeed * Time.deltaTime * moveVector);

            var dir = new Vector3(moveVector.x, 0, moveVector.z);
            dir.y = 0;

            if (dir != Vector3.zero)
            {
                Quaternion newRot = Quaternion.LookRotation(dir, transform.up);
                body.rotation = newRot;
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    public void RotateTo(Vector3 target)
    {
        target.y = 0;

        if (target != Vector3.zero)
        {            
            body.LookAt(target);
        }
    }
}
