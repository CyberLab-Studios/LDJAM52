using CyberLabStudios.Game.Utilities;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public float weaponCooldown;
    public float animationTime = 3;
    public AudioData swoosh;
    public float audioDelay = .5f;
    bool hasScythe = false;
    float tempCooldown;
    bool hasControl = true;

    public void OnAttack(CallbackContext ctx)
    {
        if (ctx.started && tempCooldown <= 0 && hasScythe && hasControl)
        {
            anim.SetTrigger("Attack");
            Invoke("PlayAudio", audioDelay);
            tempCooldown = weaponCooldown;
            GameEvents.Instance.OnAttack(animationTime);
        }
    }

    private void Start()
    {
        GameEvents.Instance.onEquipScythe += GetScytheData;
        GameEvents.Instance.onDialogueStart += NotControl;
        GameEvents.Instance.onDialogueEnd += GiveControl;
    }

    private void GiveControl()
    {
        hasControl = true;
    }

    private void NotControl()
    {
        hasControl = false;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onEquipScythe -= GetScytheData;
        GameEvents.Instance.onDialogueStart -= NotControl;
        GameEvents.Instance.onDialogueEnd -= GiveControl;
    }

    void PlayAudio()
    {
        Utility.PlayOneShotAudio(gameObject, swoosh.clip, swoosh.volume);
    }

    private void GetScytheData(ScytheData scythe)
    {
        weaponCooldown = scythe.Cooldown;
        hasScythe = true;
    }

    public void Update()
    {
        if (tempCooldown >= 0)
            tempCooldown -= Time.deltaTime;
    }
}
