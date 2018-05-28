using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    [SerializeField] private AttackName _attackName = AttackName.None;
    public AttackName Name {
        get {
            return _attackName;
        }
    }
    [SerializeField] private string _animatorString = "animator_string";
    public string AnimatorString {
        get {
            return _animatorString;
        }
    }

    protected Character _source;
    public Character Source {
        get {
            return _source;
        }
    }

    public virtual void Activate(Character source)
    {
        _source = source;
        source.GetComponent<Animator>().SetTrigger(_animatorString);
    }
}
