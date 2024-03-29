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
        StopAllCoroutines();
        StartCoroutine(Move(target));

    }

    IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.5f)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime));
            anim?.SetFloat("Speed", 1);
            yield return null;
        }
        anim?.SetFloat("Speed", 0);
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
