using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls.Gameplay
{
    public class PlayerStateDead : PlayerState
    {
        public override void OnStateEnter(StateMachineController controller)
        {
            var player = controller.GetComponent<Player>();
            player.Notify(Message.Combat_Death);
            foreach (MonoBehaviour m in player.GetComponents<MonoBehaviour>())
                m.enabled = false;
            foreach (Collider c in player.GetComponentsInChildren<Collider>())
                c.enabled = false;
        }
    }
}