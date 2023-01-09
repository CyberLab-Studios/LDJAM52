using CyberLabStudios.Game.Utilities;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public Transform body;
    public float weaponCooldown;
    public float animationTime = 3;
    public AudioData swoosh;
    public float audioDelay = .5f;
    bool hasScythe = false;
    float tempCooldown;
    float damage;
    bool hasControl = true;

    public void OnAttack(CallbackContext ctx)
    {
        if (ctx.started && tempCooldown <= 0 && hasScythe && hasControl)
        {
            anim.SetTrigger("Attack");
            Invoke("MakeAttack", audioDelay);
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

    void MakeAttack()
    {
        Utility.PlayOneShotAudio(gameObject, swoosh.clip, swoosh.volume);
        if (Physics.Raycast(new Ray(body.position + body.up, body.forward), out RaycastHit hit, 3f))
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.TryGetComponent(out IEnemy enemy))
            {
                Debug.Log(enemy);

                enemy.TakeDamage(damage);
            }
        }
    }

    private void GetScytheData(ScytheData scythe)
    {
        weaponCooldown = scythe.Cooldown;
        damage = scythe.Damage;
        hasScythe = true;
    }

    public void Update()
    {
        if (tempCooldown >= 0)
            tempCooldown -= Time.deltaTime;
    }
}
