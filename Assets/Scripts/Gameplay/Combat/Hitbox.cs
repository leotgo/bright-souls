using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

namespace BrightSouls
{

    public enum HitboxType
    {
        Hitbox,
        Hurtbox
    }

    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour
    {

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    registeredHits.Clear();
                }
                _isActive = value;
                coll.enabled = value;
            }
        }

        public HitboxType type = HitboxType.Hitbox;

        private bool _isActive = false;
        private IHitboxOwner owner;
        private List<IHitboxOwner> registeredHits = new List<IHitboxOwner>();
        private Collider coll;

        private void Start()
        {
            AssignComponents();
            if (type == HitboxType.Hurtbox)
                IsActive = true;
            else
                IsActive = false;
        }

        private void AssignComponents()
        {
            coll = GetComponent<Collider>();
            owner = GetComponentInParent<IHitboxOwner>();

            if (!coll.isTrigger)
            {
                Debug.LogError("ERROR: Hitbox \"" + this.name + "\" Collider is not trigger! Deactivating component.");
                this.enabled = false;
            }
            else if (owner == null)
            {
                Debug.LogError("ERROR: Hitbox \"" + this.name + "\" has no owner! Deactivating component.");
                this.enabled = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (type != HitboxType.Hitbox || IsActive == false)
                return;

            var hitbox = other.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                if (hitbox.type == HitboxType.Hurtbox && owner != hitbox.owner)
                    OnDetectHit(hitbox.owner);
            }
        }

        internal void OnDetectHit(IHitboxOwner target)
        {
            if (!registeredHits.Contains(target))
            {
                registeredHits.Add(target);
                this.Notify(Message.Combat_DetectHit, target);
            }
        }

    }

    public interface IHitboxOwner
    {
    }

    public interface IHitter : IHitboxOwner
    {
    }

    public interface IHittable : IHitboxOwner
    {
        void OnGetHit(Attack attack);
    }
}