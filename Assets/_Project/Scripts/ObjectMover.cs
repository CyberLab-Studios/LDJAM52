using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float movementSpeed = 3;
    public Animator anim;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector3 target)
    {
        target.y = transform.position.y;
        StartCoroutine(Move(target));
        
    }

    IEnumerator Move(Vector3 target)
    {
        while(Vector3.Distance(transform.position, target) > 0.5f)
        {
            rb.MovePosition(transform.position + target * movementSpeed * Time.deltaTime);
            anim?.SetFloat("Speed", rb.velocity.magnitude);
            yield return null;
        }
    }

    public void MoveTo(Transform target)
    {
        LookAt(target);
        MoveTo(target.position);
    }

    public void LookAt(Transform target)
    {
        transform.LookAt(target.position);
    }
}
