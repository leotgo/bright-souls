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
            if (Source == null)
            {
                return;
            }

            bool senderIsSource = sender == Source || (Object)sender == Source.transform.GetComponent<Animator>();
            if (!senderIsSource && (Object)sender != hitbox)
            {
                return;
            }

            switch (msg)
            {
                case Message.Combat_AttackStart:
                    {
                    attackStarted = true;
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
                        if (target is ICombatCharacter)
                        {
                            var targetCharacter = (ICombatCharacter)target;
                            var targetFaction = targetCharacter.Faction.Value;
                            var sourceFaction = Source.Faction.Value;

                            if(targetFaction == sourceFaction)
                            {
                                Debug.Log($"COMBAT: {Source} tried to hit {target}, but both were from same faction.");
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

        public override void Activate(ICombatCharacter source)
        {
            var stamina = source.Stamina;
            if (stamina.Value > 0)
            {
                base.Activate(source);
            }
        }
    }
}