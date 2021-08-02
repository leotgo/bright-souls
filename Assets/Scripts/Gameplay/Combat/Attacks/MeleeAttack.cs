using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls
{
    public class MeleeAttack : Attack, IHitboxOwner, IObserver
    {
        [SerializeField] private Hitbox hitbox;

        private bool attackStarted;

        private void Start()
        {
            if (!hitbox)
            {
                hitbox = GetComponentInChildren<Hitbox>();
            }

            attackStarted = false;

            this.Observe(Message.Combat_AttackStart);
            this.Observe(Message.Combat_AttackEnd);
            this.Observe(Message.Combat_AttackActivateHitbox);
            this.Observe(Message.Combat_AttackDeactivateHitbox);
            this.Observe(Message.Combat_DetectHit);
        }

        public void OnAttackStart()
        {

        }

        public void OnAttackEnd()
        {

        }

        public void OnActivateHitbox()
        {

        }

        public void OnDeactivateHitbox(string atkName)
        {
            if (Name.ToString() != atkName)
            {
                return;
            }
        }

        public void OnDetectHit()
        {

        }

        public void OnNotification(object sender, Message msg, params object[] args)
        {
            if (!Source)
            {
                return;
            }

            bool senderIsSource = (Object)sender == Source || (Object)sender == Source.GetComponent<Animator>();
            if (!senderIsSource && (Object)sender != hitbox)
            {
                return;
            }

            switch (msg)
            {
                case Message.Combat_AttackStart:
                    {
                    attackStarted = true;
                    var staminaManager = Source.GetComponent<StaminaBehaviour>();
                    }
                    break;
                case Message.Combat_AttackEnd:
                    {
                        attackStarted = false;
                    }
                    break;
                case Message.Combat_AttackActivateHitbox:
                    {
                        hitbox.IsActive = true;
                    }
                    break;
                case Message.Combat_AttackDeactivateHitbox:
                    {
                        hitbox.IsActive = false;
                    }
                    break;
                case Message.Combat_DetectHit:
                    {
                        var target = (IHittable)args[0];
                        if (target is Character)
                        {
                            var targetCharacter = (Character)target;
                            var targetFaction = targetCharacter.Attributes.GetAttribute<FactionAttribute>().Value;
                            var sourceFaction = Source.Attributes.GetAttribute<FactionAttribute>().Value;

                            if(targetFaction == sourceFaction)
                            {
                                Debug.LogFormat("COMBAT: {0} tried to hit {1}, but both were same faction.", this.Source, target);
                            }
                            else
                            {
                                target.OnGetHit(this);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Activate(Character source)
        {
            var staminaManager = source.GetComponent<StaminaBehaviour>();
            if (staminaManager)
            {
                if (staminaManager.Value <= 0)
                {
                    return;
                }
            }
            base.Activate(source);
        }
    }
}