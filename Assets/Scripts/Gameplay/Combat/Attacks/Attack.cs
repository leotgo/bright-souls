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
                return _attackName;
            }
        }

        public string AnimatorString
        {
            get
            {
                return _animatorString;
            }
        }

        public Character Source
        {
            get
            {
                return m_source;
            }
        }

        public AttackData Data { get => data; }

        [SerializeField] private AttackData data;
        [SerializeField] private AttackName _attackName = AttackName.None;
        [SerializeField] private string _animatorString = "animator_string";
        protected Character m_source;

        public virtual void Activate(Character source)
        {
            m_source = source;
            source.GetComponent<Animator>().SetTrigger(_animatorString);
        }
    }
}