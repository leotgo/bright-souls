using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{
    public class PlayerStateMachine : StateMachine
    {
        private Player player;

        protected override void Initialize()
        {
            player = GetComponentInParent<Player>();
        }
    }
}
