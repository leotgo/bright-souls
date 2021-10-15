using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{
    public abstract class Attack : MonoBehaviour
    {
        public AttackName Name
        {
            get
            {
                return attackName;
            }
        }

        public string AnimatorString
        {
            get
            {
                return animatorString;
            }
        }

        public ICombatCharacter Source
        {
            get
            {
                return source;
            }
        }

        public AttackData Data { get => data; }

        [SerializeField] private AttackData data;
        [SerializeField] private AttackName attackName = AttackName.None;
        [SerializeField] private string animatorString = "animator_string";
        protected ICombatCharacter source;

        public virtual void Activate(ICombatCharacter source)
        {
            this.source = source;
            source.transform.GetComponent<Animator>().SetTrigger(animatorString);
        }
    }
}