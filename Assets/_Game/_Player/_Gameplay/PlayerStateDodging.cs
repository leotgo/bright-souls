using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDodging : State {

    private Player player;
    
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public override void OnStateEnter()
    {
        player.Stagger.BonusHealth += player.Stagger.dodgeBonusHealth;
        player.Stamina.Value -= player.Combat.dodgeStaminaCost;
    }

    public override void OnStateExit()
    {
        player.Stagger.BonusHealth -= player.Stagger.dodgeBonusHealth;
    }
}
