using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine {

    private Player player;

    protected override void Initialize()
    {
        player = GetComponentInParent<Player>();
    }

}
