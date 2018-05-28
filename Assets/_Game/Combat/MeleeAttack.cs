using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class MeleeAttack : Attack, IHitboxOwner, IObserver
{
    [SerializeField] private Hitbox hitbox;

    public int minDamage = 10;
    public int maxDamage = 15;
    public int staggerDamage = 50;
    public int blockStaminaDamage = 0;
    public int staminaCost = 0;

    private bool attackStarted;

    private void Start()
    {
        if (!hitbox)
            hitbox = GetComponentInChildren<Hitbox>();

        attackStarted = false;

        this.Observe(Message.Combat_AttackStart);
        this.Observe(Message.Combat_AttackEnd);
        this.Observe(Message.Combat_AttackActivateHitbox);
        this.Observe(Message.Combat_AttackDeactivateHitbox);
        this.Observe(Message.Combat_DetectHit);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if (!Source)
            return;

        bool senderIsSource = (Object)sender == Source || (Object)sender == Source.GetComponent<Animator>();
        if (!senderIsSource && (Object)sender != hitbox)
            return;

        switch (msg)
        {
            case Message.Combat_AttackStart:
            case Message.Combat_AttackEnd:
            case Message.Combat_AttackActivateHitbox:
            case Message.Combat_AttackDeactivateHitbox:
                string atkName = (string)args[0];
                if (Name.ToString() != atkName)
                    return;
                break;
            case Message.Combat_DetectHit:
                if (!attackStarted)
                    return;
                break;
        }

        switch (msg)
        {
            case Message.Combat_AttackStart:
                attackStarted = true;
                var staminaManager = Source.GetComponent<StaminaBehaviour>();
                if (staminaManager)
                    staminaManager.Value -= staminaCost;
                break;
            case Message.Combat_AttackEnd:
                attackStarted = false;
                break;
            case Message.Combat_AttackActivateHitbox:
                hitbox.IsActive = true;
                break;
            case Message.Combat_AttackDeactivateHitbox:
                hitbox.IsActive = false;
                break;
            case Message.Combat_DetectHit:
                IHittable target = (IHittable)args[0];
                if (target is Character)
                {
                    if ((Character)target == Source)
                        return;
                }
                target.OnGetHit(this);
                break;

        }
    }

    public override void Activate(Character source)
    {
        var staminaManager = source.GetComponent<StaminaBehaviour>();
        if (staminaManager)
        {
            if (staminaManager.Value <= 0)
                return;
        }
        base.Activate(source);
    }
}