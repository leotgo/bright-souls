using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class PlayerStateDead : State {

    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public override void OnStateEnter()
    {
        player.Notify(Message.Combat_Death);
        foreach (MonoBehaviour m in player.GetComponents<MonoBehaviour>())
            m.enabled = false;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            c.enabled = false;
    }
}
