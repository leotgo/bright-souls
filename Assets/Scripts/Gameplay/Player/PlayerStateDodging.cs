using UnityEngine;
using UnityPatterns.FiniteStateMachine;

namespace BrightSouls.Player
{
    public class PlayerStateDodging : PlayerState
    {
        public override void OnStateEnter(StateMachineController controller)
        {
            //player.Attributes.Poise.BonusHealth += player.Attributes.Poise.dodgeBonusHealth;
            //player.Attributes.Stamina.Value -= player.Combat.Data.DodgeStaminaCost;
        }

        public override void OnStateExit(StateMachineController controller)
        {
            //player.Attributes.Poise.BonusHealth -= player.Attributes.Poise.dodgeBonusHealth;
        }
    }
}