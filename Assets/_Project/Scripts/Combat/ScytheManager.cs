using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheManager : MonoBehaviour
{
    public Transform handParent;
    public GameObject actualScythe;

    private void Awake()
    {
        GameEvents.Instance.onEquipScythe += EquipScythe;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onEquipScythe -= EquipScythe;
    }

    public void EquipScythe(ScytheData scythe)
    {
        if (actualScythe != null)
            Destroy(actualScythe);

        actualScythe = Instantiate(scythe.ScythePrefab, handParent);
    }
}
