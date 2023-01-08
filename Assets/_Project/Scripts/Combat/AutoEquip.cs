using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEquip : MonoBehaviour
{
    public ScytheData itemToEquip;
    public ScytheManager playerScythe;
    private void Start()
    {
        if (playerScythe.actualScythe == null)
        {
            GameEvents.Instance.OnEquipScythe(itemToEquip);
        }
    }
}
