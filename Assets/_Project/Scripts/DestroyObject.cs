using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public List<Rigidbody> rigidbodies;

    public void ExplodeItem()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
        Destroy(gameObject, 5);
    }
}
