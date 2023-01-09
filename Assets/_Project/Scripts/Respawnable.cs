using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    public GameObject actualObject;
    public GameObject prefabObject;
    public float respawnAfter;

    bool isSpawning = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (actualObject == null && !isSpawning)
        {
            StartCoroutine(SpawnObjectAfter());
        }
    }

    IEnumerator SpawnObjectAfter()
    {
        isSpawning = true;
        yield return new WaitForSeconds(respawnAfter);
        actualObject = Instantiate(prefabObject, transform);
        actualObject.transform.localPosition = Vector3.zero;
        actualObject.transform.localRotation = Quaternion.identity;
    }
}
