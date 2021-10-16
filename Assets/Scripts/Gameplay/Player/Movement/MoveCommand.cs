using UnityEngine;

namespace BrightSouls.Gameplay
{
    public class MoveCommand : PlayerCommand<Vector2>
    {
        public MoveCommand(Player owner) : base(owner) { }

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