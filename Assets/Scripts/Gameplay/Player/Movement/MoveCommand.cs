using UnityEngine;

namespace BrightSouls.Player
{
    public class MoveCommand : PlayerCommand<Vector2>
    {
        public MoveCommand(PlayerComponentIndex owner) : base(owner) { }

        public override bool CanExecute()
        {
            bool canMove = !player.State.IsDodging && !player.State.IsStaggered && !player.State.IsDead;
            return canMove;
        }

        public override void Execute(Vector2 inputDirection)
        {
            if (this.CanExecute())
            {
                player.Motor.PerformGroundMovement(inputDirection);
            }
        }
    }
}