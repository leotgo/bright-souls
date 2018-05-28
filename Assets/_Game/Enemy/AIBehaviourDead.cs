using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviourDead : AIBehaviour {

    public override void BehaviourStart()
    {
        owner.enabled = false;
        foreach (Collider c in owner.GetComponentsInChildren<Collider>())
            c.enabled = false;
    }

    public override void BehaviourUpdate()
    {
        
    }

    public override void BehaviourEnd()
    {
        
    }
}
